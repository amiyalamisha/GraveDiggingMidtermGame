using UnityEngine;
using System.Collections;

// this code is referenced from the demo code that came with the unity asset i got from free asset store
// it is not the exact same though
public class Bandit : MonoBehaviour {

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
        //transform.localScale = fixScale;

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
            transform.localScale = new Vector3(1.5f, 1.5f, 1.0f);

        // Move
        body2d.velocity = new Vector2(inputX * speed, body2d.velocity.y);

        //Set AirSpeed in animator
        animator.SetFloat("AirSpeed", body2d.velocity.y);

        // -- Handle Animations --
        //Death
        if (Input.GetKeyDown("e")) {
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
            
        //Hurt
        else if (Input.GetKeyDown("q"))
            animator.SetTrigger("Hurt");

        //Attack
        else if(Input.GetMouseButtonDown(0)) {
            animator.SetTrigger("Attack");
        }

        //Change between idle and combat idle
        else if (Input.GetKeyDown("f"))
            combatIdle = !combatIdle;

        //Jump
        else if (Input.GetKeyDown("space") && animator.GetBool("Grounded")) 
        {
            animator.SetTrigger("Jump");
            animator.SetBool("Grounded", false);
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            animator.SetInteger("AnimState", 2);
        }

        //Combat Idle
        else if (combatIdle)
        {
            animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            animator.SetInteger("AnimState", 0);
        }
    }
}
