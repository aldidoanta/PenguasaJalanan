using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Player's knocback score display
public class HUDPlayerKnockback : MonoBehaviour
{

    public int playerNumber; //player number of this instance
    //assume max decimal place is 3 digits (hundreds)
    public int decimalPos; //the decimal place (ones is 1, tens is 2, hundreds is 3)

    private Image digitImage;
    private Sprite[] numberSprite;
    private Player player;
    private float playerKnockbackPoint;
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
            Debug.Log("sprites not found");
        }

        //find player script
        player = GameObject.Find("Player" + playerNumber.ToString()).GetComponent<Player>();
        if (player == null)
        {
            Debug.Log("Player" + playerNumber.ToString() + " not found");
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //get current player knockback point
        playerKnockbackPoint = player.knockbackPoint;

        //set digit displayed based on the decimal place
        switch (decimalPos)
        {
            case 1: //ones
            {
                digitDisplayed = (int)playerKnockbackPoint % 10;
                break;
            }
            case 2: //tens
            {
                digitDisplayed = (int)(playerKnockbackPoint / 10) % 10;
                break;
            }
            case 3: //hundreds
            {
                digitDisplayed = (int)(playerKnockbackPoint / 100);
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
