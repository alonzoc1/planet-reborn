using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTester : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;
    [SerializeField]
    private float _gravity = 1;
    [SerializeField]
    private float _jumpSpeed = 20;

    private CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (Input.GetButtonDown("Jump"))
        {
            direction.y = _jumpSpeed; // Calls the jump power, but fails to move the object up on the Y-axis.
            Debug.Log("Jump key was pressed.");
        }

        direction.y -= _gravity;

        controller.Move(direction * _moveSpeed * Time.deltaTime);
    }
}
