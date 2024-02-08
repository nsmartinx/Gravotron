using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Transform mainCamera;
    public CharacterController controller;
    public Transform letter;

    public float speed = 20f; //controlls speed of the player
    public float wallrunSpeed = 50f;//controlles wallrunning speed
    public float gravity = -9.81f; //controlls gravity of player
    public float jumpHeight = 5f; //controlls jump height of player
    public float wallJumpHeight = 5f;//controlls jump height off wall
    public float wallVelocityOut = 5f;//controlls how far player flies out from wall
    public float wallVelocityForward = 5f;//controlls how far player flies forward when jumping off wall

    public Transform groundCheck;
    public Transform groundCheck1;
    public Transform rightWallCheck;
    public Transform leftWallCheck;
    public float groundDistance = 0.4f; //size of sphere that we are checking the ground for
    public float wallDistance = 0.4f; //size of sphere that we are checking wall for
    public LayerMask groundMask;
    public LayerMask wallMask; //gets the layer masks for the different types of objects
    public LayerMask deathMask;
    public LayerMask launchMask;
    public LayerMask letterMask;
    public LayerMask defaultMask;
    public bool isGrounded, isOnLeftWall, isOnRightWall;

    Vector3 velocity, wallrunMove;
    bool isWallrunning, wallrunAllowed, gravityAllowed, isLaunching;
    float cameraRotate = 0, gravityCameraRotate;
    int x, z;
    float wallrunTimer, gravityTimer;
    int wallSide; //set to -1 if on left wall, 1 if on right wall
    int directionOfGravity; //set to 1 if gravity is normal, -1 if gravity is inverse
    float launchSpeed;


    void Start()
    {
        wallrunAllowed = true;
        gravityAllowed = true;
        directionOfGravity = 1;//sets gravity to be normal
        gravityCameraRotate = 0;
    }

    // Update is called once per frame
    void Update()
    {


        //when player hits a death object
        if (Physics.CheckSphere(groundCheck.position, 1, deathMask) || Physics.CheckSphere(groundCheck1.position, 1, deathMask) || Physics.CheckSphere(rightWallCheck.position, 1, deathMask) || Physics.CheckSphere(leftWallCheck.position, 1, deathMask))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);//restarts level when you hit a death object
        }


        //detects if player is grounded. sets isGrounded to true or false
        if (directionOfGravity > 0)//if gravity is normal
        {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) || Physics.CheckSphere(groundCheck.position, groundDistance, wallMask);//will set isGrounded to true if player is on ground (invisible sphere around groundCheck collides with an object on layer ground or wall)
        }
        else //if gravity is flipped
        {
            isGrounded = Physics.CheckSphere(groundCheck1.position, groundDistance, groundMask) || Physics.CheckSphere(groundCheck1.position, groundDistance, wallMask);//will set isGrounded to true if player is on ground (invisible sphere around groundCheck collides with an object on layer ground or wall)
        }


        //if player is on the ground, it will reset their downward speed
        if (isGrounded && velocity.y < 0 || Physics.CheckSphere(groundCheck.position, .4f, defaultMask) || Physics.CheckSphere(groundCheck1.position, .4f, defaultMask))//checks if player is on ground and is moving downwards, or if player is standing on something labeled default mask (platforms that the player can't jump on, but will reset gravity)
        {
            velocity.y = -2f; //resets player downward speed to 2
        }


        //takes the input of the player on which way they want to move
        float x = Input.GetAxis("Horizontal"); //based on a/d keys
        float z = Input.GetAxis("Vertical"); //based on w/s keys

        if (directionOfGravity<0) //if direction of gravity is flipped, the left/right walking is switched
        {
            x = -x;
        }

        Vector3 move = transform.right * x + transform.forward * z; //assigns direction that we want to move
        if (move.magnitude > 1)
        {
            move /= move.magnitude; //ensures that moving in a diagnal (i.e. pushing two movement keys at once) is not faster
        }

        if(Input.GetButtonDown("Jump") && isGrounded)//when player jumps and is on the ground
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);//provides an upwards force on the player
        }

        velocity.y += gravity * Time.deltaTime; //sets downwards force based on the set gravity




        //wallrunning code starts

        isOnRightWall = Physics.CheckSphere(rightWallCheck.position, wallDistance, wallMask); //checks if player is next to a wall (and which side it is on)
        isOnLeftWall = Physics.CheckSphere(leftWallCheck.position, wallDistance, wallMask);


        //figures out which way the player is looking and sets x and z values to travel in when the player is wallrunning. This code will figure out which of the 4 cardinal directions the player is looking closest to, and set the direction of movement in that direction
        var v = transform.forward;
        v.y = 0;
        v.Normalize();

        if (Vector3.Angle(v, Vector3.forward) <= 45.0)
        {
            x = 0;
            z = 1;
        }
        else if (Vector3.Angle(v, Vector3.right) <= 45.0)
        {
            x = 1;
            z = 0;
        }
        else if (Vector3.Angle(v, Vector3.back) <= 45.0)
        {
            x = 0;
            z = -1;
        }
        else
        {
            x = -1;
            z = 0;
        }



        if (isOnRightWall && !isGrounded && Input.GetKey("w") && wallrunAllowed == true) //checks if player has wall on right, is not on wall, is allowed to wall run, and is attempting to move forward(criteria for starting a wall run)
        {
            wallSide = 1; //wall is on right side, used when jumping off wall
            velocity.y = 0; //turns off gravity
            isWallrunning = true;
            wallrunMove.x = x;//assigns moving direction to forward
            wallrunMove.z = z;//^^ based on which way the player is facing
            cameraRotate += (1000 * Time.deltaTime);//starts the rottioin of the camera
            if (cameraRotate>150)//checks when camera has fully rotated (30 degrees total)
            {
                cameraRotate = 150;//makes sure the camera does not over rotate
            }
            if (directionOfGravity > 0)//runs if gravity is normal
            {
                mainCamera.Rotate(Vector3.forward * cameraRotate / 5);//rotation of camera right
            }
            else//if gravity is flipped
            {
                mainCamera.Rotate(-Vector3.forward * cameraRotate / 5);//rotation of camera right relitive to the player (who is flipped)
            }
        }
        else
        {
            if (isOnLeftWall && !isGrounded && Input.GetKey("w") && wallrunAllowed == true)//checks if player has wall on left, is not on wall, is allowed to wall runm, and is attempting to move forward(criteria for starting a wall run)
            {
                wallSide = -1; //wall is on left side, used when jumping off wall
                velocity.y = 0; //turns off gravity
                isWallrunning = true;
                wallrunMove.x = x;//assigns moving direction to forward
                wallrunMove.z = z;
                cameraRotate -= (1000 * Time.deltaTime);//starts the rotation of the camera
                if (cameraRotate < -150)//checks when camera has fully roatated
                {
                    cameraRotate = -150;
                }
                if (directionOfGravity > 0)//if gravity is normal
                {
                    mainCamera.Rotate(Vector3.forward * cameraRotate / 5);//rotation of camera right
                }
                else//if gravity is flipped
                {
                    mainCamera.Rotate(-Vector3.forward * cameraRotate / 5);//rotation of camera right relitive to the player
                }
            }
            else
            {
                //rotation of camera back to center
                if (cameraRotate > 0)//if camera was rotated right
                {
                    cameraRotate -= (1000 * Time.deltaTime);
                    if (cameraRotate < 0)
                    {
                        cameraRotate = 0;
                    }
                }
                if (cameraRotate < 0)//if camera was roatated left
                {
                    cameraRotate += (1000 * Time.deltaTime);
                    if (cameraRotate > 0)
                    {
                        cameraRotate = 0;
                    }
                }
                if (directionOfGravity > 0)//if gravity is normal
                {
                    mainCamera.Rotate(Vector3.forward * cameraRotate / 5);//rotation of camera back to centre
                }
                else//if gravity is flipped
                {
                    mainCamera.Rotate(-Vector3.forward * cameraRotate / 5);//rotation of camera back to centre
                }
                isWallrunning = false;
            }
        }

        if (isWallrunning && Input.GetButtonDown("Jump"))//jump off of a wallrun
        {
            isWallrunning = false;
            wallrunTimer = .75f;//number of seconds of cooldown between wallruns
            velocity.y += Mathf.Sqrt(wallJumpHeight * -2f * gravity);//vertical jump off wall
        }

        
        if (wallrunTimer <= 0)//checks when the wallrun cooldown has finished
        {
            wallrunTimer = 0;
            wallrunAllowed = true;//enables wallrunning
        }
        else//if timer is still on
        {
            wallrunTimer -= Time.deltaTime;//countdown untill player can wallrun again, subtracts the frame time (in seconds) from the counter
            wallrunAllowed = false;
            controller.Move(((wallVelocityOut * -transform.right * wallSide) + (transform.forward * wallVelocityForward)) * Time.deltaTime);//moves player away from wall and forward
        }
       
        if (Input.GetKey("f") && gravityAllowed)//swapping gravity by pushing the f key
        {
            directionOfGravity = -directionOfGravity;//swaps direction of gravity (sets 1 to -1 or vice versa)
            gravityTimer = .5f;//number of seconds of cooldown between gravity swaps
        }

        if (gravityTimer <= 0)//once timer finishes
        {
            gravityTimer = 0;
            gravityAllowed = true;//enables gravity swapping
        }
        else//if timer is still counting down
        {
            gravityTimer -= Time.deltaTime;//countdown untill player can swap gravity again, subtracts the frame time (in seconds) from the counter
            gravityAllowed = false;//disables gravity swapping
        }

        if (directionOfGravity == 1 && gravityCameraRotate > 0)//checks if gravity is normal and camera is not in normal position
        {
            gravityCameraRotate -= Time.deltaTime * 500;//starts turning the camera back to normal
            if (gravityCameraRotate < 0)//if it overshoots
            {
                gravityCameraRotate = 0;
            }
        }
        if (directionOfGravity == -1 && gravityCameraRotate < 180)//if gravity is flipped, and camera is not flipped
        {
            gravityCameraRotate += Time.deltaTime * 500;//starts turning camera
            if (gravityCameraRotate > 180)//if it overshoots
            {
                gravityCameraRotate = 180;
            }
        }
        mainCamera.Rotate(Vector3.forward * gravityCameraRotate);//rotation of camera due to gravity swapping



        //code for the launchers
        if (Physics.CheckSphere(groundCheck.position, 1, launchMask) || Physics.CheckSphere(groundCheck1.position, 1, launchMask))//when player steps on a launcher
        {
            isLaunching = true;
            launchSpeed = 200;//sets initial launch velocity
        }
        if (isLaunching)
        {
            launchSpeed -= Time.deltaTime * 50;//decreases launch velocity with time
            controller.Move(transform.forward * launchSpeed * Time.deltaTime);//move player forwards the amoung of the launch velocity
        }
        if (launchSpeed < 0 || isGrounded)//stops you if you are on the gound, or the launch effect is done
        {
            isLaunching = false;
            launchSpeed = 0;
        }


        //code for revealing the secret letters when the player gets close
        if (Physics.CheckSphere(groundCheck.position, 20, letterMask) || Physics.CheckSphere(groundCheck1.position, 20, letterMask))//when player gets near secret letter
        {
            letter.gameObject.SetActive(true);//makes letter active (visible)
        }
        else
        {
            letter.gameObject.SetActive(false);//inactivates letterr (invisible)
        }



        controller.Move(velocity * Time.deltaTime * directionOfGravity); //applies downwards force (set earlier by things such as gravity and jumping off ground and walls)

        if (!isWallrunning)
        {
            controller.Move(move * speed * Time.deltaTime); //moves player in the direction of the Vector 3 move (usually based on whichever wasd keys are pressed)
            controller.Move(move * speed * Time.deltaTime); //moves player in thedirection of the Vector 3 move (usually based on whichever wasd keys are pressed)
        }
        if (isWallrunning)
        {
            controller.Move(wallrunMove * wallrunSpeed * Time.deltaTime);//uses wallrun speed instead of walking speed
        }

    }
}
