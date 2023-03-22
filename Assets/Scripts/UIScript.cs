using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    public static UIScript userInterface { get; private set; }
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
        GetComponentInChildren<TextMeshProUGUI>().text = "Health: " + newPlayerHP;
    }
}
