using UnityEngine;
using System.Collections;

// this code is referenced from the demo code that came with the unity asset i got from free asset store
// it is not the exact same though
public class Bandit : MonoBehaviour 
{
    [SerializeField] 
    private float speed = 5.0f;
    [SerializeField] 
    private float jumpForce = 7.5f;

    private Animator animator;
    private Rigidbody2D body2d;
    private Sensor_Bandit groundSensor;
    private bool combatIdle = false;
    private bool isDead = false;

    // Use this for initialization
    void Start () 
    {
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }

    // Update is called once per frame
    void Update()
    {
        //Check if character just landed on the ground
        if (!animator.GetBool("Grounded") && groundSensor.State())
        {
            animator.SetBool("Grounded", true);
        }

        //Check if character just started falling
        if (animator.GetBool("Grounded") && !groundSensor.State())
        {
            animator.SetBool("Grounded", false);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1.0f);
        }
        else if (inputX < 0)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);
        }

        // Move horizontally
        body2d.velocity = new Vector2(inputX * speed, body2d.velocity.y);

        //Set AirSpeed in animator
        animator.SetFloat("AirSpeed", body2d.velocity.y);

        // -- Handle Animations --
        //Death
        if (Input.GetKeyDown("e"))
        {
            if (!isDead)
            {
                animator.SetTrigger("Death");
            }
            else
            {
                animator.SetTrigger("Recover");
            }

            isDead = !isDead;
        }
        else if (Input.GetKeyDown("q"))         // getting hurt
        {
            animator.SetTrigger("Hurt");
        }
        else if (Input.GetMouseButtonDown(0))   // attacking
        {
            animator.SetTrigger("Attack");
        }
        else if (Input.GetKeyDown("f"))         // Change between idle and combat idle
        { 
            combatIdle = !combatIdle;
        }
        else if (Input.GetKeyDown("space") && animator.GetBool("Grounded"))     // jumping
        {
            animator.SetTrigger("Jump");
            animator.SetBool("Grounded", false);
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)     // running
        {
            animator.SetInteger("AnimState", 2);
        }
        else if (combatIdle)                    // idle during combat
        {
            animator.SetInteger("AnimState", 1);
        }
        else                                    // normal idle
        {
            animator.SetInteger("AnimState", 0);
        }
    }

    // this is my collision code for interacting with other prefabs
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // bounces off gravestone
        if (collision.gameObject.tag == "grave")
        {
            animator.SetTrigger("Jump");
            animator.SetBool("Grounded", false);
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }
        /*
        if(collision.gameObject.tag == "ladder" && Input.GetKeyDown("w"))
        {
            Debug.Log("ladder");
            float inputY = Input.GetAxis("Vertical");

            body2d.gravityScale = 0;
            body2d.velocity = new Vector2(body2d.velocity.y, inputY * speed);
        }*/
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        body2d.gravityScale = 1;
    }
}
