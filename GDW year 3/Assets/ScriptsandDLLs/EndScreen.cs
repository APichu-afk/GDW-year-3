using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{

    public GameObject humanWin;
    public GameObject monsterWin;

    // Start is called before the first frame update
    void Start()
    {
        
    }



    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            SceneManager.LoadScene("MainMenu");
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (ButtonPresses.humanWin == true)
        {
            humanWin.SetActive(true);
            monsterWin.SetActive(false);
        }

        if(ButtonPresses.humanWin == false)
        {
            humanWin.SetActive(false);
            monsterWin.SetActive(true);
        }
    }
}
