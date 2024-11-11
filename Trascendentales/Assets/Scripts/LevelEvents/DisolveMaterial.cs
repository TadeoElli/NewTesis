using UnityEngine;
using System.Collections;

public class DisolveMaterial : MonoBehaviour
{
    public Material material; 
    [SerializeField] private float duration = 2f;


    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    public void StartDisolve()
    {
        Debug.Log("trigger ");
        StartCoroutine(CambiarValorGradualmente(0f,2f, duration));
    }

    private IEnumerator CambiarValorGradualmente(float inicio, float fin, float tiempo)
    {
        float elapsed = 0f;

        while (elapsed < tiempo)
        {
            float valor = Mathf.Lerp(inicio, fin, elapsed / tiempo);

            material.SetFloat("DisolveStrength", valor);

            elapsed += Time.deltaTime;

            yield return null;
        }

        material.SetFloat("DisolveStrength", fin);
    }
}
