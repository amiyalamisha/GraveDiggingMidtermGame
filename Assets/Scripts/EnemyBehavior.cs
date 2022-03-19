using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField]
    private float speed = 4.0f;
    private Rigidbody2D rigidBody;

    public Bandit player;
    public Animator enemyAnimator;
    public bool attackFinished = false;

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
        // will turn again if they hit a hidden enemy barrier
        if (collision.gameObject.tag == "Ewall")
        {
            speed *= -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // enemy dies if hit by the player
        if(collision.gameObject.tag == "Player" && player.playerAttack)
        {
            enemyAnimator.SetBool("dead", true);
        }

        if (collision.gameObject.tag == "Player" && !player.playerAttack)
        {
            // enemy attacks
            attackFinished = false;
            enemyAnimator.SetBool("attack", true);
        }
    }

    private void AfterAttackEvent()
    {
        enemyAnimator.SetBool("attack", false);
        attackFinished = true;
    }

    private void AfterDeadEvent()
    {
        Destroy(gameObject);
    }
    /*
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (enemyAnimator.GetBool("attack"))
        {
            enemyAnimator.SetBool("attack", false);
        }
    }*/
}
