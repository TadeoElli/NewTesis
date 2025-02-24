using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CheckPositionEvent : MonoBehaviour
{
    [SerializeField] private List<PositionCheck> objects;
    public UnityEvent OnCompleted;
    public UnityEvent OnBreak;
    private bool isCompleted = false;
    [SerializeField] private bool blockAfterComplete = false;
    [SerializeField] private float tolerance;

    void Update()
    {
        if (isCompleted && blockAfterComplete)
            return;

        CheckPositions();
    }
    private void OnDisable()
    {
        OnBreak?.Invoke();
        isCompleted = false;
    }

    private void CheckPositions()
    {
        // Verificar posiciones X
        bool allXPositionsCorrect = CheckXPositions();

        // Verificar posiciones Y
        bool allYPositionsCorrect = CheckYPositions();

        // Verificar posiciones Z
        bool allZPositionsCorrect = CheckZPositions();

        // Si todas las posiciones se cumplen
        if (allXPositionsCorrect && allYPositionsCorrect && allZPositionsCorrect)
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

    private bool CheckXPositions()
    {
        // Filtrar objetos que necesitan chequeo de posición X
        var xPositionObjects = objects.Where(obj => obj.needXPosition).ToList();

        // Si no hay objetos para chequear X, considerar X como completado
        if (xPositionObjects.Count == 0)
            return true;

        // Verificar si TODOS los objetos con posición X cumplen
        return xPositionObjects.TrueForAll(obj =>
            Vector3.Distance(
                obj.prefab.transform.position, new Vector3(obj.goalPositionX, obj.prefab.transform.position.y, obj.prefab.transform.position.z)
            )
            <= tolerance
        );
    }

    private bool CheckYPositions()
    {
        // Filtrar objetos que necesitan chequeo de posición Y
        var yPositionObjects = objects.Where(obj => obj.needYPosition).ToList();

        // Si no hay objetos para chequear Y, considerar Y como completado
        if (yPositionObjects.Count == 0)
            return true;

        // Verificar si TODOS los objetos con posición Y cumplen
        return yPositionObjects.TrueForAll(obj =>
            Vector3.Distance(
                obj.prefab.transform.position, new Vector3(obj.prefab.transform.position.x, obj.goalPositionY, obj.prefab.transform.position.z)
            )
            <= tolerance
        );
    }

    private bool CheckZPositions()
    {
        // Filtrar objetos que necesitan chequeo de posición Z
        var zPositionObjects = objects.Where(obj => obj.needZPosition).ToList();

        // Si no hay objetos para chequear Z, considerar Z como completado
        if (zPositionObjects.Count == 0)
            return true;

        // Verificar si TODOS los objetos con posición Z cumplen
        return zPositionObjects.TrueForAll(obj =>
            Vector3.Distance(
                obj.prefab.transform.position, new Vector3(obj.prefab.transform.position.x, obj.prefab.transform.position.y, obj.goalPositionZ)
            )
            <= tolerance
        );
    }
}

[System.Serializable]
public class PositionCheck
{
    public GameObject prefab;
    public float goalPositionX;
    public float goalPositionY;
    public float goalPositionZ;
    public bool needXPosition = false;
    public bool needYPosition = false;
    public bool needZPosition = false;

    public PositionCheck(GameObject prefab, float goalX, float goalY, float goalZ)
    {
        this.prefab = prefab;
        this.goalPositionX = goalX;
        this.goalPositionY = goalY;
        this.goalPositionZ = goalZ;
    }
}
