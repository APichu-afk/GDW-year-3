//https://www.codegrepper.com/code-examples/csharp/can+you+reference+ui+button+being+pressed+in+script+in+unity
//This website helped with the Button code

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPlane : MonoBehaviour
{
    Camera maincam;
    RaycastHit hitInfo;
    public Transform CubePrefab;
    public Transform CylinderPrefab;
    public Transform singleCubePrefab;
    public Button spawnButton;
    public Button cylinderButton;
    public Button cubeButton;
    public int press = 1;//spawner

    // Start is called before the first frame update
    void Awake()
    {
        maincam = Camera.main;
    }

    void Start()
    {
        Button spawnBut = spawnButton.GetComponent<Button>();
        spawnBut.onClick.AddListener(clickCheckSpawn);

        Button cylinderBut = cylinderButton.GetComponent<Button>();
        cylinderBut.onClick.AddListener(clickCheckCylinder);

        Button cubeBut = cubeButton.GetComponent<Button>();
        cubeBut.onClick.AddListener(clickCheckCube);
    }

    void clickCheckSpawn()
    {
        Debug.Log("You have clicked the button!");
        press = 2;
    }

    void clickCheckCylinder()
    {
        Debug.Log("You have clicked the button!");
        press = 1;
    }

    void clickCheckCube()
    {
        Debug.Log("You have clicked the button!");
        press = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = maincam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity))
            {
                Color c = new Color(Random.Range(0.5f, 1f), Random.Range(0.5f, 1f), Random.Range(0.5f, 1f));
                //CubePlacer.PlaceCube(hitInfo.point, c, cubePrefab);

                if (press == 1)
                {
                    ICommand command = new PlaceCubeCommand(hitInfo.point, c, CubePrefab);
                    CommandInvoker.AddCopmmand(command);
                }

                if (press == 2)
                {
                    ICommand command = new PlaceCubeCommand(hitInfo.point, c, CylinderPrefab);
                    CommandInvoker.AddCopmmand(command);
                }

                if (press == 3)
                {
                    ICommand command = new PlaceCubeCommand(hitInfo.point, c, singleCubePrefab);
                    CommandInvoker.AddCopmmand(command);
                }
                //CommandInvoker.AddCopmmand(command);
            }
        }
    }
}
