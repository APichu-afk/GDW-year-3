using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public CharacterController player;//gets the character controller
    public Transform playertransform;
    public float speed;//speed of the character movement
    public float camspeed;//speed of the character movement
    public float gravity = 15.0f;//Gravity intensity
    private Vector3 movementDirection = Vector3.zero;
    private Vector2 moveinput;
    private Vector2 lookinput;
    private Vector2 rotate = Vector2.zero;
    // Update is called once per frame
    void Update()
    {
        //camera look
        float xaxis = lookinput.x * Time.fixedDeltaTime * camspeed;
        float yaxis = lookinput.y * Time.fixedDeltaTime * camspeed;

        rotate.x += xaxis;
        rotate.y += yaxis;
        rotate.y = Mathf.Clamp(rotate.y, -90.0f, 90.0f);

        transform.localRotation = Quaternion.AngleAxis(rotate.x, Vector3.up) * Quaternion.AngleAxis(rotate.y, Vector3.left);

        //player movement
        movementDirection = new Vector3(moveinput.x, 0, moveinput.y);
        movementDirection = transform.TransformDirection(movementDirection);

        movementDirection *= speed;

        movementDirection.y -= gravity;

        player.Move(movementDirection * Time.deltaTime);
    }

    public void Onmove(InputAction.CallbackContext ctx) => moveinput = ctx.ReadValue<Vector2>();
    public void Onlook(InputAction.CallbackContext ctx) => lookinput = ctx.ReadValue<Vector2>();
}
