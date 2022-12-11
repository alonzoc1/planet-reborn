using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller; // The motor that drives the player.
    public float turnSmoothTime = 0.1f;
    public Transform cam;
    public float forceGravity = -18f; // Should be negative

    private float turnSmoothVelocity;
    private float jumpVelocity;
    private PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the mouse cursor when playing!
        playerStats = gameObject.GetComponent<PlayerStats>();
        jumpVelocity = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        // If user is moving
        if (direction.magnitude >= 0.1f)
        {
            //We can use the Atan 2 function to get the angle of the x-axis, but assumes that we start at 0 degrees.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; // Allows the player to rotate. Do not get rid of this variable, as variable angle is dependent on this.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // Helps smooth out player rotation. Also needed for player rotation.
            transform.rotation = Quaternion.Euler(0f, angle, 0f); // Have the player rotate, based on where the player/camera is moving.

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // Takes into account of the camera rotation.
            moveDir = HandleJumpMovement(moveDir.normalized * playerStats.GetMovementSpeed());
            controller.Move(moveDir * Time.deltaTime); // Takes into account the Vector3 variable for where the player should move. Defaulting it to just 'direction' will make the controls very janky.
        } else if (jumpVelocity != 0f) // Finish vertical movement even if user is not moving
        {
            controller.Move(HandleJumpMovement(Vector3.zero) * Time.deltaTime);
        }
        

    }
    
    private Vector3 HandleJumpMovement(Vector3 moveDir) {
        // moveDir should already be normalized and player speed should be factored in already
        // Reset jump velocity when we hit ground
        if (controller.isGrounded && jumpVelocity < 0)
            jumpVelocity = 0f;

        // Jump (change this logic if we ever add something like double jump)
        if (controller.isGrounded && Input.GetButtonDown("Jump"))
            jumpVelocity = Mathf.Sqrt(playerStats.jumpPower);

        jumpVelocity += forceGravity * Time.deltaTime;
        moveDir.y = jumpVelocity;
        return moveDir;
    }
}
