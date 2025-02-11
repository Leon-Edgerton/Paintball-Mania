using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;

    private PlayerMotor motor;
    private PlayerLook look;
    private bool isSprintPressed;
    public bool IsSprintPressed => isSprintPressed;

    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;

        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();

        onFoot.Jump.started += OnJumpStarted;
        onFoot.Jump.performed += OnJumpPerformed;

        onFoot.Crouch.performed += OnCrouchPerformed;
        // onFoot.Sprint.performed += ctx => motor.Sprint();

    }

    private void OnJumpStarted(InputAction.CallbackContext obj)
    {
        motor.HandleJumpStarted();
    }

    private void OnCrouchPerformed(InputAction.CallbackContext obj)
    {
        motor.HandleCrouchPerformed();
    }

    private void OnJumpPerformed(InputAction.CallbackContext obj)
    {
        motor.Jump();
    }

    private void Update()
    {
        isSprintPressed = onFoot.Sprint.IsPressed();
    }

    void FixedUpdate()
    {
        //tell the playermotor to move using the value from our movement action.
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessingLook(onFoot.Look.ReadValue<Vector2>());
    }


    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        // Unsubscribe from all events
        onFoot.Jump.started -= OnJumpStarted;
        onFoot.Jump.performed -= OnJumpPerformed;
        onFoot.Crouch.performed -= OnCrouchPerformed;
        onFoot.Disable();
    }
}



