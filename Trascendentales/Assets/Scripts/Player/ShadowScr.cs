using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class ShadowScr : MonoBehaviour
{
    public Transform player;  // Asigna el jugador desde el inspector
    public float rayDistance = 20f;


    // Update is called once per frame
    void Update()
    {

        int layerMask = 1 << LayerMask.NameToLayer("Player");

        RaycastHit hit;
        Vector3 rayOrigin = transform.position + Vector3.up * 10; 
        layerMask = ~layerMask; 

        if (Physics.Raycast(rayOrigin, Vector3.down, out hit, rayDistance, layerMask))
        {
            // Si el Raycast detecta el piso, ajusta la posición Y
            transform.position = new Vector3(player.position.x, hit.point.y + 0.01f, player.position.z);
        }

    }

}
