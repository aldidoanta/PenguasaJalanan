using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//Manages game flow
public class GameplayManager : MonoBehaviour
{

    //gameplay handlers
    public float gameTime = 60.0f; //Time in a game (in seconds)
    public GameObject[] players; //array of player GameObject, its length is the number of players in a game
    private List<PlayerControl> playerControl; //control handler for each player

    //score for each player
    public int[] playerKnockPoint;
    public int[] playerFallPoint;

    //pause handlers
    private bool isGamePaused; //flag for pausing game
    private Text pauseText;

    //for the beginning of the stage
    private bool isGameStarted;
    private Image getreadyText;
    private Image fightText;

    //array of GameObject containing texts in the game
    public GameObject[] texts;
    private const int PAUSE_TEXT = 0;
    private const int GETREADY_TEXT = 1;
    private const int FIGHT_TEXT = 2;

    //paused Menu
    public GameObject pausedMenu;
    public GameObject menu;
    public GameObject[] menuOpt;
    public GameObject areYouSure;
    public GameObject[] areYouSureOpt;
    private int menuIdx = 0;
    private bool isQuit = false;
    private bool isVerticalAxisInUse = false;

    //TutorialManager
    private TutorialManager tutorialmanager; //TutorialManager script

    private AudioPlayer audioplayer;

