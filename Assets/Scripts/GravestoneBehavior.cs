using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is also my own code
public class GravestoneBehavior : MonoBehaviour
{
    // total gravestones in level: 25

    public Bandit player;

    // collision with player breaks grave stone
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // breaking sound

        // destroys gravestone
        if(collision.gameObject.tag == "Player")
        {
            // calls score function to add to the score
            player.RobbingScore();

            // then the game object is immediately destroyed
            Destroy(gameObject);
        }
    }
}
