//This script is a combination of the movement script and the camera movement script from Game Engine Design ILE 1
//https://www.youtube.com/watch?v=iFrO4bqmOPU
//https://www.youtube.com/watch?v=HcP8_dR-kqU&ab_channel=BrokenKnightsGames
//Above are the links we used as a reference for the new unity input system movement. 
//We adaped this code into our by adding camera movement using similar controls to the 3D movement controls. 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices;

public class PlayerController : MonoBehaviour
{
    public CharacterController player;//gets the character controller
    public Transform playertransform;
    //public GameObject playerobject;
    public float speed;//speed of the character movement
    public float monsterspeed;
    public float camspeed;//speed of the character movement
    public float gravity = 5.0f;//Gravity intensity
    public float timeleftboost = 10.0f;
    public Vector2 moveinput;//movement inputs
    public Vector2 lookinput;//camera rotation inputs
    private Vector3 movementDirection = Vector3.zero;//The direction the player is moving
    private Vector2 rotate = Vector2.zero;//A rotation vector
    private bool boost = false;
    public AudioSource HumanWalk;
    public AudioSource AlienWalk;
    //public Animator move;
    Animator animatorMonster;

    [DllImport("MonsterSpeed")]
    private static extern int MonsterSpeed();

    void Start()
    {
        animatorMonster = GetComponent<Animator>();
    }

    void Awake()
    {
        monsterspeed = speed + MonsterSpeed();
    }

    public void SpeedPowerUp()
    {
        if (boost == false)
        {
            monsterspeed = monsterspeed + MonsterSpeed();
            boost = true;
        }
    }
    public void Onmove(InputAction.CallbackContext context) => moveinput = context.ReadValue<Vector2>();//Similar to the button press this checks if WASD or the left analog stick is bing used
    public void Onlook(InputAction.CallbackContext context) => lookinput = context.ReadValue<Vector2>();//Similar to the button press this checks if IJKL or the right analog stick is being used

    // Update is called once per frame
    void Update()
    {
        //if(moveinput.x != 0 || moveinput.y != 0)
        //{
        //    animatorMonster.SetBool("isWalking", true);
        //}
        //else
        //{
        //    animatorMonster.SetBool("isWalking", false);
        //}


        //Debug.Log(MonsterSpeed());
        //camera look
        float xaxis = lookinput.x * Time.fixedDeltaTime * camspeed;//The input for the xaxis for camera movement
        float yaxis = lookinput.y * Time.fixedDeltaTime * camspeed;//The input for the yaxis for camera movement

        rotate.x += xaxis;//puts the inputs values into the rotation vector
        rotate.y += yaxis;
        rotate.y = Mathf.Clamp(rotate.y, -90.0f, 90.0f);//Clamps the camera for looking up and down so you do not look behind yourself when using up and down camera movements

        transform.localRotation = Quaternion.AngleAxis(rotate.x, Vector3.up) * Quaternion.AngleAxis(rotate.y, Vector3.left);//The rotation quaternion for camera movement

        //player movement
        movementDirection = new Vector3(moveinput.x, 0, moveinput.y);//Gets the input values
        movementDirection = transform.TransformDirection(movementDirection);

        if (boost == true)
        {
            timeleftboost -= Time.deltaTime;
            //Debug.Log(timeleftboost);
        }
        if(timeleftboost < 0)
        {
            boost = false;
            monsterspeed = monsterspeed - MonsterSpeed();
            timeleftboost = 10.0f;
        }

        if (moveinput.x != 0 || moveinput.y != 0)
        {
            //animatorMonster.SetBool("isWalking", true);
            if (gameObject.tag == "Player 2")
            {
                if (!AlienWalk.isPlaying)
                {
                    AlienWalk.Play();
                }
            }
            else
            {
                if (!HumanWalk.isPlaying)
                {
                    HumanWalk.Play();
                }
            }
        }
        else
        {
            //animatorMonster.SetBool("isWalking", false);
            AlienWalk.Stop();
            HumanWalk.Stop();
        }

        if (gameObject.tag == "Player 2")
        {
            movementDirection *= monsterspeed;
        }
        else
        {
            movementDirection *= speed;
        }
        movementDirection.y -= gravity;//Adds gravity

        player.Move(movementDirection * Time.deltaTime);//Moves the player
    }
}
