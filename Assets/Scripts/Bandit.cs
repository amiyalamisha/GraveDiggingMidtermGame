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

    public Animator banditAnimator;
    private Rigidbody2D rigidBody;
    private Sensor_Bandit groundSensor;
    private bool combatIdle = false;
    private bool isDead = false;
    public bool playerAttack = false;

    [SerializeField]
    private EnemyBehavior enemy;

    void Start () 
    {
        banditAnimator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }

    void Update()
    {
        playerAttack = false;
        //Check if character just landed on the ground
        if (!banditAnimator.GetBool("Grounded") && groundSensor.State())
        {
            banditAnimator.SetBool("Grounded", true);
        }

        //Check if character just started falling
        if (banditAnimator.GetBool("Grounded") && !groundSensor.State())
        {
            banditAnimator.SetBool("Grounded", false);
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
        rigidBody.velocity = new Vector2(inputX * speed, rigidBody.velocity.y);

        //Set AirSpeed in animator
        banditAnimator.SetFloat("AirSpeed", rigidBody.velocity.y);

        // -- Handle Animations --
        //Death
        if (Input.GetKeyDown("e"))
        {
            if (!isDead)
            {
                banditAnimator.SetTrigger("Death");
            }
            else
            {
                banditAnimator.SetTrigger("Recover");
            }

            isDead = !isDead;
        }
        else if (Input.GetKeyDown("q"))         // getting hurt
        {
            banditAnimator.SetTrigger("Hurt");
        }
        else if (Input.GetMouseButtonDown(0))   // attacking
        {
            banditAnimator.SetTrigger("Attack");
            playerAttack = true;
        }
        else if (Input.GetKeyDown("f"))         // Change between idle and combat idle
        { 
            combatIdle = !combatIdle;
        }
        else if (Input.GetKeyDown("space") && banditAnimator.GetBool("Grounded"))     // jumping
        {
            banditAnimator.SetTrigger("Jump");
            banditAnimator.SetBool("Grounded", false);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)     // running
        {
            banditAnimator.SetInteger("AnimState", 2);
        }
        else if (combatIdle)                    // idle during combat
        {
            banditAnimator.SetInteger("AnimState", 1);
        }
        else                                    // normal idle
        {
            banditAnimator.SetInteger("AnimState", 0);
        }
    }
    
    // this is my collision code for interacting with other prefabs
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // bounces off gravestone
        if (collision.gameObject.tag == "grave")
        {
            banditAnimator.SetTrigger("Jump");
            banditAnimator.SetBool("Grounded", false);
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }

        // if the enemy hits you
        if (collision.gameObject.tag == "enemy" && enemy.attackFinished)
        {
            enemy.attackFinished = false;
            banditAnimator.SetTrigger("Hurt");
            // lose a life animation idk
        }
    }
}
