using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    private float turnSmoothVelocity;
    private PlayerStats playerStats; // Declare the gameObject playerStats, but reference our C# script.
    public CharacterController controller; // The motor that drives the player.
    public float turnSmoothTime = 0.1f;
    public Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = player.GetComponent<PlayerStats>(); // Allow us to connect the PlayerStats.cs script, rather than shoving it in here. We're going to need this, in the event we link stuff here.
        Cursor.lockState = CursorLockMode.Locked; // Lock the mouse cursor when playing!
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            //We can use the Atan 2 function to get the angle of the x-axis, but assumes that we start at 0 degrees.
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y; // Allows the player to rotate. Do not get rid of this variable, as variable angle is dependent on this.
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // Helps smooth out player rotation. Also needed for player rotation.
            transform.rotation = Quaternion.Euler(0f, angle, 0f); // Have the player rotate, based on where the player/camera is moving.

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // Takes into account of the camera rotation.
            controller.Move(moveDir.normalized * playerStats.movementSpeed * Time.deltaTime); // Takes into account the Vector3 variable for where the player should move. Defaulting it to just 'direction' will make the controls very janky.
        }
    }
}
