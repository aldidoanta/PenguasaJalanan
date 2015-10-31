using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//shows gameplay time countdown
public class HUDTime : MonoBehaviour
{

    public int decimalPos; //the decimal place (ones is 1, tens is 2)

    private Image digitImage;
    private Sprite[] numberSprite;
    private GameplayManager gameplaymanager;
    private int currentTime;
    private int digitDisplayed;

    void Awake()
    {
        //load Image GameObject
        digitImage = GetComponent<Image>();
        if (digitImage == null)
        {
            Debug.Log("digitImage not found");
        }

        //load number sprite
        numberSprite = Resources.LoadAll<Sprite>("Number");
        if (numberSprite.Length == 0)
        {
            Debug.Log("number sprites not found");
        }

        //find GameplayManager
        gameplaymanager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        if (gameplaymanager == null)
        {
            Debug.Log("GameplayManager not found");
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //get current time
        currentTime = Mathf.CeilToInt(gameplaymanager.gameTime); //get the ceiling number

        //set digit displayed based on the decimal place
        switch (decimalPos)
        {
            case 1: //ones
                {
                    digitDisplayed = (int)currentTime % 10;
                    break;
                }
            case 2: //tens
                {
                    digitDisplayed = (int)(currentTime / 10) % 10;
                    break;
                }
            default:
                {
                    digitDisplayed = 0; //display 0
                    break;
                }
        }
        //set sprite based on digit displayed
        Sprite digitSprite = numberSprite[digitDisplayed];
        digitImage.sprite = digitSprite;
    }
}
