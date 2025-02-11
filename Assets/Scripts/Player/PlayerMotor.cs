using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] private float crouchSmoothTime = 0.1f;
    private float crouchSmoothVelocity;
    [SerializeField] private LayerMask collisionLayer;
    private InputManager inputManager;
    private CharacterController controller;
    private RaycastHit hit;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    public float rayLength = 1.1f;
    private bool lerpCrouch;
    private bool crouching;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        inputManager = GetComponent<InputManager>();
        crouching = false;
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            if (inputManager.IsSprintPressed)
            {
                Sprint();
            }
            else
            {
                Walk();
            }
        }
        else
        {
            Walk();
        }

        if (lerpCrouch)
        {
            float targetHeight = crouching ? 1f : 2f;

            controller.height = Mathf.SmoothDamp(controller.height, targetHeight, ref crouchSmoothVelocity, crouchSmoothTime);

            if (Mathf.Abs(controller.height - targetHeight) < 0.01f)
            {
                controller.height = targetHeight;
                lerpCrouch = false;
                crouchSmoothVelocity = 0;
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

    public void HandleCrouchPerformed()
    {
        if (!crouching)
        {
            Crouch();
        }
        else
        {
            UnCrouch();
        }
    }

    private void Sprint()
    {
        speed = crouching ? 3.5f : 8f;
    }

    private void Walk()
    {
        speed = crouching ? 2.5f : 5;
    }

    public void HandleJumpStarted()
    {
        Walk();
    }

    private void Crouch()
    {
        crouching = true;
        lerpCrouch = true;
    }

    private void UnCrouch()
    {
        if (!CanUnCrouch()) return;

        crouching = false;
        lerpCrouch = true;
    }

    private bool CanUnCrouch()
    {
        if (Physics.Raycast(transform.position, Vector3.up, out hit, rayLength, collisionLayer))
        {
            return false;
        }

        return true;
    }
}
