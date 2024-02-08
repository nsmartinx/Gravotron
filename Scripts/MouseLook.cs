using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 1500f; //controlls sensitivity

    public Transform playerBody;

    float xRotation = 0f;
    int directionOfGravity = 1;
    bool gravityAllowed = true;
    float gravityTimer = 0;
    float mouseX, mouseY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //holds cursor in center of screen so that it can not leave the game window
    }

    // Update is called once per frame
    void Update()
    {
        //code to unlock and relock the mouse (so the user can press on screen buttons)
        if (Input.GetKey("q"))
        {
            Cursor.lockState = CursorLockMode.None;//unlocks mouse
        }
        if (Input.GetKey("e"))
        {
            Cursor.lockState = CursorLockMode.Locked;//locks mouse
        }


        //code to figure out the direction of gravity. Same as code in player movement script.
        if (Input.GetKey("f") && gravityAllowed)
        {
            directionOfGravity = -directionOfGravity;//swaps direction of gravity
            gravityTimer = .5f;//number of seconds of cooldown between gravity swaps
        }

        if (gravityTimer <= 0)//once timer finishes
        {
            gravityTimer = 0;
            gravityAllowed = true;
        }
        else//if timer is still counting down
        {
            gravityTimer -= Time.deltaTime;//countdown untill player can swap gravity again
            gravityAllowed = false;
        }




        if (directionOfGravity > 0)//if gravity is normal
        {
            mouseX = Input.GetAxis("Mouse X") * mouseSensitivity; //multiplying by sensitivity and accounting for framrate
            mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity; //gets position of mouse on each axis
        }
        else//if gravity is swapped (flips the x and y values so left is right, and up is down)
        {
            mouseX = -Input.GetAxis("Mouse X") * mouseSensitivity; //multiplying by sensitivity and accounting for framrate
            mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity; //gets position of mouse on each axis
        }

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //ensures that player can not look past straight up or straight down

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f); //rotation of camera up/down
        playerBody.Rotate(Vector3.up * mouseX);//rotation of player left/right
    }
}