    void Awake()
    {
        tutorialmanager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        pauseText = texts[PAUSE_TEXT].GetComponent<Text>();
        getreadyText = texts[GETREADY_TEXT].GetComponent<Image>();
        fightText = texts[FIGHT_TEXT].GetComponent<Image>();

        isGamePaused = false; //init pause state

        //init player control for each player
        playerControl = new List<PlayerControl>();
        //get PlayerControl script component from each player
        for (int i = 0; i < players.Length; i++)
        {
            PlayerControl pc = players[i].GetComponent<PlayerControl>();
            if (pc == null)
            {
                Debug.Log("PlayerControl for " + players[i].name + " not found");
            }
            else
            {
                playerControl.Add(pc);
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        audioplayer = GameObject.Find("TutorialManager").GetComponent<AudioPlayer>();

        if (tutorialmanager.tutorialLevel == TutorialManager.TUTORIAL_INACTIVE)
        {
            DontDestroyOnLoad(gameObject);

            //init score for each player
            playerKnockPoint = new int[players.Length];
            playerFallPoint = new int[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                playerKnockPoint[i] = 0;
                playerFallPoint[i] = 0;
            }

            isGameStarted = false;

            StartCoroutine(prefight());
        }
        else
        {
            isGameStarted = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //enable/disable player control
        if (isGameStarted == true)
        {
            //unpause the game, the fight begins!
            //enable control for all players
            foreach (PlayerControl pc in playerControl)
            {
                pc.enabled = true;
            }
        }
        else
        {
            //disable control for all players
            foreach (PlayerControl pc in playerControl)
            {
                pc.enabled = false;
            }
        }

        //pause game
        if ((Input.GetButtonDown("P1Pause"))
            || (Input.GetButtonDown("P2Pause")))
        {
            resetPauseMenu();
            isGamePaused = !isGamePaused;
        }
        if (isGamePaused)
        {
            //disable control for all players
            foreach (PlayerControl pc in playerControl)
            {
                pc.enabled = false;
            }
            Time.timeScale = 0f;
            //pauseText.enabled = true;
            pausedMenu.SetActive(true);

            if (!isQuit)
            {
                //handle selected menu
                if (Input.GetButtonDown("UIselect"))
                {
                    audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SELECT]);//play the sound

                    //menu selection
                    switch (menuIdx)
                    {
                        case 0:
                            {
                                resetPauseMenu();
                                isGamePaused = !isGamePaused;
                                break;
                            }
                        case 1:
                            {
                                Debug.Log("Settings screen");
                                break;
                            }
                        case 2:
                            {
                                Debug.Log("Movement screen");
                                break;
                            }
                        case 3:
                            {
                                isQuit = true;
                                menu.SetActive(false);
                                areYouSure.SetActive(true);
                                menuIdx = 0;
                                areYouSureOpt[0].SetActive(true);
                                areYouSureOpt[1].SetActive(false);
                                break;
                            }
                    }
                }
                if (Input.GetAxisRaw("UIupdown") == 0)
                {
                    isVerticalAxisInUse = false;
                }
                if (Input.GetAxisRaw("UIupdown") != 0)
                {
                    if (!isVerticalAxisInUse)
                    {
                        if (Input.GetAxisRaw("UIupdown") < 0)
                        {
                            if (menuIdx == 3)
                            {
                                menuIdx = 0;
                            }
                            else
                            {
                                menuIdx++;
                            }

                            for (int i = 0; i < 4; i++)
                            {
                                if (i == menuIdx)
                                {
                                    menuOpt[i].SetActive(true);
                                }
                                else
                                {
                                    menuOpt[i].SetActive(false);
                                }
                            }

                            audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SWITCH]);//play the sound
                        }
                        else if (Input.GetAxisRaw("UIupdown") > 0)
                        {
                            if (menuIdx == 0)
                            {
                                menuIdx = 3;
                            }
                            else
                            {
                                menuIdx--;
                            }

                            for (int i = 0; i < 4; i++)
                            {
                                if (i == menuIdx)
                                {
                                    menuOpt[i].SetActive(true);
                                }
                                else
                                {
                                    menuOpt[i].SetActive(false);
                                }
                            }

                            audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SWITCH]);//play the sound
                        }
                    }
                    isVerticalAxisInUse = true;
                }
            }
            else
            {
                //handle selected menu
                if (Input.GetButtonDown("UIselect"))
                {
                    audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SELECT]);//play the sound

                    //menu selection
                    switch (menuIdx)
                    {
                        case 0:
                            {
                                Application.LoadLevel("MainMenu");
                                break;
                            }
                        case 1:
                            {
                                isQuit = false;
                                menu.SetActive(true);
                                areYouSure.SetActive(false);
                                menuIdx = 3;
                                for (int i = 0; i < 4; i++)
                                {
                                    if (i == menuIdx)
                                    {
                                        menuOpt[i].SetActive(true);
                                    }
                                    else
                                    {
                                        menuOpt[i].SetActive(false);
                                    }
                                }
                                break;
                            }
                    }
                }
                if (Input.GetAxisRaw("UIupdown") == 0)
                {
                    isVerticalAxisInUse = false;
                }
                if (Input.GetAxisRaw("UIupdown") != 0)
                {
                    if (!isVerticalAxisInUse)
                    {
                        if (Input.GetAxisRaw("UIupdown") < 0)
                        {
                            if (menuIdx == 1)
                            {
                                menuIdx = 0;
                            }
                            else
                            {
                                menuIdx++;
                            }

                            for (int i = 0; i < 2; i++)
                            {
                                if (i == menuIdx)
                                {
                                    areYouSureOpt[i].SetActive(true);
                                }
                                else
                                {
                                    areYouSureOpt[i].SetActive(false);
                                }
                            }

                            audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SWITCH]);//play the sound
                        }
                        else if (Input.GetAxisRaw("UIupdown") > 0)
                        {
                            if (menuIdx == 0)
                            {
                                menuIdx = 1;
                            }
                            else
                            {
                                menuIdx--;
                            }

                            for (int i = 0; i < 2; i++)
                            {
                                if (i == menuIdx)
                                {
                                    areYouSureOpt[i].SetActive(true);
                                }
                                else
                                {
                                    areYouSureOpt[i].SetActive(false);
                                }
                            }

                            audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SWITCH]);//play the sound
                        }
                    }
                    isVerticalAxisInUse = true;
                }
            }
        }
        else
        {
            //enable control for all players
            foreach (PlayerControl pc in playerControl)
            {
                pc.enabled = true;
            }
            Time.timeScale = 1.0f;
            //pauseText.enabled = false;
            pausedMenu.SetActive(false);
        }

        //for gameplay scene only
        if ((tutorialmanager.tutorialLevel == TutorialManager.TUTORIAL_INACTIVE) && (isGameStarted))
        {
            //reduce gameTime, giving countdown efffect
            if (gameTime > 0)
            {
                gameTime -= Time.deltaTime;
            }
            else if (gameTime <= 0)
            {
                Application.LoadLevel("resultScreen");
            }
        }
    }

    void resetPauseMenu()
    {
        //reset paused menu
        if (!isGamePaused)
        {
            menuIdx = 0; isQuit = false;
            menu.SetActive(true);
            areYouSure.SetActive(false);
            areYouSureOpt[0].SetActive(true);
            areYouSureOpt[1].SetActive(false);
            for (int i = 0; i < 4; i++)
            {
                if (i == menuIdx)
                {
                    menuOpt[i].SetActive(true);
                }
                else
                {
                    menuOpt[i].SetActive(false);
                }
            }
        }
    }

    IEnumerator prefight()
    {
        pauseText.enabled = false;

        //show the "get ready" text
        getreadyText.enabled = true;
        fightText.enabled = false;
        yield return new WaitForSeconds(2.0f);
        //show the "fight" text
        getreadyText.enabled = false;
        fightText.enabled = true;
        yield return new WaitForSeconds(1.0f);
        fightText.enabled = false;

        isGameStarted = true;

        yield return null;
    }
}
