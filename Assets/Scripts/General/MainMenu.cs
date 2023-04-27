using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public AudioSource soundPlayer;
    public AudioClip clickSound;
    public AudioClip hoverSound;
    public void PlayGame()
    {
        SceneManager.LoadScene("Level_1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ClickSound()
    {
        soundPlayer.PlayOneShot(clickSound);
    }

    public void HoverSound()
    {
        soundPlayer.PlayOneShot(hoverSound);
    }
}
