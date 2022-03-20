using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is my own enemy behavior code with some reference from my bandit movements
public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    private float speed = 4.0f;
    private Rigidbody2D rigidBody;

    public Bandit player;
    public Animator enemyAnimator;
    public bool enemyAttackFinished = false;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // automatic enemy pacing
        rigidBody.velocity = new Vector2(speed, rigidBody.velocity.y);

        // changing direction facing of enemy when pacing
        if(speed < 0)
        {
            transform.localScale = new Vector3(-4f, 4f, 1.0f);
        }
        else
        {
            transform.localScale = new Vector3(4f, 4f, 1.0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // will turn and bounce off if they hit a hidden enemy barrier
        if (collision.gameObject.tag == "Ewall")
        {
            speed *= -1;        // reverses the speed
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // enemy dies if hit by the player
        if(collision.gameObject.tag == "Player" && player.playerAttackFinished)
        {
            // the player attacking is done so now ther is no need for this to be true until they attack again
            player.playerAttackFinished = false;
            enemyAnimator.SetBool("dead", true);
        }

        // enemy automatically attacks if player doesn't attack first
        if (collision.gameObject.tag == "Player" && !player.playerAttackFinished)
        {
            // the enemy attack animation is not finished yet
            enemyAttackFinished = false;
            enemyAnimator.SetBool("attack", true);
        }
    }

    // lets the player know the enemy exceuted attack animation
    private void AfterAttackEvent()
    {
        enemyAnimator.SetBool("attack", false);
        enemyAttackFinished = true;         // animation is now finished
    }

    // destroys enemy object after death animation plays
    private void AfterDeadEvent()
    {
        Destroy(gameObject);
    }
}
