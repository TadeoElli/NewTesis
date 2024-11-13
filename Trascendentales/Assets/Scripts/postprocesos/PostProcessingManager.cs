using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessingManager : MonoBehaviour
{
    [SerializeField] private Material pp_MaterialDimensionChanger;
    [SerializeField] private float pp_intensity;
    public void ActivateChangeDimension()
    {
        pp_intensity = 1f; //1.1 para true
        pp_MaterialDimensionChanger.SetFloat("_DimensionChanger", Mathf.Clamp(pp_intensity, 0, 1));
    }

    public void DesactivateChangeDimension()
    {
        pp_intensity = 0.0f; //0.0f para false
        pp_MaterialDimensionChanger.SetFloat("_DimensionChanger", Mathf.Clamp(pp_intensity, 0, 1));
    }
}
