using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (Input.GetKey(KeyCode.D)) {
            rb.velocity += new Vector2(0.1f, 0);
        }
        if(Input.GetKey(KeyCode.A))
        {
            rb.velocity += new Vector2(-0.1f, 0);
        }
        if(Input.GetKeyDown(KeyCode.W)) {
            rb.velocity += new Vector2(0, 50);
        }
        if(Input.GetKey(KeyCode.S))
        {
            rb.velocity += new Vector2(0, -0.1f);
        }
    }
}
