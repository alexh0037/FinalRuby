using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NonPlayerCharacter : MonoBehaviour
{
    
    public float displayTime = 4.0f;
    public GameObject dialogBox;
    float timerDisplay;
    public TextMeshProUGUI Dialogue;
    private bool LoadCheck;
    
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        LoadCheck = false;
    }
    
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);

                if(UIHealthBar.GameOver && LoadCheck == true)
                {
                   SceneManager.LoadScene("Level2");

                }
            }
        }
    }
    
    public void DisplayDialog()
    {
        if(UIHealthBar.GameOver)
        {
            Dialogue.text = "Congratulations on fixing the robots, head to my other property to fix the rest!";
            LoadCheck = true;

        }
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
    }
}