using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float turnSmoothVelocity;
    public CharacterController controller; // The motor that drives the player.
    public float speed = 6.0f;
    public float turnSmoothTime = 0.1f;
    public Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
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
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime); // Helps smooth out player rotation.
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward; // Takes into account of the camera rotation.
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }
}
