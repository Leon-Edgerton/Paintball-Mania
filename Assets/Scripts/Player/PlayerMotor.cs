using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using JetBrains.Annotations;
using UnityEditor.Experimental.GraphView;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private RaycastHit hit;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    public float rayLength = 1.1f;
    public bool lerpCrouch;
    public bool crouching;
    public bool sprinting;
    public float crouchTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
            
                controller.height = Mathf.Lerp(controller.height, 1, p);
            
            else
                controller.height = Mathf.Lerp(controller.height, 2, p);
            
            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }

    }

    //recieve the inputs for InputManager.cs and apply them to Character Controller.
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2;
        }
        controller.Move(playerVelocity * Time.deltaTime);

    }

    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

    }

    public void Crouch()
    {
        if(crouching)
        {
            if(Physics.Raycast(transform.position, Vector3.up, out hit, rayLength)) // sets start of raycast on players position, makes it shoot up, makes it store info when it collides with something, gives it a length.
            {
                return;
            }

        }
        
        crouching = !crouching;
        if (crouching)
            speed = 2.5f;
        else
            speed = 5f;
        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        if (isGrounded && sprinting)
            speed = 8;
        else
            speed = 5;
    }
}
