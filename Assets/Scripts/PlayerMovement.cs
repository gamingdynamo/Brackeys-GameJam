using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 10.0f;
    public float gravity = 20.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;

    private Animator animator;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator= GetComponent<Animator>();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    void Update()
    {
        // Direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Is running (left shift)
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical");
        float curSpeedY = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal");

        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        moveDirection.y -= gravity ;

        if (moveDirection.x != 0 || moveDirection.z != 0) { animator.SetBool("IsWalking", true);  } else { animator.SetBool("IsWalking", false); }

        // Gravity
        if (!characterController.isGrounded)
        {
            
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
