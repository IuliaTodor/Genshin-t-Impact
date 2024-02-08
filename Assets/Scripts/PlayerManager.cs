using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    InputManager inputManager;
    PlayerLocalMotion playerLocalMotion;
    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocalMotion = GetComponent<PlayerLocalMotion>();
    }

    private void Update()
    {
        inputManager.HandleInputs();
    }

    //FixedUpdate funciona mejor con Rigidbody
    private void FixedUpdate()
    {
        playerLocalMotion.HandleAllMovement();
    }
}
