using UnityEngine;
using System.Collections;

//Handles player control
public class PlayerControl : MonoBehaviour
{

    private Player player; //the Player script component
    private int playerNumber; //the player number

    //TutorialManager
    private TutorialManager tutorialmanager; //TutorialManager script

    void Awake()
    {
        tutorialmanager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();

        player = transform.GetComponent<Player>();
        if (player == null)
        {
            Debug.Log("Player script not found in GameObject " + transform.name);
        }
        else
        {
            //set player number
            playerNumber = player.playerNumber;
        }
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /* INPUT */
        //horizontal axis for left and right movement
        player.inputX = Input.GetAxis("P" + playerNumber.ToString() + "Horizontal");

        //jumping movement
        if (Input.GetButtonDown("P" + playerNumber.ToString() + "Jump")
            && (Input.GetAxisRaw("P" + playerNumber.ToString() + "Vertical") >= 0))
        {
            //jump conditions
            if (player.grounded && player.rigidbody2D.velocity.y == 0 && player.isJumping == false)
            {
                StartCoroutine(player.jump(player.jumpprepAnim));
            }
        }

        //falling from platform movement
        if ((Input.GetAxisRaw("P" + playerNumber.ToString() + "Vertical") < 0)
            && (Input.GetButtonDown("P" + playerNumber.ToString() + "Jump")))
        {
            //fall from platform
            if (player.grounded && player.rigidbody2D.velocity.y == 0)
            {
                StartCoroutine(player.fall());
            }
        }

        if (tutorialmanager.tutorialLevel != TutorialManager.TUTORIAL_MOVEMENT) //disable attack control if current level is Movement Tutorial
        {
            //regular attack (square/kotak button)
            if (Input.GetButtonDown("P" + playerNumber.ToString() + "Attack1"))
            {

                if (player.attackRemaining > 0)
                {
                    player.attackRemaining--; //reduce attackRemaining
                    player.playerAnim.SetBool("isAttacking", true);
                }
            }
            //strong attack (triangle/segitiga button)
            if (Input.GetButtonDown("P" + playerNumber.ToString() + "Attack2"))
            {

                if (player.attackRemaining > 0)
                {
                    player.attackRemaining = 0; //reduce attackRemaining to 0
                    player.playerAnim.SetBool("isAttacking", true);
                }
            }
        }
    }
}
