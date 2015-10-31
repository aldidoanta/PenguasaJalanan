using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//show player's falling score
public class HUDPlayerFall : MonoBehaviour
{
    public int playerNumber;
    //assume max decimal place is 3 digits (hundreds)
    public int decimalPos; //the decimal place (ones is 1, tens is 2, hundreds is 3)

    private Image digitImage;
    private Sprite[] numberSprite;
    private ResultScreenManager resultscreenmanager;
    private float playerFallPoint;
    private int digitDisplayed;

    //flags for current scene
    [HideInInspector]public bool isShowScore; //step1 - show falling point

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
            Debug.Log("sprites not found");
        }

        //init flags
        isShowScore = false;
    }

    // Use this for initialization
    void Start()
    {
        //find gameplaymanager script
        resultscreenmanager = GameObject.Find("ResultScreenManager").GetComponent<ResultScreenManager>();
        if (resultscreenmanager == null)
        {
            Debug.Log("ResultScreenManager script not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //get current player knockback point
        playerFallPoint = resultscreenmanager.playerFallPoint[playerNumber - 1];

        //set digit displayed based on the decimal place
        switch (decimalPos)
        {
            case 1: //ones
                {
                    digitDisplayed = (int)playerFallPoint % 10;
                    break;
                }
            case 2: //tens
                {
                    digitDisplayed = (int)(playerFallPoint / 10) % 10;
                    break;
                }
            case 3: //hundreds
                {
                    digitDisplayed = (int)(playerFallPoint / 100);
                    break;
                }
            default:
                {
                    digitDisplayed = 0; //display 0
                    break;
                }
        }

        if (isShowScore)
        {
            //set sprite based on digit displayed
            Sprite digitSprite = numberSprite[digitDisplayed];
            digitImage.sprite = digitSprite;
        }
    }
}
