using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerLocalMotion : MonoBehaviour
{
    PlayerManager playerManager;
    InputManager inputManager;
    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody rigidBody;
    AnimatorManager animatorManager;

    public float inAirTimer;
    public float leapingVelocity;
    public float fallingVelocity;
    public float rayCastHeightOffset = 0.5f;
    public LayerMask groundLayer;


    public float walkingSpeed = 1.5f;
    public float runningSpeed = 3f;
    public float sprintingSpeed = 7f;
    public float rotationSpeed = 15;

    public bool isGrounded;
    public bool isSprinting;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rigidBody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        playerManager = GetComponent<PlayerManager>();
        animatorManager = GetComponent<AnimatorManager>();
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();

        if(playerManager.isInteracting)
        {
            return;
        }
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {

        moveDirection = DirectionVector(moveDirection);

        if (isSprinting)
        {
            moveDirection = moveDirection.normalized * sprintingSpeed;
        }

        else
        {
            if (inputManager.moveAmount >= 0.5f)
            {
                moveDirection = moveDirection.normalized * runningSpeed;
            }

            else
            {
                moveDirection = moveDirection.normalized * walkingSpeed;
            }
        }


        //Si corremos, usamos sprintingSpeed. Si corremos, usamos runningSpeed. Si caminamos, usamos WalkingSpeed.
        moveDirection *= runningSpeed;
        rigidBody.velocity = moveDirection;
    }

    private void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = DirectionVector(targetDirection);

        //Si la dirección de rotación que queremos es 0, la dirección se convierte en la del eje Z a la que mira el personaje
        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        //Los Quaternion se usan para calcular rotaciones
        //La rotación a la que queremos que mire el personaje es la del vector de dirección
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        //Slerp es una rotación entre punto A y B. El jugador rota hacia targetRotation a una velocidad
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    /// <summary>
    /// Toma la dirección de movimiento que usaremos para movernos y rotar al personaje
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private Vector3 DirectionVector(Vector3 target)
    {
        //La dirección de la cámera en el eje z (hacia la que está mirando) multiplicado por cuanto nos movemos en y
        target = cameraObject.forward * inputManager.verticalInput;
        //La dirección de la cámera en el eje x (90º grados a camera.forward) multiplicado por cuanto nos movemos en x
        target = target + cameraObject.right * inputManager.horizontalInput;
        target.Normalize();
        //Evitamos movernos hacia arriba al caminar
        target.y = 0;

        return target;
    }
    
    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y = raycastOrigin.y + rayCastHeightOffset;

        if(!isGrounded)
        {
            if(playerManager.isInteracting)
            {
                Debug.Log("lol");
                animatorManager.PlayTargetAnimation("Falling", true);
            }

            inAirTimer = inAirTimer + Time.deltaTime;
            rigidBody.AddForce(transform.forward * leapingVelocity);
            rigidBody.AddForce(-Vector3.up * fallingVelocity * inAirTimer);
        }

        if (Physics.SphereCast(raycastOrigin, 0.2f, -Vector3.up, out hit, groundLayer))
        {
            if(!isGrounded && playerManager.isInteracting)
            {
                Debug.Log("XD");
                animatorManager.PlayTargetAnimation("Idle", true);
            }

            inAirTimer = 0;
            isGrounded = true;
        }

        else
        {
            isGrounded = false;
        }

    }

}
