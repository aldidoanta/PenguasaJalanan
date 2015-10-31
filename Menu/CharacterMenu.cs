using UnityEngine;
using System.Collections;

public class CharacterMenu : MonoBehaviour {
	
	private const string GAMEPLAY_SCENE = "level"; //name of gameplay scene
	public menucontroller controller;

    private AudioPlayer audioplayer; //SFX script

	// Use this for initialization
	void Start () {
        audioplayer = GameObject.Find("TutorialManager").GetComponent<AudioPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
		if((Input.GetButtonDown("UIstart")) && (controller.pages[controller.CHARACTERSCREEN].activeSelf == true)) //Quick fix
		{
            audioplayer.playSound(audioplayer.sounds[audioplayer.UI_SELECT]);//play the sound
			Application.LoadLevel(GAMEPLAY_SCENE);
        }
        else if ((Input.GetButtonDown("UIback")) && (controller.pages[controller.CHARACTERSCREEN].activeSelf == true)) //Quick fix
        {
			controller.hideCharacterScreen();
		}
	}
}
