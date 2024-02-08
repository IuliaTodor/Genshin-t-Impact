using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocalMotion : MonoBehaviour
{
    InputManager inputManager;
    Vector3 moveDirection;
    Transform cameraObject;
    Rigidbody rigidBody;

    public float movementSpeed = 6;
    public float rotationSpeed = 15;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        rigidBody = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
    }

    public void HandleAllMovement()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {

        moveDirection = DirectionVector(moveDirection);

        moveDirection *= movementSpeed;
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
