using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem scoreSystem { get; private set; }
    int score = 0;

    void Start()
    {
        if (scoreSystem == null)
        {
            scoreSystem = this;
            Debug.Log(gameObject);
        }
        else
        {
            Debug.Log(gameObject);
            //Destroy(gameObject);
        }
    }

    public void UpdateScore(int changeAmount)
    {
        score += changeAmount;
        UIScript.userInterface.UpdateScore(score);
    }
}
