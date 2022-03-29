using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterPickUp : MonoBehaviour
{
    public GameObject PowerUp;
    public float respawntime = 20.0f;
    public bool respawning = false;

    public void pick()
    {
        PowerUp.SetActive(false);
        respawning = true;
    }

    void Update()
    {
        if (respawning == true)
        {
            respawntime -= Time.deltaTime;
        }
        if (respawntime < 0)
        {
            PowerUp.SetActive(true);
            respawning = false;
            respawntime = 20.0f;
        }
    }
}
