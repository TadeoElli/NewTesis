using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerLocomotion playerLocomotion;
    private Vector3 spawnPosition;
    private Vector3 originalPositionZ;
    private Transform currentPlatform;
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    private void Start()
    {
        LoadPlayer();
    }
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
    }

    public IEnumerator LoadPlayerCoroutine()
    {
        PlayerData data = SaveSystem.LoadPlayer();
        if (data == null)
        {
            Debug.Log("No save data found, starting from default position");
            yield break;
        }

        Debug.Log("Loading saved position");
        Vector3 position;
        position.x = data.lastPosition[0];
        position.y = data.lastPosition[1];
        position.z = data.lastPosition[2];

        transform.position = position;
        SaveSystem.DeleteSaveData();
        SaveSystem.SavePlayer(this);
        yield return null; // Asegurarse de esperar un frame para confirmar que los datos se aplicaron correctamente
    }

    public void LoadPlayer()
    {
        StartCoroutine(LoadPlayerCoroutine());
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }
    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.FollowTarget();

    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(collision.transform);
            currentPlatform = collision.transform;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            transform.SetParent(null);
            currentPlatform = null;
        }
    }
}
