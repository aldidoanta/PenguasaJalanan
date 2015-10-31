using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Handles item spawn and its usage
public class Item : MonoBehaviour
{
    //TODO add all types of items
    //constants for item type
    private const int ITEM_INCREASE_KNOCKBACKPOINT = 0;
    private const int ITEM_DECREASE_KNOCKBACKPOINT = 1;
    private const int ITEM_EXPLOSION = 2;
    private const int ITEM_FLOOD = 3;

    private float increaseKnockbackValue = 150.0f; //knockback value for ITEM_INCREASE_KNOCKBACKPOINT
    private float decreaseKnockbackValue = 100.0f; //knockback value for ITEM_DECREASE_KNOCKBACKPOINT

    private StageEnvironment env; //StageEnvironment component to manage change in environment appearance

    public int itemType; //stores current item type

    //respawn reference(for item tutorial scene only)
    public Transform respawnPoint;

    //TutorialManager
    private TutorialManager tutorialmanager; //TutorialManager script

    private AudioPlayer audioplayer;

    void Awake()
    {
        tutorialmanager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        env = GameObject.Find("GameplayManager").GetComponent<StageEnvironment>();
    }

    // Use this for initialization
    void Start()
    {
        audioplayer = GameObject.Find("TutorialManager").GetComponent<AudioPlayer>();

        if (tutorialmanager.tutorialLevel == TutorialManager.TUTORIAL_INACTIVE)
        {
            //assign item type
            itemType = (int)Random.Range(0, 3 + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    //activate item affect according to its itemType value
    public void activateItem(Player attackingPlayer)
    {
        //activate item effect
        StartCoroutine(env.showitemicon(itemType, this.transform));
        switch (itemType)
        {
            case ITEM_INCREASE_KNOCKBACKPOINT:
            {
                audioplayer.playSound(audioplayer.sounds[audioplayer.POWERUPDOWN]);

                attackingPlayer.knockbackPoint += increaseKnockbackValue; //item effect
                break;
            }
            case ITEM_DECREASE_KNOCKBACKPOINT:
            {
                audioplayer.playSound(audioplayer.sounds[audioplayer.POWERUP]);//play the sound

                attackingPlayer.knockbackPoint -= decreaseKnockbackValue;
                if (attackingPlayer.knockbackPoint < 0)
                {
                    attackingPlayer.knockbackPoint = 0;
                }
                break;
            }
            case ITEM_EXPLOSION:
            {
                audioplayer.playSound(audioplayer.sounds[audioplayer.EXPLOSION]);//play the sound

                //apply knockback to the attacking player
                attackingPlayer.knockbackDirection = new Vector2(-5 * Mathf.Sign(attackingPlayer.transform.localScale.x), 1);
                break;
            }
            case ITEM_FLOOD:
            {
                StartCoroutine(env.flood());
                break;
            }
            default:
            {
                break;
            }
        }


        //disable item gameobject
        gameObject.renderer.enabled = false; //because we need to wait for the flood() coroutine
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        if (tutorialmanager.tutorialLevel == TutorialManager.TUTORIAL_ITEM)
        {
            StartCoroutine(respawnItem()); //respawn item
        }
    }

    //respawn used item (for item tutorial scene only)
    public IEnumerator respawnItem()
    {
        yield return new WaitForSeconds(5.0f);
        gameObject.renderer.enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

        transform.position = respawnPoint.position; //respawn at respawnPoint's position
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero; //reset item's velocity
        transform.eulerAngles = new Vector3(0,0,0); //reset item rotation
    }
}
