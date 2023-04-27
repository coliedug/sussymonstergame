using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EndScreenScript : MonoBehaviour
{
    public AudioSource soundPlayer;
    public AudioClip clickSound;
    public AudioClip hoverSound;
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
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
