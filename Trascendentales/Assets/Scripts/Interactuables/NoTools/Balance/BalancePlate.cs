using System.Collections.Generic;
using UnityEngine;

public class BalancePlate : MonoBehaviour
{
    public Balance balance; // Referencia al script principal de la balanza
    [SerializeField] float plateWeight;
    private List<WeightedObject> objectsOnPlate = new List<WeightedObject>();

    private void Update()
    {
        ChangeTotalWeight();
    }
    private void OnTriggerEnter(Collider other)
    {
        WeightedObject weightedObject = other.GetComponent<WeightedObject>();
        if (weightedObject != null)
        {
            objectsOnPlate.Add(weightedObject);
            //ChangeTotalWeight(); // Notifica a la balanza que el peso ha cambiado
        }
    }

    private void OnTriggerExit(Collider other)
    {
        WeightedObject weightedObject = other.GetComponent<WeightedObject>();
        if (weightedObject != null)
        {
            objectsOnPlate.Remove(weightedObject);
            //ChangeTotalWeight(); // Notifica a la balanza que el peso ha cambiado
        }
    }

    public void ChangeTotalWeight()
    {
        float totalWeight = 0f;
        foreach (WeightedObject obj in objectsOnPlate)
        {
            totalWeight += obj.weight;
        }
        balance.UpdatePlateWeight(this, totalWeight + plateWeight);
    }
}
