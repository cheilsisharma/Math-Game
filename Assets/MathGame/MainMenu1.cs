using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu1 : MonoBehaviour
{
    public GameObject mMainMenu;
    public GameObject mInstructions;

    public void PlayGame1()
    {
        SceneManager.LoadScene("gameplay");
    }

    public void QuitGame1()
    {
        SceneManager.LoadScene("Classroom");
    }

    public void InstGame1()
    {
        mInstructions.SetActive(true);
        mMainMenu.SetActive(false);

    }

    public void StartGame1()
    {
        SceneManager.LoadScene("Main1");
    }

    public void Back1()
    {
        mInstructions.SetActive(false);
        mMainMenu.SetActive(true);
    }
}
