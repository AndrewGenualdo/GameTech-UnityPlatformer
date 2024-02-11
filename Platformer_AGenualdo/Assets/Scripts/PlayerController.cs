using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] string levelBefore;
    [SerializeField] string levelAfter;


    [SerializeField] Sprite neutral;
    [SerializeField] Sprite up;
    [SerializeField] Sprite down;
    [SerializeField] Sprite left;
    [SerializeField] Sprite right;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
        LookAtMouse();

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SceneManager.LoadScene(levelAfter);
        } else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SceneManager.LoadScene(levelBefore);
        }

    }

    private void movePlayer()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (ObjectController.INSTANCE.wasMouseDown)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        
        SpriteRenderer sr = rb.GetComponent<SpriteRenderer>();
        //Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, gameObject.transform.position.y, Camera.main.transform.position.z);
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity += new Vector2(0.1f, 0);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity += new Vector2(-0.1f, 0);
  
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            rb.velocity += new Vector2(0, -0.1f);
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.name.Contains("FLOOR"))
        {
            if (Input.GetKey(KeyCode.W))
            {
                Rigidbody2D rb = GetComponent<Rigidbody2D>();
                rb.velocity += new Vector2(0, 10);
                return;
            }
        }
    }

    [SerializeField] float bounceStrength = 20.0f;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name.StartsWith("BOUNCY"))
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (collision.gameObject.name.Contains("WALL"))
            {
                float velX = gameObject.transform.position.x > collision.transform.position.x ? Mathf.Max(bounceStrength, -rb.velocity.x) : Mathf.Min(-bounceStrength, -rb.velocity.x);

                rb.velocity = new Vector2(velX, rb.velocity.y + 5.0f);
            }
            else
            {

                float velY = Mathf.Max(bounceStrength * (Input.GetKey(KeyCode.W) ? 1 : 0.5f), -rb.velocity.y);
                rb.velocity += new Vector2(rb.velocity.x, velY);
            }
            
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Checkpoint"))
        {
            SceneManager.LoadScene(levelAfter);
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
