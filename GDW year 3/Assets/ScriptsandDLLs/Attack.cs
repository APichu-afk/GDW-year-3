//https://unity3d.college/2017/05/26/unity3d-design-patterns-state-basic-state-machine/
//Above is the link we used for the state design pattern
//We adapted the code by changing the states from animations into which player type the player is playing as. We also made the state of the object change based on a keypress.
//https://www.youtube.com/watch?v=sPiVz1k-fEs&ab_channel=Brackeys
//Above is the link I used as a reference for melee attacks for unity
//We adapted this code by changing it from attacks in a 2D space to a 3D space.
//We also referenced the Game Engine design lecture slides to get our button presses to work with the new unity input system.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Attack : MonoBehaviour
{
    public Transform attackpointhuman;//the middle of the attack hitbox for humans
    public Transform attackpointmonster;//the middle of the attack hitbox for monsters
    public float attackrange = 0.5f;//the radius of the attack
    public LayerMask monsterlayer;//gets the layer that the monster is on
    public LayerMask humanlayer;//gets the layer that the human is on
    public InputAction attack;//Attack button press
    public InputAction pickup;//pickup button press
    public InputAction start;//pickup button press
    public bool gamestart = false;//checks if the game has started
    public float humanhealth = 100.0f;// the humans health
    public GameObject player;//Gets the player object
    public playerstate currentstate;//Hides the player's state from the player
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
        start.performed += Onstart;

    }
    //If the attack button is pressed
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!gamestart)
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
            }
        }
    }
    //If the pick up button is pressed
    public void Onpickup(InputAction.CallbackContext context)
    {
        if (!gamestart)
        {
            //If the player has pressed the pick up button before the game has started they will become a monster
            currentstate = playerstate.Monster;
        }
        else
        {
            if (gameObject.tag == "Player 1")
            {

            }
        }
    }
    //If the start button is pressed
    public void Onstart(InputAction.CallbackContext context)
    {
        if (!gamestart)
        {
            //Starts the game
            gamestart = true;
        }
    }
    //the humans attack
    void humanAttack()
    {
        //Creates the hit box and checks what is being hit for the attack
        Collider[] hitmonsters = Physics.OverlapSphere(attackpointhuman.position, attackrange, monsterlayer);

        foreach(Collider monster in hitmonsters)
        {
            Debug.Log("hit");
        }
    }
    //the monsters attack
    void monsterAttack()
    {
        //Creates the hitbox and checks what is being hit for the attack
        Collider[] hithumans = Physics.OverlapSphere(attackpointmonster.position, attackrange, humanlayer);

        foreach (Collider human in hithumans)
        {
            if (humanhealth > 0.0f)
            {
                humanhealth -= 10.0f;
            }
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
                break;
            case playerstate.Monster:
                gameObject.tag = "Player 2";
                gameObject.layer = 3;
                break;
            case playerstate.Monsteronehit:
                gameObject.tag = "Player 2";
                gameObject.layer = 3;
                break;
        }
        Debug.Log(humanhealth);
        //Checks if the monsters win or not
        if (humanhealth <= 0)
        {
            Debug.Log("Monsters win");
            SceneManager.LoadScene("EndScreen");//When the monsters win go to the end screen
        }
    }

    //draws the hitbox for testing purposes
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackpointhuman.position, attackrange);
    }
}