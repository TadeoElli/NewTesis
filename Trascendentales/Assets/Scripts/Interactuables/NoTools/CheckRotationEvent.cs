using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class CheckRotationEvent : MonoBehaviour
{
    [SerializeField] private List<RotationCheck> objects;
    public UnityEvent OnCompleted;
    public UnityEvent OnBreak;
    private bool isCompleted = false;
    [SerializeField] private bool blockAfterComplete = false;
    public float angleOfObject;

    void Update()
    {
        if (isCompleted && blockAfterComplete)
            return;
        angleOfObject = objects[0].prefab.transform.rotation.eulerAngles.y;

        CheckRotations();
    }
    private void Start()
    {
    }

    private void CheckRotations()
    {
        // Verificar rotaciones Y
        bool allYRotationsCorrect = CheckYRotations();

        // Verificar rotaciones Z
        bool allZRotationsCorrect = CheckZRotations();

        // Si ambas rotaciones se cumplen
        if (allYRotationsCorrect && allZRotationsCorrect)
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

    private bool CheckYRotations()
    {
        // Filtrar objetos que necesitan chequeo de rotación Y
        var yRotationObjects = objects.Where(obj => obj.needYRotation).ToList();

        // Si no hay objetos para chequear Y, considerar Y como completado
        if (yRotationObjects.Count == 0)
            return true;

        // Verificar si TODOS los objetos con rotación Y cumplen
        return yRotationObjects.TrueForAll(obj =>(
            (obj.prefab.transform.rotation.eulerAngles.y >= obj.goalRotationY - 0.01f) &&
            (obj.prefab.transform.rotation.eulerAngles.y <= obj.goalRotationY + 0.01f)
            )
        );
    }

    private bool CheckZRotations()
    {
        // Filtrar objetos que necesitan chequeo de rotación Z
        var zRotationObjects = objects.Where(obj => obj.needZRotation).ToList();

        // Si no hay objetos para chequear Z, considerar Z como completado
        if (zRotationObjects.Count == 0)
            return true;

        // Verificar si TODOS los objetos con rotación Z cumplen
        return zRotationObjects.TrueForAll(obj =>
            Mathf.Approximately(
                NormalizeAngle(obj.prefab.transform.rotation.eulerAngles.z),
                NormalizeAngle(obj.goalRotationZ)
            )
        );
    }

    // Método para normalizar ángulos entre 0 y 360
    private float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle < 0) angle += 360;
        return angle;
    }
}
[System.Serializable]
public class RotationCheck
{
    public GameObject prefab;
    public float goalRotationY;
    public float goalRotationZ;
    public bool needYRotation = false;
    public bool needZRotation = false;

    public RotationCheck(GameObject prefab, float goalRotationY, float goalRotationZ)
    {
        this.prefab = prefab;
        this.goalRotationY = goalRotationY;
        this.goalRotationZ = goalRotationZ;
    }
}
