using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MonsterPlayer : MonoBehaviour
{
    public CharacterController player;
    public float speed;
    public float gravity;
    private Vector2 moveinput;
    private Vector3 movementDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = new Vector3(moveinput.x, 0, moveinput.y);
        movementDirection = transform.TransformDirection(movementDirection);

        movementDirection *= speed;

        movementDirection.y -= gravity;

        player.Move(movementDirection * Time.deltaTime);
    }

    public void inputs (InputAction.CallbackContext ctx) => moveinput = ctx.ReadValue<Vector2>();
}
