using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Level_Changer_Script : MonoBehaviour
{
    public Animator animator;
    private string endScreen;
    GameObject EndLevelObject;
    public bool isEnd = false;
 
    private void Start()
    {
        EndLevelObject = GameObject.Find("EndLevel");
    }
    void Update()
    {
        EndLevel cs = EndLevelObject.GetComponent<EndLevel>();
        isEnd = cs.isEnd;
        if (isEnd == true)
        {
            FadeToLevel("EndScreen");
        }
    }

    public void FadeToLevel (string endscreen)
    {
        endScreen = endscreen;
        animator.SetTrigger("fadeout");
    }

    public void onFadeComplete()
    {
        print("done");
        SceneManager.LoadScene("EndScreen");
    }
}
