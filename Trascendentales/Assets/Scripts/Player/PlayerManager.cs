using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    [HideInInspector] public CameraManager cameraManager;
    AnimatorManager animatorManager;
    Animator animator;
    PlayerLocomotion playerLocomotion;
    private Vector3 spawnPosition;
    private Vector3 originalPositionZ;
    private Transform currentPlatform;
    public bool isInteracting;
    private bool isAlive = true;
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        animatorManager = GetComponent<AnimatorManager>();
        animator = animatorManager.animator;
    }
    #region SavePosition
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
    #endregion
    private void Update()
    {
        if (!isAlive)
            return;
        inputManager.HandleAllInputs();
    }
    private void FixedUpdate()
    {
        if (!isAlive)
            return;
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        if (!isAlive)
            return;
        //cameraManager.FollowTarget();
        isInteracting = animator.GetBool("isInteracting");
    }
    public void Death()
    {
        isAlive = false;
        animator.SetTrigger("isDeath");
        animatorManager.PlayTargetAnimation("Death", true);
        inputManager.Death();
    }
    

    
}
