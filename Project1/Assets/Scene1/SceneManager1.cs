using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using  UnityEngine.UI;

public class SceneManager1 : MonoBehaviour
{
    public GameObject fsmManager;
    public float timeToSwitchLevel;
    public GameObject fadeObject;
    public GameObject fadeScreen;
    public bool playIntroFade;
    public GameObject fadeToBlackScreen;
    public bool playEndFade;
    public int currentLevel;

    public float endLevelTimer;
    private float startTimer;
    public GameObject fogObject;

    public GameObject player;
    public bool startPlayerPosition;
    
    public GameObject SceneManagerLevel;
    public GameObject cameraManager;
    public float startPositionDelay;
    public Vector3[] playerPositionForLevelStarting;
    public Vector3[] killerFogPositionForLevelStarting;
    public enum DisplayState
    {
        start,
        playing,
        end
    }

    public DisplayState _displayState;
    
    // Start is called before the first frame update
    void Start()
    {
        _displayState = DisplayState.start;
        // player Start positions
        playerPositionForLevelStarting = new Vector3[10];
        playerPositionForLevelStarting[0] = new Vector3(-15.2f, 1f, 0f);
        playerPositionForLevelStarting[1] = new Vector3(58.2f, -4.91f, 0f);
        playerPositionForLevelStarting[2] = new Vector3(107.5f, -3.3f, 0f);
        playerPositionForLevelStarting[3] = new Vector3(174f, -3.56f, 0f);


        killerFogPositionForLevelStarting = new Vector3[10];
        killerFogPositionForLevelStarting[0] = new Vector3(-28.1359f, 5.3909f, 0f);
        killerFogPositionForLevelStarting[1] = new Vector3(42.3f, 0f, 0f);
        killerFogPositionForLevelStarting[2] = new Vector3(85f, 0f, 0f); 
        killerFogPositionForLevelStarting[3] = new Vector3(160.47f, -0.1f, 0f); 


    }

    // Update is called once per frame
    void Update()
    {
        switch (_displayState)
        {
            case DisplayState.start:
            {
                fsmManager.GetComponent<FSMController>().state = FSMController.State.Idle;
                fsmManager.GetComponent<FSMController>().levelTransitioning = true;
                //startPositionDelay
                if (startPlayerPosition)
                {
                    startTimer += Time.deltaTime;
                    if (startTimer > 3.0f)
                    {
                        player.GetComponent<Transform>().position = playerPositionForLevelStarting[currentLevel];
                        fogObject.GetComponent<Transform>().position = killerFogPositionForLevelStarting[currentLevel];
                        startPlayerPosition = false;
                        //TODO: NEED TO SET THIS TO ONLY BE ON START WHEN THE LEVEL ACTUALLY STARTS. I"TS NOT GOIN TO END LEVEL SPCAECE
                        startTimer = 0;
                        _displayState = DisplayState.playing;
                    }
                }
                if (playIntroFade)
                {
                    fadeScreen.GetComponent<Animation>().Play("FadeAnim");
                    playIntroFade = false;
                }
                // TODO: Set up transition start
            } break;

            case DisplayState.playing:
            {
                fadeScreen.SetActive(false);
                fsmManager.GetComponent<FSMController>().levelTransitioning = false;
            } break;

            case DisplayState.end:
            {
                // TODO: Set up transition end
                fadeToBlackScreen.SetActive(true);
                startPlayerPosition = true;
                fsmManager.GetComponent<FSMController>().state = FSMController.State.ActivelyMoving;
                fsmManager.GetComponent<FSMController>().levelTransitioning = true;
            } break;
            
        }

        if (_displayState == DisplayState.end)
        {
            endLevelTimer += Time.deltaTime;
            fadeObject.GetComponent<FadeToWhite>().fadeOut = true;
            if (playEndFade)
            {
                fadeToBlackScreen.GetComponent<Animation>().Play("FadeToBlack");
                playEndFade = false;

            }
            if (endLevelTimer > timeToSwitchLevel)
            {
                currentLevel++;
                endLevelTimer = 0;
                _displayState = DisplayState.start;
                playIntroFade = true;
                playEndFade = true;
                fadeToBlackScreen.SetActive(false);
                fsmManager.GetComponent<FSMController>().state = FSMController.State.Idle;
                
                cameraManager.GetComponent<CameraManager>().changeLevel = true;
                startPlayerPosition = true;
                // Scene currentScene = SceneManager.GetActiveScene();
                //
                // Debug.Log(currentScene.name);
                // if (currentScene.name == "Scene1")
                // {
                //     _dis
                // }
            }
            
        }

        
    }
}
