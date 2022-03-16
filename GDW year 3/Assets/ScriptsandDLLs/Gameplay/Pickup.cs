using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameObject spacepart;

   public void pick()
    {
          spacepart.SetActive(false);
    }
}
