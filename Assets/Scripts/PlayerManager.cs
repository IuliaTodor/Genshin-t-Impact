using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerLocalMotion playerLocalMotion;
    Animator animator;
    AnimatorManager animManager;

    [SerializeField] public CinemachineVirtualCamera thirdPersonCam;
    [SerializeField] public CinemachineVirtualCamera firstPersonCam;

    public bool isInteracting;
    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>() ;
        inputManager = GetComponent<InputManager>();
        playerLocalMotion = GetComponent<PlayerLocalMotion>();
        animManager = GetComponent<AnimatorManager>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        CameraSwitch.Register(thirdPersonCam);
        CameraSwitch.Register(firstPersonCam);   
        CameraSwitch.SwitchCamera(thirdPersonCam);
        Debug.Log("ActiveCamera" + CameraSwitch.activeCamera);
    }

    private void OnDisable()
    {
        CameraSwitch.Unregister(thirdPersonCam);
        CameraSwitch.Unregister(firstPersonCam);
     
  
    }

    private void Update()
    {
        inputManager.HandleInputs();
        animManager.HandleDeathAnimation();
        animManager.HandleAimAnimation();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(CameraSwitch.IsActiveCamera(thirdPersonCam)) 
            {
                CameraSwitch.SwitchCamera(firstPersonCam);
            }

            else if (CameraSwitch.IsActiveCamera(firstPersonCam))
            {
                CameraSwitch.SwitchCamera(thirdPersonCam);
            }
        }
    }

    //FixedUpdate funciona mejor con Rigidbody
    private void FixedUpdate()
    {
        playerLocalMotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        if(cameraManager != null)
        {

            cameraManager.HandleCameraMovement();
        }

        isInteracting = animator.GetBool("isInteracting");
    }
}
