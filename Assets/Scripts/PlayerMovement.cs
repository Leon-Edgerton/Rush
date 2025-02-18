using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private InputManager inputManager;

    public float speed = 5f;
    public float jumpForce = 7f;
    public LayerMask groundLayer;


    private bool isGrounded;
    [SerializeField] private float rotationScalar;
    private Transform _camTransform;
    private Quaternion _targetRotation;
    private Vector3 _moveDirection;
    private const float SmallFloat = 0.001f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        _camTransform = Camera.main.transform;
    }

    private void OnCollisionStay(Collision collision)
    {
        // Check if the collided object is in the groundLayer
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // When exiting the collision with ground, set isGrounded to false
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = false;
        }
    }

    private void Update()
    {
        // Prevent Look rotation viewing vector is zero
        if (_moveDirection == Vector3.zero)
            return;
        
        var desiredRotation = Quaternion.LookRotation(_moveDirection);
        _targetRotation = Quaternion.Slerp(rb.rotation, desiredRotation, Time.deltaTime * rotationScalar);
    }

    public void ProcessMove(Vector2 input)
    {
        Transform camTransform = Camera.main.transform;
        Vector3 forward = camTransform.forward;
        Vector3 right = camTransform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        _moveDirection = forward * input.y + right * input.x;

        rb.linearVelocity = new Vector3(_moveDirection.x * speed, rb.linearVelocity.y, _moveDirection.z * speed);
        rb.MoveRotation(_targetRotation.normalized);
    }

    public void Jump()
    {
        if (isGrounded)
        {
            var linearVelocity = rb.linearVelocity;
            linearVelocity = new Vector3(linearVelocity.x, jumpForce, linearVelocity.z);
            rb.linearVelocity = linearVelocity;
            isGrounded = false; // Prevents double jumping until OnCollisionStay is called again
        }
    }
}


