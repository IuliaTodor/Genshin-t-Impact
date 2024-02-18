using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerLocalMotion playerLocalMotion;
    Animator animator;
    AnimatorManager animManager;

    public bool isInteracting;
    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>() ;
        inputManager = GetComponent<InputManager>();
        playerLocalMotion = GetComponent<PlayerLocalMotion>();
        animManager = GetComponent<AnimatorManager>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        inputManager.HandleInputs();
        animManager.HandleDeathAnimation();
        animManager.HandleAimAnimation();
    }

    //FixedUpdate funciona mejor con Rigidbody
    private void FixedUpdate()
    {
        playerLocalMotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleCameraMovement();

        isInteracting = animator.GetBool("isInteracting");
    }
}
