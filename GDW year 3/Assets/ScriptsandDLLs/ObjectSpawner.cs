//https://thehardestwork.com/2020/12/10/how-to-build-a-spawner-in-unity/
//The above link is the reference we used for the spawner in our assignment. 
//We adapted this code to fit in this assignment by adding the z dimension to the placement of the objects, and we also changed how the random number is assigned we get from the random function
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject SpawningTheCubes;//Gets the game object to spawn
    public GameObject initialSpawn;//Gets the initial object to spawn from
    public int amount;//The amount of objects that will spawn
    public int limit;//The limit
    public float rate;//The rate at which objects will spwan
    float InGameTimer;

    //Our DLL that makes more boxes spawn
    [DllImport("morebox")]
    private static extern int morebox();
    // Start is called before the first frame update
    void Start()
    {
        InGameTimer = rate;//Sets the up the timer
        amount = amount + morebox();//Adds the dll value to the amount that will spawn
    }

    // Update is called once per frame
    void Update()
    {
        
        if (initialSpawn.transform.childCount < limit)
        {
            InGameTimer -= Time.fixedDeltaTime;//counts down based on the deltatime
            if (InGameTimer <= 0f)
            {
                for (int i = 0; i < amount; i++)
                {
                    //Spawns the objects in an area around the initial one
                    Instantiate(SpawningTheCubes, new Vector3(this.transform.position.x + RandomNumber(), this.transform.position.y + RandomNumber(), this.transform.position.z + RandomNumber()), Quaternion.identity, initialSpawn.transform);
                }
                InGameTimer = rate;//resets the timer for the next object to spaen
            }
        }
    }

    //Gets a random number between -5 and 5
    float RandomNumber()
    {
        float number = UnityEngine.Random.Range(-5f, 5f);
        return number;
    }
}
