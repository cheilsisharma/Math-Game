using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BackButton1 : MonoBehaviour
{
    public void BackGame()
    {
        SceneManager.LoadScene("Main1");
    }
}
