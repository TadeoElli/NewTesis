using UnityEngine;

public class TelescopeBase : MonoBehaviour
{
    private const string rechargableLayerName = "Rechargable"; // Nombre de la capa Rechargable
    [SerializeField] Telescope telescope;
    private void OnCollisionEnter(Collision other)
    {
        // Activa el telescopio si colisiona con un objeto de la capa Rechargable
        if (other.gameObject.layer == 9)
        {
            telescope.SetCharged(true);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        // Desactiva el telescopio al salir de la colisión con un objeto Rechargable
        if (other.gameObject.layer == 9)
        {
            telescope.SetCharged(false);
        }
    }

}

