using Unity.VisualScripting;
using UnityEngine;

public class ValveController : MonoBehaviour,IRotableClamp
{
    public Transform platform; // Referencia a la plataforma que quieres escalar
    public float maxRotation = 180f; // El ángulo máximo que puedes rotar la válvula (por ejemplo, 180 grados)
    public float minRot = 0;
    public Vector3 closedScale = new Vector3(1, 1, 1); // Escala de la plataforma cuando la válvula está cerrada
    public Vector3 openScale = new Vector3(3, 1, 1); // Escala de la plataforma cuando la válvula está completamente abierta

    private float currentRotation = 0f; // El ángulo de rotación actual de la válvula
    private float rotationDifference;
    [SerializeField] private bool isClamped;
    [SerializeField] private RotableObject rotable;

    Rigidbody rb;

    [SerializeField] private float minRotationY = -45f; // Límite mínimo en Y
    [SerializeField] private float maxRotationY = 45f;  // Límite máximo en Y
    [SerializeField] private float minRotationZ = -45f; // Límite mínimo en Z
    [SerializeField] private float maxRotationZ = 45f;  // Límite máximo en Z


    public bool IsClamped() => isClamped;
    public float GetMinRotationY() => minRotationY;
    public float GetMaxRotationY() => maxRotationY;
    public float GetMinRotationZ() => minRotationZ;
    public float GetMaxRotationZ() => maxRotationZ;

    private void Start()
    {
        currentRotation = transform.localEulerAngles.y;
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        RotateValve();
    }

    void RotateValve()
    {
        // Obtenemos la rotación actual del objeto válvula en el eje Y (o el eje que corresponda)
        float previousRotation = 0;
        if(MouseState.Instance.IsLeftClickPress() && rotable.gimballRef != null)
            previousRotation = rotable.gimballRef.localEulerAngles.y;
        else
            previousRotation = transform.localEulerAngles.y;



        //Calculamos la diferencia entre la rotacion actual y la anterior
        float deltaRot = Mathf.DeltaAngle(previousRotation, currentRotation);

        //Si la diferencia es lo suficientemente significante sumamos (si es numero negativo se resta 2+2 4 [-2 + 2 0])
        if (Mathf.Abs(deltaRot) > Mathf.Epsilon)
        {
            rotationDifference += deltaRot;
        }

        //Reseteamos el valor previo (si, los nombres estan al reves)
        currentRotation = previousRotation;

        float clampedRot = Mathf.Clamp(previousRotation, minRot, maxRotation);
        //print(clampedRot);

  

        //mueve los valores entre 0 y 1
        float progress = Mathf.InverseLerp(minRot, maxRotation, rotationDifference);

      

        // Modificamos la escala de la plataforma basándonos en el progreso
        platform.localScale = Vector3.Lerp(closedScale, openScale, progress);
    }

}

