//https://unity3d.college/2017/05/26/unity3d-design-patterns-state-basic-state-machine/
//Above is the link we used for the state design pattern
//We adapted the code by changing the states from animations into which player type the player is playing as. We also made the state of the object change based on a keypress.
//https://www.youtube.com/watch?v=sPiVz1k-fEs&ab_channel=Brackeys
//Above is the link I used as a reference for melee attacks for unity
//We adapted this code by changing it from attacks in a 2D space to a 3D space, and we also updated the pick up mechanic to use the hit detection found in this video.
//We also referenced the Game Engine design lecture slides to get our button presses to work with the new unity input system, 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonPresses : MonoBehaviour
{
    public Transform attackpointhuman;//the middle of the attack hitbox for humans
    public Transform attackpointmonster;//the middle of the attack hitbox for monsters
    public CharacterController player;
    private float attackrange = 1.0f;//the radius of the attack
    public LayerMask monsterlayer;//gets the layer that the monster is on
    public LayerMask humanlayer;//gets the layer that the human is on
    public LayerMask spacepartlayer;//gets the layer that the human is on
    public LayerMask shiplayer;
    public LayerMask speedboost;
    public InputAction attack;//Attack button press
    public InputAction pickup;//pickup button press
    public float humanhealth = 100.0f;// the humans health
    public Text UIhealth;
    public Text UISpacepartsamount;
    public Text UIHolding;
    public int spacepartscount = 0;
    public CanvasGroup humanUI;
    public CanvasGroup monsterUI;
    public float monsterhealth = 2.0f;
    private playerstate currentstate;//Hides the player's state from the player
    private bool picked = false;
    public int wincounter = 0;
    private Vector3 spawner1 = new Vector3(-40, 6, -40);//40, -37
    private Vector3 spawner2 = new Vector3(-65, 10, 44);//-65, 44
    private Vector3 spawner3 = new Vector3(-141, 6, 53);//-141, 53
    private Vector3 spawner4 = new Vector3(-211, 6, 2);//-211, 2
    private Vector3 spawner5 = new Vector3(-191, 10, -96);//-191, -96
    private Vector3 spawner6 = new Vector3(-111, 10, -114);//-111, -114
    private int spawn;
    public ReadyCheck var;
    public GameObject human;
    public GameObject monster;
    public GameObject ThrusterUI;
    public GameObject Thruster;
    public GameObject WingUI;
    public GameObject Wing;
    public GameObject BlasterUI;
    public GameObject Blaster;
    public GameObject Camera;
    public GameObject flashLight;
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    private int humanpainsound;
    private int monsterpainsound;
    public AudioSource HumanPain;
    public AudioSource HumanPain2;
    public AudioSource HumanPain3;
    public AudioSource AlienPain;
    public AudioSource AlienPain2;
    public AudioSource Shipfix;
    public Camera playercamera;

    //Animation
    Animator animatorMonster;

    void Start()
    {
        animatorMonster = GetComponent<Animator>();
        SetMaxHealth(humanhealth);
        fill.color = gradient.Evaluate(1f);
    }
    //The different states a player can be in
    public enum playerstate
    {
        Human,
        Monster,
        Monsteronehit,
    }

    //checks if the buttons are pressed
    public void Awake()
    {
        attack.performed += OnAttack;
        pickup.performed += Onpickup;
        var = GameObject.Find("Player Manager").GetComponent<ReadyCheck>();
        Thruster = GameObject.Find("Thruster");
        Wing = GameObject.Find("Wing");
        Blaster = GameObject.Find("BlasterTip");
        Camera.GetComponent<DeferredNightVisionEffect>().enabled = false;
    }
    //If the attack button is pressed
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!var.gamestart)
            {
                //If the player presses the attack button before the game has started they will become a human
                currentstate = playerstate.Human;


            }
            else
            {
                if (gameObject.tag == "Player 1")
                {
                    //If the player is a human they will do a human attack
                    humanAttack();
                }
                if (gameObject.tag == "Player 2")
                {
                    //If the player is a monster they will do a monster attack
                    monsterAttack();
                    animatorMonster.SetBool("isAttack", true);
                }
            }
        }
    }
    //If the pick up button is pressed
    public void Onpickup(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!var.gamestart)
            {
                //If the player has pressed the pick up button before the game has started they will become a monster
                currentstate = playerstate.Monster;


            }
            else
            {
                if (gameObject.tag == "Player 1")
                {
                    humanpickup();
                }
                if (gameObject.tag == "Player 2")
                {
                    monsterpickup();
                }
            }
        }
    }
    //the humans attack
    void humanAttack()
    {
        //Creates the hit box and checks what is being hit for the attack
        Collider[] hitmonsters = Physics.OverlapSphere(attackpointhuman.position, attackrange, monsterlayer);

        foreach (Collider monster in hitmonsters)
        {
            monster.GetComponent<ButtonPresses>().monsterhit();
        }
    }
    void humanpickup()
    {
        Collider[] hitspaceparts = Physics.OverlapSphere(attackpointhuman.position, attackrange, spacepartlayer);

        foreach (Collider spacepart in hitspaceparts)
        {
            if (!picked)
            {
                spacepartscount += 1;
                spacepart.GetComponent<Pickup>().pick();
                if (spacepart.name == Thruster.name)
                {
                    ThrusterUI.SetActive(true);
                }
                if (spacepart.name == Wing.name)
                {
                    WingUI.SetActive(true);
                }
                if (spacepart.name == Blaster.name)
                {
                    BlasterUI.SetActive(true);
                }
                picked = true;
            }
        }

        Collider[] ship = Physics.OverlapSphere(attackpointhuman.position, attackrange, shiplayer);

        foreach (Collider shipcollider in ship)
        {
            if (picked)
            {
                if (!Shipfix.isPlaying)
                {
                    Shipfix.Play();
                }
                wincounter += 1;
                picked = false;
            }
        }

    }
    //the monsters attack
    void monsterAttack()
    {
        //Creates the hitbox and checks what is being hit for the attack
        Collider[] hithumans = Physics.OverlapSphere(attackpointmonster.position, attackrange, humanlayer);

        foreach (Collider human in hithumans)
        {
            human.GetComponent<ButtonPresses>().humanhit();
        }

    }

    void monsterpickup()
    {
        Collider[] mushrooms = Physics.OverlapSphere(attackpointmonster.position, attackrange, speedboost);
        foreach (Collider mush in mushrooms)
        {
            //Debug.Log("Mush");
            gameObject.GetComponent<PlayerController>().SpeedPowerUp();
            mush.GetComponent<MonsterPickUp>().pick();
        }
    }
    void monsterhit()
    {
        monsterpainsound = Random.Range(1, 3);
        switch (monsterpainsound)
        {
            case 1:
                if (!AlienPain.isPlaying)
                {
                    AlienPain.Play();
                }
                break;
            case 2:
                if (!AlienPain2.isPlaying)
                {
                    AlienPain2.Play();
                }
                break;
        }
        monsterhealth -= 1;

    }

    void humanhit()
    {
        humanhealth -= 1.5f;
        SetHealth(humanhealth); //UI
        fill.color = gradient.Evaluate(slider.normalizedValue); //UI
        humanpainsound = Random.Range(1, 4);
        switch (humanpainsound)
        {
            case 1:
                if (!HumanPain.isPlaying)
                {
                    HumanPain.Play();
                }
                break;
            case 2:
                if (!HumanPain2.isPlaying)
                {
                    HumanPain2.Play();
                }
                break;
            case 3:
                if (!HumanPain3.isPlaying)
                {
                    HumanPain3.Play();
                }
                break;
        }
    }

    void Update()
    {
        //State design pattern
        switch (currentstate)
        {
            case playerstate.Human:
                gameObject.tag = "Player 1";
                gameObject.layer = 6;
                Camera.GetComponent<DeferredNightVisionEffect>().enabled = false;
                flashLight.SetActive(true);
                human.SetActive(true);
                monster.SetActive(false);
                playercamera.transform.localPosition = new Vector3(0, 0.8f, 0);
                //nightVis.GetComponent<DeferredNightVisionEffect>().OnDisable();
                break;
            case playerstate.Monster:
                gameObject.tag = "Player 2";
                gameObject.layer = 3;
                Camera.GetComponent<DeferredNightVisionEffect>().enabled = true;
                flashLight.SetActive(false);
                monster.SetActive(true);
                human.SetActive(false);
                playercamera.transform.localPosition = new Vector3(0, 0.25f, 0.4f);
                break;
        }
        //UI health
        Debug.Log(humanhealth);
        UIhealth.text = humanhealth.ToString();

        UISpacepartsamount.text = spacepartscount.ToString();
        if (picked == true)
        {
            UIHolding.text = "Holding";
        }
        else
        {
            UIHolding.text = "Hands free";
        }
        if (gameObject.tag == "Player 1")
        {
            humanUI.alpha = 1.0f;
        }
        if (gameObject.tag == "Player 2")
        {
            humanUI.alpha = 0.0f;
        }

        //Checks if the monsters win or not (Monster win condition moved here)
        if (humanhealth <= 0)
        {
            Debug.Log("Monsters win");
            SceneManager.LoadScene("EndScreen");//When the monsters win go to the end screen
        }
        spawn = Random.Range(1, 7);
        if (monsterhealth <= 0)
        {
            switch (spawn)
            {
                case 1:
                    player.transform.position = spawner1;
                    break;
                case 2:
                    player.transform.position = spawner2;
                    break;
                case 3:
                    player.transform.position = spawner3;
                    break;
                case 4:
                    player.transform.position = spawner4;
                    break;
                case 5:
                    player.transform.position = spawner5;
                    break;
                case 6:
                    player.transform.position = spawner6;
                    break;
            }

            monsterhealth = 2;
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (gameObject.tag == "Player 2")
            {
                switch (spawn)
                {
                    case 1:
                        player.transform.position = spawner1;
                        break;
                    case 2:
                        player.transform.position = spawner2;
                        break;
                    case 3:
                        player.transform.position = spawner3;
                        break;
                    case 4:
                        player.transform.position = spawner4;
                        break;
                    case 5:
                        player.transform.position = spawner5;
                        break;
                    case 6:
                        player.transform.position = spawner6;
                        break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            humanhit();
        }
            if (wincounter == 3)
        {
            SceneManager.LoadScene("EndScreen");
        }
    }

    //draws the hitbox for testing purposes
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackpointhuman.position, attackrange);
    }

    public void SetHealth(float health)
    {
        slider.value = health;
    }

    public void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
}