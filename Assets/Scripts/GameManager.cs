using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Bandit player;

    public int gameScore;
    public TextMeshProUGUI scoreText;
    //public TextMeshProUGUI endOfGame;

    void Update()
    {
        // displaying the score as text
        gameScore = player.score;
        scoreText.text = gameScore.ToString();      // i had to reference Alexander Zotov on Youtube for this
        /*
        if(player.gravesDestroyed == 25)
        {
            endOfGame.enabled = true;
        }
        else
        {
            endOfGame.enabled = false;
        }*/
    }

    public void ClearScore()
    {
        scoreText.text = string.Empty;
    }
}
