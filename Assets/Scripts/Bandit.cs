using UnityEngine;
using System.Collections;

/*
 * ----------------------------------------GENERAL INFO ABOUT MY GAME-------------------------------------------------
 * This is a simple platformer about grave robbing and avoidng the angry spirits you rob
 * To play use WASD or arrows to move, left click to attack, space to jump, spam space to climb ladders,
 * and jump on gravestones to break them.
 * 
 * The coding techniques I used in this are tilemaps, animation controllers, animation events, animation states, and encapsulation.
 * */

// this script is referenced from the demo code that came with the unity asset i got from free asset store
// it is not the exact same though and is transformed to fit the dimensions of my game

// this script acts as both a player controller and game manager
public class Bandit : MonoBehaviour 
{
    [SerializeField] 
    private float speed = 5.0f;
    [SerializeField] 
    private float jumpForce = 7.5f;

    public GameManager manager;
    public Animator banditAnimator;
    private Rigidbody2D rigidBody;
    private Sensor_Bandit groundSensor;


    private bool isDead = false;
    public bool playerAttackFinished = false;

    [SerializeField]
    private EnemyBehavior enemy;

    public int score = 0;
    public int gravesDestroyed = 0;

    void Start () 
    {
        banditAnimator = GetComponent<Animator>();
        rigidBody = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Bandit>();
    }

    void Update()
    {
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
        if (Input.GetMouseButtonDown(0))   // attacking
        {
            banditAnimator.SetTrigger("Attack");
            playerAttackFinished = false;
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
        if (collision.gameObject.tag == "enemy" && enemy.enemyAttackFinished)
        {
            enemy.enemyAttackFinished = false;
            banditAnimator.SetTrigger("Hurt");

            // score lost
        }
    }

    // function to add to the score when a grave is robbed
    // called from the GravestoneBehavior Script
    public void RobbingScore()
    {
        manager.ClearScore();

        score += 10;
        gravesDestroyed += 1;
    }

    // lets the enemy know the player exceuted attack animation
    private void AfterAttackEvent()
    {
        playerAttackFinished = true;
    }

    // after getting hurt animation
    private void AfterHurtEvent()
    {
        manager.ClearScore();

        score -= 2;
    }
}
