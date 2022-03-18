using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravestoneBehavior : MonoBehaviour
{
    //Bandit banditAnimator Ani;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // collision with player breaks grave stone
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // breaking particle effect

        // destroys gravestone
        if(collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
