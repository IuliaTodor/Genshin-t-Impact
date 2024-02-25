using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerLocalMotion : MonoBehaviour
{
    Vector3 moveDirection;

    InputManager inputManager;
    AnimatorManager animManager;
    Transform cameraObject;
    Rigidbody rigidBody;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] public bool isGrounded;

    [Header("Movement")]
    public float walkingSpeed = 1.5f;
    public float runningSpeed = 3f;
    public float sprintingSpeed = 7f;
    public bool isSprinting;

    [Header("Rotation")]
    public float rotationSpeed = 2;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rigidBody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        animManager = GetComponent<AnimatorManager>();
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundRadius, (int)whatIsGround);

        Debug.Log(isGrounded);
    }


    public void HandleAllMovement()
    {
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

        //Si la direcci�n de rotaci�n que queremos es 0, la direcci�n se convierte en la del eje Z a la que mira el personaje
        if (targetDirection == Vector3.zero)
        {
            targetDirection = transform.forward;
        }

        //Los Quaternion se usan para calcular rotaciones
        //La rotaci�n a la que queremos que mire el personaje es la del vector de direcci�n
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        //Slerp es una rotaci�n entre punto A y B. El jugador rota hacia targetRotation a una velocidad
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.rotation = playerRotation;
    }

    /// <summary>
    /// Toma la direcci�n de movimiento que usaremos para movernos y rotar al personaje
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    /// 

    private Vector3 DirectionVector(Vector3 target)
    {
        //La direcci�n de la c�mera en el eje z (hacia la que est� mirando) multiplicado por cuanto nos movemos en y
        target = cameraObject.forward * inputManager.verticalInput;
        //La direcci�n de la c�mera en el eje x (90� grados a camera.forward) multiplicado por cuanto nos movemos en x
        target = target + cameraObject.right * inputManager.horizontalInput;
        target.Normalize();
        //Evitamos movernos hacia arriba al caminar
        target.y = 0;

        return target;
    }
}
