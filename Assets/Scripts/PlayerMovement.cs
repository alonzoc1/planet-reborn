using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller; // The motor that drives the player.
    public float turnSmoothTime = 0.1f;
    public float groundDistance = 0.4f;
    private PlayerStats playerStats;
    private float turnSmoothVelocity;
    public Transform cam;

    //public Transform groundCheck;
    //public LayerMask groundMask;
    //private bool isGrounded;
    //private float _directionY;

    Vector3 velocity;

    [SerializeField]
    //private float gravity = 9.81f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Lock the mouse cursor when playing!
        playerStats = gameObject.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        // Vector3 _direction = new Vector3(horizontal, 0, vertical); // Workaround for jump logic, do NOT normalize it.

        //isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // Create a tiny invisible sphere below. If they are off the grounded, it will trigger the Boolean.

        //if (isGrounded && velocity.y < 0)
        //{
        //    velocity.y = -2f;
        //}

        //if (Input.GetButtonDown("Jump") && isGrounded)
        //{
        //    _direction.y = playerStats.jumpPower; // Calls the jump power, but fails to move the object up on the Y-axis.
        //    Debug.Log("Jump key was pressed.");
        //    Debug.Log(playerStats.jumpPower); 
        //}

        // _direction.y = playerStats.gravity;


        if (direction.magnitude >= 0.1f)
        {
            //We can use the Atan 2 function to get the angle of the x-axis, but assumes that we start at 0 degrees.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; // Allows the player to rotate. Do not get rid of this variable, as variable angle is dependent on this.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // Helps smooth out player rotation. Also needed for player rotation.
            transform.rotation = Quaternion.Euler(0f, angle, 0f); // Have the player rotate, based on where the player/camera is moving.

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // Takes into account of the camera rotation.
            controller.Move(moveDir.normalized * (playerStats.movementSpeed * Time.deltaTime)); // Seems to conflict with the jump logic and gravity itself. Needs to be refactored.
        }

        //_directionY -= gravity * Time.deltaTime;
        //direction.y = _directionY;
    }
}
