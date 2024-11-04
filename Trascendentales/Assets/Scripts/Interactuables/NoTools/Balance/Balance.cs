using UnityEngine;

public class Balance : MonoBehaviour
{
    [SerializeField] private Transform leftPlate; // Transform del platillo izquierdo
    [SerializeField] private Transform rightPlate; // Transform del platillo derecho
    [SerializeField] private float movementRange = 2f; // Rango máximo de movimiento vertical
    [SerializeField] private float balanceSpeed = 2f; // Velocidad de balanceo
    [SerializeField] private float leftWeight, rightWeight, weightDifference, leftPlateTargetY, rightPlateTargetY;
    [SerializeField] private bool isBalanced = false;

    public BalancePlate leftPlateTrigger; // Referencia al script del platillo izquierdo
    public BalancePlate rightPlateTrigger; // Referencia al script del platillo derecho

    private void Update()
    {

        // Calcula las posiciones objetivo de los platillos
        leftPlateTargetY = Mathf.Clamp(weightDifference, -movementRange, movementRange);
        rightPlateTargetY = -leftPlateTargetY;

        // Interpola suavemente las posiciones de los platillos
        Vector3 leftPlatePosition = leftPlate.localPosition;
        leftPlatePosition.y = Mathf.Lerp(leftPlate.localPosition.y, leftPlateTargetY, Time.deltaTime * balanceSpeed);
        leftPlate.localPosition = leftPlatePosition;

        Vector3 rightPlatePosition = rightPlate.localPosition;
        rightPlatePosition.y = Mathf.Lerp(rightPlate.localPosition.y, rightPlateTargetY, Time.deltaTime * balanceSpeed);
        rightPlate.localPosition = rightPlatePosition;
    }


    public void UpdatePlateWeight(BalancePlate plate, float newWeight)
    {
        // Esta función se llama cuando el peso de uno de los platillos cambia
        BalancePlate currentPlate;
        if (plate == leftPlateTrigger)
        {
            currentPlate = leftPlateTrigger;
            leftWeight = newWeight;
        }
        else
        {
            currentPlate = rightPlateTrigger;
            rightWeight = newWeight;
        }
        weightDifference = rightWeight - leftWeight;
        isBalanced = (weightDifference < 0.1f && weightDifference > -0.1f) ? true : false;
    }
}
