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
    private IEnumerator LoadPlayerPosition()
    {
        yield return new WaitForEndOfFrame();  // Esperar a que la escena esté completamente inicializada
        Vector3 savedPosition = SaveSystem.LoadPlayerData();

        if (savedPosition != Vector3.zero)
        {
            transform.position = savedPosition;
        }
    }

    private void Start()
    {
        StartCoroutine(LoadPlayerPosition());
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
