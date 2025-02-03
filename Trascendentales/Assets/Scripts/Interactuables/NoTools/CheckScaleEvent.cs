using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CheckScaleEvent : MonoBehaviour
{
    [SerializeField] private List<ScaleCheck> objects;
    public UnityEvent OnCompleted;
    public UnityEvent OnBreak;
    private bool isCompleted = false;
    [SerializeField] private bool blockAfterComplete = false;
    [SerializeField] private float rangeAccuarcy = 0.5f;

    void Update()
    {
        if (isCompleted && blockAfterComplete)
            return;

        CheckScales();
    }

    private void CheckScales()
    {
        // Verificar escalas X
        bool allXScalesCorrect = CheckXScales();

        // Verificar escalas Y
        bool allYScalesCorrect = CheckYScales();

        // Verificar escalas Z
        bool allZScalesCorrect = CheckZScales();

        // Si todas las escalas se cumplen
        if (allXScalesCorrect && allYScalesCorrect && allZScalesCorrect)
        {
            if (!isCompleted)
            {
                isCompleted = true;
                OnCompleted?.Invoke();
            }
        }
        // Si ya estaba completado y ahora no cumple
        else if (isCompleted)
        {
            isCompleted = false;
            OnBreak?.Invoke();
        }
    }

    private bool CheckXScales()
    {
        // Filtrar objetos que necesitan chequeo de escala X
        var xScaleObjects = objects.Where(obj => obj.needXScale).ToList();

        // Si no hay objetos para chequear X, considerar X como completado
        if (xScaleObjects.Count == 0)
            return true;

        // Verificar si TODOS los objetos con escala X cumplen
        return xScaleObjects.TrueForAll(obj =>
            Vector3.Distance(
                obj.prefab.transform.localScale, new Vector3(obj.goalScaleX, obj.prefab.transform.localScale.y, obj.prefab.transform.localScale.z)
            )
            <= rangeAccuarcy
        );
    }

    private bool CheckYScales()
    {
        // Filtrar objetos que necesitan chequeo de escala Y
        var yScaleObjects = objects.Where(obj => obj.needYScale).ToList();

        // Si no hay objetos para chequear Y, considerar Y como completado
        if (yScaleObjects.Count == 0)
            return true;

        // Verificar si TODOS los objetos con escala Y cumplen
        return yScaleObjects.TrueForAll(obj =>
            Vector3.Distance(
                obj.prefab.transform.localScale, new Vector3(obj.prefab.transform.localScale.x, obj.goalScaleY, obj.prefab.transform.localScale.z)
            )
            <= rangeAccuarcy
        );
    }

    private bool CheckZScales()
    {
        // Filtrar objetos que necesitan chequeo de escala Z
        var zScaleObjects = objects.Where(obj => obj.needZScale).ToList();

        // Si no hay objetos para chequear Z, considerar Z como completado
        if (zScaleObjects.Count == 0)
            return true;

        // Verificar si TODOS los objetos con escala Z cumplen
        return zScaleObjects.TrueForAll(obj =>
            Vector3.Distance(
                obj.prefab.transform.localScale, new Vector3(obj.prefab.transform.localScale.x, obj.prefab.transform.localScale.y, obj.goalScaleZ)
            )
            <= rangeAccuarcy
        );
    }
}

[System.Serializable]
public class ScaleCheck
{
    public GameObject prefab;
    public float goalScaleX;
    public float goalScaleY;
    public float goalScaleZ;
    public bool needXScale = false;
    public bool needYScale = false;
    public bool needZScale = false;

    public ScaleCheck(GameObject prefab, float goalScaleX, float goalScaleY, float goalScaleZ)
    {
        this.prefab = prefab;
        this.goalScaleX = goalScaleX;
        this.goalScaleY = goalScaleY;
        this.goalScaleZ = goalScaleZ;
    }
}

