using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControlls playerControls;
    PlayerLocalMotion motion;
    AnimatorManager animatorManager;
    CameraManager cameraManager;
    

    public Vector2 movementInput;
    public Vector2 cameraInput;
    public float moveAmount;

    public float cameraInputX;
    public float cameraInputY;

    public float verticalInput;
    public float horizontalInput;

    public bool spaceInput;
    public bool aimInput;


    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        motion = GetComponent<PlayerLocalMotion>();
        cameraManager = GetComponent<CameraManager>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControlls();

            //Cuando pulsamos ASDW o movemos el joystick, grabamos el movimiento en la variable movementInput.
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
           
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.Sprint.performed += i => spaceInput = true;
            playerControls.PlayerActions.Sprint.canceled += i => spaceInput = false;

            playerControls.PlayerActions.Aim.performed += i => aimInput = true;
            playerControls.PlayerActions.Aim.canceled += i => aimInput = false;

            Debug.Log(playerControls.PlayerMovement.Camera);
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
        HandleSprinting();
        
        //HandleJump();
    }
    /// <summary>
    /// Separamos movementInput en dos variables por separado para el movimiento horizontal y vertical 
    /// </summary>
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        //Clamp var�a un valor entre 0 y 1. Para asegurar que sean positivos usamos MathAbs (esto se debe a que en el animator solo usamos valores positivos)
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount, motion.isSprinting);
    }

    private void HandleSprinting()
    {
        if(spaceInput && moveAmount > 0.5f)
        {
            motion.isSprinting = true;
        }

        else
        {
            motion.isSprinting= false;
        }
    }
}
