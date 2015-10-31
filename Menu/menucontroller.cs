using UnityEngine;
using System.Collections;

//manages splash screen and main menu screen
public class menucontroller : MonoBehaviour
{

    public GameObject[] pages;
    public int STARTSCREEN = 0;
    public int MENUSCREEN = 1;
    public int CHARACTERSCREEN = 2;
    public int SETTINGSCREEN = 3;
    public int BONUSSCREEN = 4;

    private AudioPlayer audioplayer; //SFX script

    // Use this for initialization
    void Start()
    {
        //destroy gameplaymanager if it exists after gameplay scene
        GameObject gameplaymanager = GameObject.Find("GameplayManager");
        if (gameplaymanager != null)
        {
            Destroy(gameplaymanager);
        }

        audioplayer = GameObject.Find("TutorialManager").GetComponent<AudioPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetButtonDown("UIstart")) && (pages[STARTSCREEN].activeSelf == true)) //Quick fix
        {
            pages[STARTSCREEN].SetActive(false);
            pages[MENUSCREEN].SetActive(true);

            audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SELECT]);//play the sound
        }
        else if ((Input.GetButtonDown("UIback")) && (pages[MENUSCREEN].activeSelf == true)) //Quick fix
        {
            pages[STARTSCREEN].SetActive(true);
            pages[MENUSCREEN].SetActive(false);

            audioplayer.playSound(audioplayer.sounds[audioplayer.UI_BACK]);//play the sound
        }
        else if ((Input.GetButtonDown("UIback")) && (pages[SETTINGSCREEN].activeSelf == true)) //Quick fix
        {
            hideSettingScreen();
        }
        else if ((Input.GetButtonDown("UIback")) && (pages[BONUSSCREEN].activeSelf == true)) //Quick fix
        {
            hideBonusScreen();
        }
    }

    public void showCharacterScreen()
    {
        audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SELECT]);//play the sound

        pages[CHARACTERSCREEN].SetActive(true);
        pages[MENUSCREEN].SetActive(false);
    }

    public void hideCharacterScreen()
    {
        audioplayer.playSound(audioplayer.sounds[audioplayer.UI_BACK]);//play the sound

        pages[CHARACTERSCREEN].SetActive(false);
        pages[MENUSCREEN].SetActive(true);
    }

    public void showSettingScreen()
    {
        audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SELECT]);//play the sound

        pages[SETTINGSCREEN].SetActive(true);
        pages[MENUSCREEN].SetActive(false);
    }

    public void hideSettingScreen()
    {
        audioplayer.playSound(audioplayer.sounds[audioplayer.UI_BACK]);//play the sound

        pages[SETTINGSCREEN].SetActive(false);
        pages[MENUSCREEN].SetActive(true);
    }

    public void showBonusScreen()
    {
        audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SELECT]);//play the sound

        pages[BONUSSCREEN].SetActive(true);
        pages[MENUSCREEN].SetActive(false);
    }

    public void hideBonusScreen()
    {
        audioplayer.playSound(audioplayer.sounds[audioplayer.UI_BACK]);//play the sound

        pages[BONUSSCREEN].SetActive(false);
        pages[MENUSCREEN].SetActive(true);
    }
}
