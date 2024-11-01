using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayerCamera : MonoBehaviour
{
    InputManager inputManager;
    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position =  new Vector3(inputManager.transform.position.x,inputManager.transform.position.y, transform.position.z);
    }
}
