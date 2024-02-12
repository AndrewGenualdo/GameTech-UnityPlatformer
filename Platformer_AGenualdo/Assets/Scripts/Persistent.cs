using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Persistent : MonoBehaviour
{

    public static bool hasMoved = false;


    public static void SwitchScene(string sceneName)
    {
        if(sceneName == "Level1")
        {
            hasMoved = false;
        }
        SceneManager.LoadScene(sceneName);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}