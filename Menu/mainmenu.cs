using UnityEngine;
using System.Collections;

//manages menu selection in main menu scene
public class mainmenu : MonoBehaviour
{

    public menucontroller controller;

    public GameObject[] menus;
    int currentIndex = 0;

    private const string TUTORIAL_SCENE = "trainingMenu"; //name of gameplay scene

    private TutorialManager tutorialmanager; //TutorialManager script

    private AudioPlayer audioplayer; //SFX script

    //used to simulate Input.GetButtonDown
    private bool isHorizontalAxisInUse = false;

    // Use this for initialization
    void Start()
    {
        tutorialmanager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        tutorialmanager.tutorialLevel = TutorialManager.TUTORIAL_INACTIVE; //set the default level for gameplay

        audioplayer = GameObject.Find("TutorialManager").GetComponent<AudioPlayer>();
    }

    // Update is called once per frame
    void Update()
    {

        //menu selection
        if (Input.GetAxisRaw("UIleftright") != 0)
        {
            if (isHorizontalAxisInUse == false)
            {
                if (Input.GetAxisRaw("UIleftright") < 0)
                {
                    if (currentIndex > 0)
                    {
                        currentIndex--;
                    }
                    else
                    {
                        currentIndex = 3;
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == currentIndex)
                        {
                            menus[i].SetActive(true);
                        }
                        else
                        {
                            menus[i].SetActive(false);
                        }
                    }

                    audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SWITCH]);//play the sound
                }
                else if (Input.GetAxisRaw("UIleftright") > 0)
                {
                    if (currentIndex < 3)
                    {
                        currentIndex++;
                    }
                    else
                    {
                        currentIndex = 0;
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        if (i == currentIndex)
                        {
                            menus[i].SetActive(true);
                        }
                        else
                        {
                            menus[i].SetActive(false);
                        }
                    }

                    audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SWITCH]);//play the sound
                }

                isHorizontalAxisInUse = true;
            }
        }
        if (Input.GetAxisRaw("UIleftright") == 0)
        {
            isHorizontalAxisInUse = false;
        }

        //handle selected menu
        if (Input.GetButtonDown("UIselect"))
        {
            audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SELECT]);//play the sound

            //menu selection
            switch (currentIndex)
            {
                case 0:
                    {
                        controller.showCharacterScreen();
                        break;
                    }
                case 1:
                    {
                        Application.LoadLevel(TUTORIAL_SCENE);
                        break;
                    }
                case 2:
                    {
                        controller.showSettingScreen();
                        break;
                    }
                case 3:
                    {
                        controller.showBonusScreen();
                        break;
                    }
            }

        }

    }
}
