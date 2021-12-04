using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIHealthBar : MonoBehaviour
{
    public static UIHealthBar instance { get; private set; }
    
    public Image mask;
    float originalSize;
    public int TotalRobots = 1;
    public Text GameOverText;
    public Text AmmoText;
    public Text RobotText;
    public RubyController player;
    public AudioSource speaker;
    public AudioClip WinMusic;
    public AudioClip LoseMusic; 
    public static bool GameOver; 
    public GameObject PauseMenu;
    public GameObject InstructionMenu;

    void Awake()
    {
        instance = this;
    }

    public void ToggleInstructions(bool Instructions)
    {
        InstructionMenu.SetActive(Instructions);
    }


        public void pause()
        {
            if(Time.timeScale == 1)
            {
                PauseMenu.SetActive(true);
                Time.timeScale = 0;
            }

            else
            {
                PauseMenu.SetActive(false);
                Time.timeScale = 1;
                ToggleInstructions(false);
            }
        }

        public void quit()
        {
            Application.Quit();
        }


    void Start()
    {
        originalSize = mask.rectTransform.rect.width;
        EnemyController.RobotCount = 0; 
        GameOver = false;
        PauseMenu.SetActive(false);
        InstructionMenu.SetActive(false);
    }

        public void restart()
        {
           SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }


    void Update()
    {
        AmmoText.text = "Gears: " + player.GearCount;
        RobotText.text = "Robots Fixed: " + EnemyController.RobotCount;

        if(Input.GetKeyDown(KeyCode.P))
        {
            pause();
        }

        if(player.health <= 0 && GameOver == false)
        {
            speaker.Stop();
            speaker.clip = LoseMusic;
            speaker.Play();
            GameOverText.gameObject.SetActive(true);
            GameOverText.text = "You Lose!  Press R to restart";
            GameOver = true;
        }

        if(Input.GetKeyDown(KeyCode.R) && GameOver == true)
            {
                restart();
            }

        if(EnemyController.RobotCount == TotalRobots && GameOver == false)
        {
            speaker.Stop();
            speaker.clip = WinMusic;
            speaker.Play();
            GameOverText.gameObject.SetActive(true);
            GameOver = true;

    
        }
    }

    public void SetValue(float value)
    {				      
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * value);
    }
}