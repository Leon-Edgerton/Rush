using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private InputManager inputManager;

    private Rigidbody rb;

    private Vector3 playerVelocity;
    public float speed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ProcessMove(Vector2 Input)
    {
        Vector3 moveDirection = new Vector3(Input.x, 0f, Input.y);

        //Apply movement
        rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);

        // Rotate player to face movement direction
        if (moveDirection.sqrMagnitude > 0.01f) // Prevents snapping to zero direction
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);// Tells player to look in the direction its moving
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, Time.fixedDeltaTime * 10f); // Smooth rotation and rotation speed
        }
    }
}
