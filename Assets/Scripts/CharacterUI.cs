using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CharacterUI : MonoBehaviour
{
    GameObject player;
    public int currentChar;
    Image currentImg;
    [SerializeField] Sprite[] sprites;
    public bool tank;

    void Start()
    {
        currentImg = gameObject.GetComponent<Image>();
        player = PlayerController.player;
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerController cs = player.GetComponent<PlayerController>();
        currentChar = cs.currentChar;
        if(currentChar == 1 && tank == true)
        {
            currentImg.sprite = sprites[0];
        }
        else if(currentChar == 2 && tank == true)
        {
            currentImg.sprite = sprites[1];
        }
        else if(currentChar == 1 && tank == false)
        {
            currentImg.sprite = sprites[1];
        }
        else if(currentChar == 2 && tank == false)
        {
            currentImg.sprite = sprites[0];
        }

    }
}
