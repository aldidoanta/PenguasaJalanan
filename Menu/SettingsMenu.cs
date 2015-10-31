using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SettingsMenu : MonoBehaviour {

    public Transform settingSelection;
    public AudioPlayer audioplayer;

    private List<Transform> settingList;
    private int currentSelected;

    private bool isVerticalAxisInUse;
    private bool isHorizontalAxisInUse;

    public const int OPTIONS_MUSIC = 0;
    public const int OPTIONS_SOUND = 1;

	// Use this for initialization
	void Start () {
        settingList = new List<Transform>();
        foreach (Transform settingItem in settingSelection)
        {
            settingList.Add(settingItem);
        }
	}
	
	// Update is called once per frame
	void Update () {

        /*INPUT HANDLING*/

        //settings item selection
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
                    if (currentSelected < settingList.Count - 1)
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

        //set value for each settings item
        if (Input.GetAxisRaw("UIleftright") != 0)
        {
            if (isHorizontalAxisInUse == false)
            {
                if (Input.GetAxisRaw("UIleftright") > 0)
                {
                    //go right
                    switch (currentSelected)
                    {
                        case OPTIONS_MUSIC:
                        {
                            audioplayer.musicVolumeUp();
                            break;
                        }
                        case OPTIONS_SOUND:
                        {
                            audioplayer.soundVolumeUp();
                            break;
                        }
                    }

                    audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SWITCH]);//play the sound
                }
                else if (Input.GetAxisRaw("UIleftright") < 0)
                {
                    //go left
                    switch (currentSelected)
                    {
                        case OPTIONS_MUSIC:
                        {
                            audioplayer.musicVolumeDown();
                            break;
                        }
                        case OPTIONS_SOUND:
                        {
                            audioplayer.soundVolumeDown();
                            break;
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

        /*END OF INPUT HANDLING*/

        //highlight selected menu
        foreach (Transform settingItem in settingList)
        {
            if (settingList.IndexOf(settingItem) == currentSelected)
            {
                settingItem.Find("SelectionPanel").gameObject.SetActive(true);
            }
            else
            {
                settingItem.Find("SelectionPanel").gameObject.SetActive(false);
            }
        }

        //change settings item visual value
        settingList[OPTIONS_MUSIC].Find("ValueText").GetComponent<Text>().text = (Mathf.Round(audioplayer.audiosources[audioplayer.AUDIOSOURCE_MUSIC].volume * 10)).ToString();
        settingList[OPTIONS_SOUND].Find("ValueText").GetComponent<Text>().text = (Mathf.Round(audioplayer.audiosources[audioplayer.AUDIOSOURCE_SOUND].volume * 10)).ToString();

	}
}
