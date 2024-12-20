using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancePlate : MonoBehaviour
{
    private bool isActive = false;
    public Balance balance; // Referencia al script principal de la balanza
    public GameObject collider; // Referencia al script principal de la balanza
    [SerializeField] float plateWeight;
    private List<Rigidbody> objectsOnPlate = new List<Rigidbody>();

    private void Start()
    {
        // Iniciar la corrutina para verificar objetos cada segundo
        StartCoroutine(CheckObjectsOnPlate());
    }
    private void Update()
    {
        ChangeTotalWeight();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(!isActive)
        {
            StartCoroutine(CheckObjectsOnPlate());
            isActive = true;
        }
        Rigidbody weightedObject = other.GetComponent<Rigidbody>();
        if (weightedObject != null)
        {
            objectsOnPlate.Add(weightedObject);
            //ChangeTotalWeight(); // Notifica a la balanza que el peso ha cambiado
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody weightedObject = other.GetComponent<Rigidbody>();
        if (weightedObject != null)
        {
            objectsOnPlate.Remove(weightedObject);
            //ChangeTotalWeight(); // Notifica a la balanza que el peso ha cambiado
        }
    }

    public void ChangeTotalWeight()
    {
        float totalWeight = 0f;
        foreach (Rigidbody obj in objectsOnPlate)
        {
            if(obj != null)
                totalWeight += Mathf.Round(obj.mass);
        }
        balance.UpdatePlateWeight(this, totalWeight + plateWeight);
    }
    private IEnumerator CheckObjectsOnPlate()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            // Recorrer la lista al revés para eliminar objetos de manera segura
            for (int i = objectsOnPlate.Count - 1; i >= 0; i--)
            {
                if (objectsOnPlate[i] == null || !objectsOnPlate[i].gameObject.activeSelf)
                {
                    objectsOnPlate.RemoveAt(i);
                }
            }
            collider.SetActive((objectsOnPlate.Count > 1) ? true : false);
        }
    }

}
