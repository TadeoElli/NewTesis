using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class Balance : MonoBehaviour
{
    [Header("No Modificar")]
    [SerializeField] private Transform leftPlate; // Transform del platillo izquierdo
    [SerializeField] private Transform rightPlate; // Transform del platillo derecho
    [SerializeField] private BalancePlate leftPlateTrigger; // Referencia al script del platillo izquierdo
    [SerializeField] private BalancePlate rightPlateTrigger; // Referencia al script del platillo derecho
    [SerializeField] float movementRange = 2f; // Rango máximo de movimiento vertical
    [SerializeField] float offsetY = 2f; // Rango máximo de movimiento vertical
    private float balanceSpeed = 2f; // Velocidad de balanceo
    private float leftWeight, rightWeight, weightDifference, leftPlateTargetY, rightPlateTargetY;
    private bool isBalanced = false;

    [Header("Modificable")]
    [SerializeField] private bool hasBalancedEvent = false;


    // UnityEvent que se dispara cuando la balanza se balancea por primera vez
    public UnityEvent OnFirstBalance;
    // Variable para asegurarse de que el evento solo se dispare una vez
    private bool hasBalancedOnce = false;
    private bool isBlocked = false;

    private void Update()
    {
        if (isBlocked)
            return;
        // Calcula las posiciones objetivo de los platillos
        leftPlateTargetY = Mathf.Clamp(weightDifference, -movementRange, movementRange);
        rightPlateTargetY = -leftPlateTargetY;

        // Interpola suavemente las posiciones de los platillos
        Vector3 leftPlatePosition = leftPlate.localPosition;
        leftPlatePosition.y = Mathf.Lerp(leftPlate.localPosition.y, leftPlateTargetY + offsetY, Time.deltaTime * balanceSpeed);
        leftPlate.localPosition = leftPlatePosition;

        Vector3 rightPlatePosition = rightPlate.localPosition;
        rightPlatePosition.y = Mathf.Lerp(rightPlate.localPosition.y, rightPlateTargetY + offsetY , Time.deltaTime * balanceSpeed);
        rightPlate.localPosition = rightPlatePosition;
    }


    public void UpdatePlateWeight(BalancePlate plate, float newWeight)
    {
        if(hasBalancedOnce) 
            return;
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
        if(!hasBalancedEvent)
            return;
        isBalanced = (weightDifference < 0.1f && weightDifference > -0.1f) ? true : false;
        // Comprobar si la balanza está balanceada y si el evento aún no se ha disparado
        if (isBalanced && !hasBalancedOnce)
        {
            StartCoroutine(TriggerEvent());
        }
    }

    private IEnumerator TriggerEvent()
    {
        yield return new WaitForSeconds(0.5f);
        if (Mathf.Abs(leftPlate.localPosition.y - rightPlate.localPosition.y) < 0.1f)
        {
            hasBalancedOnce = true; // Marcar que el evento ya se disparó
            OnFirstBalance?.Invoke(); // Invocar el evento
            isBlocked = true;
        }
        else
            StartCoroutine(TriggerEvent());
    }

    public void ResetBalance()
    {
        isBlocked = false;
        hasBalancedOnce = false;
        isBalanced = false;
    }
}
