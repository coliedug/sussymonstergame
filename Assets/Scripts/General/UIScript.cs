using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    public static UIScript userInterface { get; private set; }
    [SerializeField] TextMeshProUGUI healthUI;
    [SerializeField] TextMeshProUGUI scoreUI;
    private void Awake()
    {
        if (userInterface == null)
        {
            userInterface = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdatePlayerHP(int newPlayerHP)
    {
        healthUI.text = "Health: " + newPlayerHP;
    }

    public void UpdateScore(int newScore)
    {
        scoreUI.text = newScore.ToString();
    }
}
