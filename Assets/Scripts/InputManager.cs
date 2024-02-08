using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControlls playerControls;
    AnimatorManager animatorManager;

    public Vector2 movementInput;
    private float moveAmount;
    
    public float verticalInput;
    public float horizontalInput;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControlls();

            //Cuando pulsamos ASDW o movemos el joystick, grabamos el movimiento en la variable movementInput.
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>(); 
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleInputs()
    {
        HandleMovementInput();
        //HandleJump();
    }
    /// <summary>
    /// Separamos movementInput en dos variables por separado para el movimiento horizontal y vertical 
    /// </summary>
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount);
    }
}
