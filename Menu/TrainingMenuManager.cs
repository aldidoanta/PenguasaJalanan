using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//manages menu interaction in a scene
public class TrainingMenuManager : MonoBehaviour
{
    public Sprite menuDefault; //sprite used for a default menu
    public Sprite menuSelected; //sprite used when a menu option is selected
    public Transform menuoptions; //transform component of MenuOptions GameObject
    private List<Transform> menuList; //list of menu options
    private int currentSelected; //current selected menu index

    private TutorialManager tutorialmanager; //TutorialManager script

    private const string TUTORIAL_SCENE_NAME = "level"; //the name of the tutorial scene

    //used to simulate Input.GetButtonDown
    private bool isVerticalAxisInUse = false;

    private AudioPlayer audioplayer;

    void Awake()
    {
        menuList = new List<Transform>();
        currentSelected = 0;

        foreach (Transform menu in menuoptions)
        {
            menuList.Add(menu); //add menuoptions' child to menuList
        }
    }

    // Use this for initialization
    void Start()
    {
        tutorialmanager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();

        audioplayer = GameObject.Find("TutorialManager").GetComponent<AudioPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        /* INPUT */

        if (Input.GetAxisRaw("UIupdown") != 0)
        {
            if (isVerticalAxisInUse == false)
            {
                if (Input.GetAxisRaw("UIupdown") > 0)
                {
                    //go up
                    if (currentSelected > 0)
                    {
                        currentSelected--;
                    }

                    audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SWITCH]);//play the sound
                }
                //negative axis value
                else if (Input.GetAxisRaw("UIupdown") < 0)
                {
                    //go down
                    if (currentSelected < menuList.Count - 1)
                    {
                        currentSelected++;
                    }

                    audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SWITCH]);//play the sound
                }

                isVerticalAxisInUse = true;
            }
        }
        if (Input.GetAxisRaw("UIupdown") == 0)
        {
            isVerticalAxisInUse = false;
        }

        if (Input.GetButtonDown("UIselect"))
        {
            audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SELECT]);//play the sound

            tutorialmanager.tutorialLevel = currentSelected; //set selected tutorial level

            //go to the specific training scene
            //Application.LoadLevel("training"+menuList[currentSelected].name);
            Application.LoadLevel(TUTORIAL_SCENE_NAME);
        }

        if (Input.GetButtonDown("UIback"))
        {
            audioplayer.playSound(audioplayer.sounds[audioplayer.UI_BACK]);//play the sound

            tutorialmanager.tutorialLevel = TutorialManager.TUTORIAL_INACTIVE; // make tutorial level inactive
            Application.LoadLevel("MainMenu");
        }

        //highlight selected menu
        foreach (Transform menu in menuList)
        {
            if (menuList.IndexOf(menu) == currentSelected)
            {
                menu.GetComponent<Image>().sprite = menuSelected;
            }
            else
            {
                menu.GetComponent<Image>().sprite = menuDefault;
            }
        }
    }
}
