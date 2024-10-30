using UnityEngine;
using System.Collections;

public class ShaderController : MonoBehaviour
{
    public Material material; 
    [SerializeField] private float duration = 2f;


    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(CambiarValorGradualmente(0f, 1f, duration));
        }
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
