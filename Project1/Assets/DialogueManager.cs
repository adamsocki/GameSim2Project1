using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    //public GameObject gameManager;
    private float textTimerForLetters;

    public int currentLevel; 
    private int displayTextSequence;

    public Text displayText;
    public Text displayTextStartButton;
    

    public string nameText;
    public string nameTextAdded;
    private char c;
    public float textTimerDisplayRate;
    private bool startTitle;
    private float delayStartTimer;
    public float delayStart;
    private bool titleOver;
    private float titleOverTimer;
    public float titleOverTimeSet;

    public string startGameTextString;
    
    public enum MenuState
    {
        DelayStart,
        StartTitle,
        DelayPhase2,
        DisplayAll
    }

    public MenuState menuState;
    // Start is called before the first frame update
    void Start()
    {
        menuState = MenuState.DelayStart;
        titleOver = false;
        nameText = "uninhabited";
        nameTextAdded = "";
        displayTextSequence = -1;
        startGameTextString = "Start Game";
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentLevel == 0)
        {
            switch (menuState)
            {
                case MenuState.DelayStart:
                {
                    delayStartTimer += Time.deltaTime;
                    if (delayStartTimer > delayStart)
                    {
                        delayStartTimer = 0;
                        menuState = MenuState.StartTitle;
                    }
                } break;

                case MenuState.StartTitle:
                {
                    textTimerForLetters += Time.deltaTime;
                    if (displayTextSequence < nameText.Length - 1)
                    {
                        if (textTimerForLetters > textTimerDisplayRate)
                        {
                            displayTextSequence++;
                            textTimerForLetters = 0;
                            c = nameText[displayTextSequence];
                            nameTextAdded += c.ToString();
                        }
                    }
                    else
                    {
                        menuState = MenuState.DelayPhase2;
                    }
                } break;

                case MenuState.DelayPhase2:
                {   
                    delayStartTimer += Time.deltaTime;
                    if (delayStartTimer > delayStart)
                    {
                        delayStartTimer = 0;
                        menuState = MenuState.DisplayAll;
                    }
                } break;
                
                case MenuState.DisplayAll:
                {
                    displayTextStartButton.text = startGameTextString;
                    
                    
                    
                } break;
                
            }
            
            displayText.text = nameTextAdded;
            
            // if (delayStartTimer < delayStart)
            // {
            //     
            // }
            // if ( startTitle && (displayTextSequence < nameText.Length - 1))
            // {
            //     
            // }
            // else
            // {
            //     titleOverTimer += Time.deltaTime;
            //     if (titleOverTimer > titleOverTimeSet)
            //     {
            //         titleOver = true;
            //     }
            //     Debug.Log("titleOver");
            // }
            
        }
    }
}