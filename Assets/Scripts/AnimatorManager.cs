using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;
    InputManager inputManager;
    CameraManager cameraManager;
    PlayerManager playerManager;
    PlayerLocalMotion localMotion;
    int x;
    int y;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerManager = FindObjectOfType<PlayerManager>();
        localMotion = FindObjectOfType<PlayerLocalMotion>();
        x = Animator.StringToHash("X");
        y = Animator.StringToHash("Y");
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement, bool isSprinting)
    {
        float snappedHorizontal = 0;
        float snappedVertical = 0;

        snappedHorizontal = SetSnap(snappedHorizontal, horizontalMovement);
        snappedVertical = SetSnap(snappedVertical, verticalMovement);

        if(isSprinting)
        {
            snappedHorizontal = horizontalMovement;
            snappedVertical = 2;
        }
       
        animator.SetFloat(x, snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(y, snappedVertical, 0.1f, Time.deltaTime);
    }

    public void HandleDeathAnimation()
    {
        if(Input.GetKey(KeyCode.F))
        {
            animator.SetBool("IsDead", true);
        }

        if(Input.GetKey(KeyCode.X))
        {
            animator.SetBool("IsDancing", true);
            Debug.Log("hola");
        }
     
    }

    public void HandleFallingAnimation()
    {
        if(!localMotion.isGrounded)
        {
            animator.SetBool("IsFalling", true);
        }

        else if(localMotion.isGrounded) 
        {
            animator.SetBool("IsFalling", false);
        }
    }

    public IEnumerator HandleAimAnimation()
    {
            if (CameraSwitch.IsActiveCamera(playerManager.firstPersonCam))
            {
                animator.SetBool("isAiming", true);
            }

            else if (CameraSwitch.IsActiveCamera(playerManager.thirdPersonCam))
            {
                animator.SetBool("isAiming", false);
            }

        yield return null;
    }

    public void PlayTargetAnimation(string targetAnimation, bool isInteracting)
    {
        animator.SetBool("isInteracting", isInteracting);
    }

    /// <summary>
    /// Mejora la transici�n entre animaciones para que sean m�s fluidas
    /// </summary>
    /// <param name="snap"></param>
    /// <param name="movement"></param>
    /// <returns></returns>
    private float SetSnap(float snap, float movement)
    {
        if (movement > 0 && movement < 0.55f)
        {
            return snap = 0.5f;
        }

        else if (movement > 0.55f)
        {
            return snap = 1;
        }

        else if (movement < 0 && movement > -0.55f)
        {
            return snap = -0.5f;
        }


        else if (movement < -0.55f)
        {
            return snap = -1;
        }

        else
        {
            return snap = 0;
        }

    }
}
