using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class PoolItem
    {
        public GameObject prefab;
        public int maxInstances = 2;
    }

    [Header("Pool Configuration")]
    public List<PoolItem> poolItems;

    private Dictionary<GameObject, List<GameObject>> poolDictionary = new Dictionary<GameObject, List<GameObject>>();
    private GameObject lastActiveObject = null; // �ltimo objeto activado

    private void Start()
    {
        // Inicializar el pool
        foreach (var item in poolItems)
        {
            if (!poolDictionary.ContainsKey(item.prefab))
            {
                poolDictionary[item.prefab] = new List<GameObject>();
                for (int i = 0; i < item.maxInstances; i++)
                {
                    GameObject obj = Instantiate(item.prefab);
                    obj.transform.parent = transform;
                    obj.SetActive(false);
                    poolDictionary[item.prefab].Add(obj);
                }
            }
        }
    }

    /// <summary>
    /// Activa un objeto del tipo solicitado en la posici�n y rotaci�n indicadas.
    /// Si un objeto ya est� activo, activar� el siguiente y desactivar� el anterior con animaci�n.
    /// </summary>
    public void ActivateObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {

        if (!poolDictionary.ContainsKey(prefab))
        {
            Debug.LogWarning($"Prefab {prefab.name} no est� configurado en el pool.");
            return;
        }
        // Desactivar el �ltimo objeto activado, si existe
        if (lastActiveObject != null)
        {
            StartCoroutine(DeactivateWithAnimation(lastActiveObject));
            lastActiveObject = null; // Limpiar referencia despu�s de iniciar la desactivaci�n
        }

        var objects = poolDictionary[prefab];
        foreach (var obj in objects)
        {
            if (!obj.activeSelf)
            {
                // Configurar el objeto y activarlo
                obj.transform.position = position;
                obj.transform.rotation = rotation;
                obj.transform.localScale = Vector3.one; // Restablecer escala
                obj.SetActive(true);
                lastActiveObject = obj; // Guardar referencia al objeto activado
                return;
            }
        }
    }

    private System.Collections.IEnumerator DeactivateWithAnimation(GameObject obj)
    {
        // Disparar una animaci�n si tiene un Animator
        Animator animator = obj.GetComponent<Animator>();
        if (animator != null)
        {
            animator.Play("DespawnPlatform");
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        }

        // Desactivar el objeto y restablecer su escala y rotaci�n
        obj.SetActive(false);
        obj.transform.localScale = Vector3.one;
        obj.transform.rotation = Quaternion.identity;
    }
}
