using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController control;

    public GameObject endScreen;
    public GameObject titleScreen;
    public GameObject lessonScreen;
    public void Awake()
    {
        control = this;
        endScreen.SetActive(false);
        titleScreen.SetActive(true);
        lessonScreen.SetActive(false);
    }
    public void SwitchToARScene()
    {
        SceneManager.LoadScene("ARScene");
    }
    public void SwitchToEndScreen()
    {
        SceneManager.LoadScene("UI");

            endScreen.SetActive(true);
            titleScreen.SetActive(false);
            lessonScreen.SetActive(false);
        
    }


}
