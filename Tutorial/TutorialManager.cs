using UnityEngine;
using System.Collections;

//Manages which tutorial level to be played
public class TutorialManager : MonoBehaviour {

    //contants as identifier for each tutorial 
    public const int TUTORIAL_INACTIVE = -1;
    public const int TUTORIAL_MOVEMENT = 0;
    public const int TUTORIAL_ATTACK = 1;
    public const int TUTORIAL_ITEM = 2;

    public int tutorialLevel = TUTORIAL_INACTIVE; //stores current tutorial level

    //the GameObjects
    GameObject sign;
    GameObject tutorialitem;
    GameObject knockbackmeter;
    GameObject timer;
    GameObject getreadytext;
    GameObject fighttext;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject); //destroy itself if the GameObject is a copy
        }
    }

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {

	}

    //triggered when a new scene was loaded
    void OnLevelWasLoaded(int level)
    {
        //get reference to GameObject in the scene
        sign = GameObject.Find("Sign");
        tutorialitem = GameObject.Find("TutorialItem");
        knockbackmeter = GameObject.Find("KnockbackMeter");
        timer = GameObject.Find("Timer");
        getreadytext = GameObject.Find("GetReadyText");
        fighttext = GameObject.Find("FightText");

        if(level == 1) //level number refers to scene index in Unity's Build Settings
        {
            if (tutorialLevel == TUTORIAL_INACTIVE)
            {
                sign.SetActive(false); //hide the Sign GameObject
                tutorialitem.SetActive(false); //hide items used for item tutorial level

                //enable some GameObject which were disabled during tutorial level
                knockbackmeter.SetActive(true);
                timer.SetActive(true);
                getreadytext.SetActive(true);
                fighttext.SetActive(true);
            }
            else
            {
                sign.SetActive(true); //show the Sign GameObject
                //disable tutorial panel contents
                foreach(GameObject gameobj in GameObject.FindGameObjectsWithTag("TutorialPanelContent"))
                {
                    gameobj.GetComponent<SpriteRenderer>().enabled = false;
                }

                //set GameObject for tutorial level
                knockbackmeter.SetActive(true); //enabled for all tutorial level except movement tutorial
                tutorialitem.SetActive(false); //disabled for all tutorial level except item tutorial
                timer.SetActive(false);
                getreadytext.SetActive(false);
                fighttext.SetActive(false);

                switch(tutorialLevel)
                {
                    case TUTORIAL_MOVEMENT:
                    {
                        knockbackmeter.SetActive(false);

                        //enable appropriate sprites for tutorial panel content
                        foreach (GameObject gameobj in GameObject.FindGameObjectsWithTag("TutorialPanelContent"))
                        {
                            if ((gameobj.name == "msgMovement")
                                || (gameobj.name == "button_dpad"))
                            {
                                gameobj.GetComponent<SpriteRenderer>().enabled = true;
                            }
                        }
                        break;
                    }
                    case TUTORIAL_ATTACK:
                    {
                        foreach (GameObject gameobj in GameObject.FindGameObjectsWithTag("TutorialPanelContent"))
                        {
                            if ((gameobj.name == "msgAttack")
                                || (gameobj.name == "button_square")
                                || (gameobj.name == "button_triangle"))
                            {
                                gameobj.GetComponent<SpriteRenderer>().enabled = true;
                            }
                        }
                        break;
                    }
                    case TUTORIAL_ITEM:
                    {
                        foreach (GameObject gameobj in GameObject.FindGameObjectsWithTag("TutorialPanelContent"))
                        {
                            if ((gameobj.name == "msgItem")
                                || (gameobj.name == "button_square")
                                || (gameobj.name == "button_triangle"))
                            {
                                gameobj.GetComponent<SpriteRenderer>().enabled = true;
                            }
                        }
                        tutorialitem.SetActive(true);
                        break;
                    }
                }
            }
        }
    }
}
