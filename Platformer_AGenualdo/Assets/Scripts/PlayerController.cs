using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float bounceStrength = 20.0f;
    [SerializeField] float maxXVel = 35.0f;

    [SerializeField] string levelBefore;
    [SerializeField] string levelAfter;


    [SerializeField] Sprite neutral;
    [SerializeField] Sprite up;
    [SerializeField] Sprite down;
    [SerializeField] Sprite left;
    [SerializeField] Sprite right;

    [SerializeField] public float timeUntilWarning;

    GameObject hintText;

    Vector2 startPos = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        if(Persistent.scene == "Level1")
        {
            hintText = GameObject.Find("Canvas/HintText");
            hintText.SetActive(false);
        }
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        hasJumped = false;
        hasBounced = false;
        MovePlayer();
        LookAtMouse();

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Persistent.SwitchScene(levelAfter);
        } 
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Persistent.SwitchScene(levelBefore);
        } 
        else if(Input.GetKeyDown(KeyCode.Escape))
        {
            Persistent.SwitchScene("Menu");
        } else if(Input.GetKeyDown(KeyCode.R))
        {
            Persistent.ReloadScene();
        }
        if(Persistent.scene == "Level1")
        {
            timeUntilWarning -= Time.deltaTime;
            if (timeUntilWarning < 0 && !Persistent.hasMoused)
            {
                hintText.SetActive(true);
            }
            else
            {
                hintText.SetActive(false);
            }
        }
        
        if(transform.position.y < -50)
        {
            transform.position = startPos;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
        }
    }

    private void MovePlayer()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (ObjectController.INSTANCE.wasMouseDown)
        {
            //rb.velocity = Vector2.zero;
            //return;
        }

        //Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, gameObject.transform.position.y, Camera.main.transform.position.z);
        if (Input.GetKey(KeyCode.D))
        {
            //rb.velocity += new Vector2(0.1f, 0);
            rb.velocity = new Vector2(Mathf.Min(rb.velocity.x + 0.1f, maxXVel), rb.velocity.y);
            Persistent.hasMoved = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            //rb.velocity += new Vector2(-0.1f, 0);
            rb.velocity = new Vector2(Mathf.Max(rb.velocity.x - 0.1f, -maxXVel), rb.velocity.y);
            Persistent.hasMoved = true;
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity += new Vector2(0, -0.1f);
            Persistent.hasMoved = true;
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Jump(collision);
        Bounce(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Jump(collision);
        Bounce(collision);
    }

    private bool hasJumped = false;
    private bool hasBounced = false;

    private void Jump(Collision2D collision)
    {
        if(hasJumped)
        {
            return;
        }
        if (collision.gameObject.name.Contains("FLOOR"))
        {
            if (Input.GetKey(KeyCode.W))
            {
                BoxCollider2D playerBc = gameObject.GetComponent<BoxCollider2D>();
                BoxCollider2D floorBc = collision.gameObject.GetComponent<BoxCollider2D>();
                //check if player is on top of floor (no jumping on walls)
                if(transform.position.y - (transform.localScale.y * playerBc.size.y / 2) > collision.transform.position.y + (collision.transform.localScale.y * floorBc.size.y / 2) - 0.05)
                {
                    Rigidbody2D rb = GetComponent<Rigidbody2D>();
                    rb.velocity += new Vector2(0, 10);
                    hasJumped = true;
                    Persistent.hasMoved = true;
                }
                
                return;
            }
        }
    }

    
    


    private void Bounce(Collision2D collision)
    {
        if (hasBounced)
        {
            return;
        }
        if (collision.gameObject.name.StartsWith("BOUNCY"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (collision.gameObject.name.Contains("WALL"))
            {
                float velX = gameObject.transform.position.x > collision.transform.position.x ? Mathf.Max(bounceStrength, Mathf.Abs(rb.velocity.x)+bounceStrength/4) : Mathf.Min(-bounceStrength, -(Mathf.Abs(rb.velocity.x)+bounceStrength/4));

                rb.velocity = new Vector2(velX, rb.velocity.y + 5.0f);
            }
            else
            {
                BoxCollider2D playerBc = gameObject.GetComponent<BoxCollider2D>();
                BoxCollider2D floorBc = collision.gameObject.GetComponent<BoxCollider2D>();
                //Touching top of bouncer
                if (transform.position.y - (transform.localScale.y * playerBc.size.y / 2) > collision.transform.position.y + (collision.transform.localScale.y * floorBc.size.y / 2) - 0.05)
                {
                    float velY;
                    if (Input.GetKey(KeyCode.W))
                    {
                        velY = bounceStrength * 0.25f;
                    } else
                    {
                        velY = bounceStrength;
                    }
                    rb.velocity = new Vector2(rb.velocity.x, Mathf.Abs(rb.velocity.y)+velY);
                } 
                //Touching bottom of bouncer
                else if(transform.position.y + (transform.localScale.y * playerBc.size.y / 2) < collision.transform.position.y - (collision.transform.localScale.y * floorBc.size.y / 2) + 0.05)
                {
                    if(transform.position.x + (transform.localScale.x * playerBc.size.x / 2) > collision.transform.position.x - (collision.transform.localScale.x * floorBc.size.x / 2) + 0.05 && transform.position.x - (transform.localScale.x * playerBc.size.x / 2) < collision.transform.position.x + (collision.transform.localScale.x * floorBc.size.x / 2) - 0.05)
                    {
                        float velY = 2.5f * -Mathf.Abs(rb.velocity.y);
                        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y + velY);
                    }
                    
                }
                    
            }
            hasBounced = true;

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Checkpoint"))
        {
            Persistent.SwitchScene(levelAfter);
        }
    }

    private void LookAtMouse()
    {
        BoxCollider2D bc = GetComponent<BoxCollider2D>();
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        SpriteRenderer sr = rb.GetComponent<SpriteRenderer>();
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distX = mousePos.x - gameObject.transform.position.x;
        float distY = mousePos.y - gameObject.transform.position.y;

        //If mouse is inside player
        if(Mathf.Abs(distX) < gameObject.transform.localScale.x / 2 * bc.size.x && Mathf.Abs(distY) < gameObject.transform.localScale.y / 2 * bc.size.y)
        {
            sr.sprite = neutral;
        } //if mouse is farther on X axis than Y axis from player
        else if(Mathf.Abs(distX) > Mathf.Abs(distY))
        {
            sr.sprite = distX < 0 ? left : right;
        } //if mouse is farther on Y axis than X axis from player
        else
        {
            sr.sprite = distY < 0 ? down : up;
        }
    }

    
}
