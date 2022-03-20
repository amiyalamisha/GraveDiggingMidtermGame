using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// this is my own game manager
public class GameManager : MonoBehaviour
{
    public Bandit player;

    public int gameScore;
    public TextMeshProUGUI scoreText;

    void Update()
    {
        // displaying the score as text
        gameScore = player.score;
        scoreText.text = gameScore.ToString();      // i had to reference Alexander Zotov on Youtube for this
    }

    // clearing score everytime it changes
    public void ClearScore()
    {
        scoreText.text = string.Empty;
    }
}
