using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Persistent.hasMoved = false;
        Persistent.hasMoused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Persistent.SwitchScene("Level1");
        }
    }
}
