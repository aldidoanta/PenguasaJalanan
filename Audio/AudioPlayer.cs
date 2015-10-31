using UnityEngine;
using System.Collections;

//lists all SFX used in the game
public class AudioPlayer : MonoBehaviour
{
    public AudioClip[] musics;
    public AudioClip[] sounds;

    public int MUSIC_MAINMENU = 0;
    public int MUSIC_GAMEPLAY = 1;

    public int UI_SELECT = 0;
    public int UI_BACK = 1;
    public int UI_SWITCH = 2;
    public int ATTACK = 3;
    public int HIT = 4;
    public int WALK = 5;
    public int JUMP = 6;
    public int FALLPLATFORM = 7;
    public int POWERUP = 8;
    public int POWERUPDOWN = 9;
    public int EXPLOSION = 10;
    public int THUNDER = 11;
    public int RAIN = 12;

    //two audio sources, one for music and one for sound
    public AudioSource[] audiosources;
    public int AUDIOSOURCE_MUSIC = 0; //index for music audio source 
    public int AUDIOSOURCE_SOUND = 1; //index for sound audio source 

    // Use this for initialization
    void Start()
    {
        playMusic(musics[MUSIC_MAINMENU]);
    }

    // Update is called once per frame
    void Update()
    {
        /*MUSIC VOLUME HANDLING*/
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            musicVolumeDown();
        }

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            musicVolumeUp();
        }

        /*SOUND VOLUME HANDLING*/
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            soundVolumeDown();
        }

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            soundVolumeUp();
        }

    }

    public void musicVolumeUp()
    {
        if (audiosources[AUDIOSOURCE_MUSIC].volume < 1.0f)
        {
            audiosources[AUDIOSOURCE_MUSIC].volume += 0.1f;
        }
    }

    public void musicVolumeDown()
    {
        if (audiosources[AUDIOSOURCE_MUSIC].volume > 0f)
        {
            audiosources[AUDIOSOURCE_MUSIC].volume -= 0.1f;
        }
    }

    public void soundVolumeUp()
    {
        if (audiosources[AUDIOSOURCE_SOUND].volume < 1.0f)
        {
            audiosources[AUDIOSOURCE_SOUND].volume += 0.1f;
        }
    }

    public void soundVolumeDown()
    {
        if (audiosources[AUDIOSOURCE_SOUND].volume > 0f)
        {
            audiosources[AUDIOSOURCE_SOUND].volume -= 0.1f;
        }
    }

    //play music clip
    public void playMusic(AudioClip clip)
    {
        audiosources[AUDIOSOURCE_MUSIC].clip = clip;
        audiosources[AUDIOSOURCE_MUSIC].Play();
        //we assume that this audiosource loop value is true, because we have set it in the editor
    }

    //play soundeffect clip
    public void playSound(AudioClip clip)
    {
        audiosources[AUDIOSOURCE_SOUND].PlayOneShot(clip);
    }

    //handles which music clip will be played, depending on current loaded scene
    void OnLevelWasLoaded(int level)
    {
        switch (Application.loadedLevelName)
        {
            case "MainMenu":
            {
                playMusic(musics[MUSIC_MAINMENU]);
                break;
            }
            case "level":
            {
                playMusic(musics[MUSIC_GAMEPLAY]);
                break;
            }
            default:
            {
                audiosources[AUDIOSOURCE_MUSIC].Stop();
                break;
            }

        }
    }
}
