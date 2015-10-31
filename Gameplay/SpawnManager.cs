using UnityEngine;
using System.Collections;

//Handles player fall, spawn, and respawns
public class SpawnManager : MonoBehaviour
{
    //the boundaries for horizontal linecast
    private float topY;
    private float bottomY;
    private float leftX;
    private float rightX;

    //respawn horizontal range
    private float respawnMinX;
    private float respawnMaxX;

    //linecast flag
    private RaycastHit2D fallingPlayer;
    private RaycastHit2D fallingPlayerPhased;
    private RaycastHit2D fallingItem;

    //respawn point, falling point, respawn references
    private Transform respawnPoint;
    private Transform respawnRefLeft;
    private Transform respawnRefRight;
    private Transform respawnRefLeftFlood;
    private Transform respawnRefRightFlood;
    private Transform fallingPointLeft;
    private Transform fallingPointRight;

    //item prefab
    public Transform itemPrefab;
    //item spawn time
    private float minItemSpawnTime;
    private float maxItemSpawnTime;
    private int maxItemSpawnNumber = 5;

    private GameplayManager gameplaymanager; //GameplayManager script
    private StageEnvironment stageenvironment; //StageEnvironment script

    //array of GameObject as spawn reference
    public GameObject[] spawnReferences;
    private const int RESPAWNPOINT = 0;
    private const int RESPAWNREF_LEFT = 1;
    private const int RESPAWNREF_RIGHT = 2;
    private const int RESPAWNREF_LEFT_FLOOD = 3;
    private const int RESPAWNREF_RIGHT_FLOOD = 4;
    private const int FALLINGPOINT_LEFT = 5;
    private const int FALLINGPOINT_RIGHT = 6;

    private TutorialManager tutorialmanager; //TutorialManager script

    void Awake()
    {
        //get script component of other GameObjects
        tutorialmanager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
        gameplaymanager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        stageenvironment = GameObject.Find("GameplayManager").GetComponent<StageEnvironment>();

        //get RespawnPoint, FallingPointLeft, and FallingPointRight position
        respawnPoint = spawnReferences[RESPAWNPOINT].transform;
        respawnRefLeft = spawnReferences[RESPAWNREF_LEFT].transform;  //TODO adapt with the real implementation of platform
        respawnRefRight = spawnReferences[RESPAWNREF_RIGHT].transform; //TODO adapt with the real implementation of platform
        respawnRefLeftFlood = spawnReferences[RESPAWNREF_LEFT_FLOOD].transform;
        respawnRefRightFlood = spawnReferences[RESPAWNREF_RIGHT_FLOOD].transform;
        fallingPointLeft = spawnReferences[FALLINGPOINT_LEFT].transform;
        fallingPointRight = spawnReferences[FALLINGPOINT_RIGHT].transform;

        if (fallingPointLeft.position.y != fallingPointRight.position.y)
        {
            Debug.Log("fallingPointLeft.position.y != fallingPointRight.position.y, will use value from fallingPointLeft.position.y");
        }

        //assign boundary values
        topY = respawnPoint.position.y;
        bottomY = fallingPointLeft.position.y; //since we assume that fallingPointLeft.position.y == fallingPointRight.position.y
        leftX = fallingPointLeft.position.x;
        rightX = fallingPointRight.position.x;
        respawnMinX = respawnRefLeft.position.x;
        respawnMaxX = respawnRefRight.position.x;
    }

    // Use this for initialization
    void Start()
    {
        if (tutorialmanager.tutorialLevel != TutorialManager.TUTORIAL_INACTIVE) //if tutorial mode is the current scene
        {
            //we don't spawn any items in tutorial mode
            maxItemSpawnNumber = 0;
        }

        if (tutorialmanager.tutorialLevel == TutorialManager.TUTORIAL_INACTIVE) //when in gameplay scene
        {
            //assign minimal and maximal spawn time
            minItemSpawnTime = gameplaymanager.gameTime * 0.2f;
            maxItemSpawnTime = gameplaymanager.gameTime * 0.8f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /* ITEM SPAWN INTERVAL HANDLING */
        if (maxItemSpawnNumber > 0)
        {
            StartCoroutine(spawnItem());
            maxItemSpawnNumber--;
        }

        /*RESPAWN POSITION REFERENCE*/
        if (stageenvironment.isFlood)
        {
            //limit the respawn area during flood
            respawnMinX = respawnRefLeftFlood.position.x;
            respawnMaxX = respawnRefRightFlood.position.x;
        }
        else
        {
            //set the respawn area back to normal
            respawnMinX = respawnRefLeft.position.x;
            respawnMaxX = respawnRefRight.position.x;
        }

        /* FALLING GAMEOBECT HANDLING */
        bottomY = fallingPointLeft.position.y; //update bottomY, since flood may change its value
        //check if a player is falling through the falling point
        fallingPlayer = Physics2D.Linecast(new Vector2(leftX, bottomY),
                                       new Vector2(rightX, bottomY),
                                       1 << LayerMask.NameToLayer("Player"));
        fallingPlayerPhased = Physics2D.Linecast(new Vector2(leftX, bottomY),
                                       new Vector2(rightX, bottomY),
                                       1 << LayerMask.NameToLayer("PlayerPhased"));
        //check if an item box is falling
        fallingItem = Physics2D.Linecast(new Vector2(leftX, bottomY),
                                       new Vector2(rightX, bottomY),
                                       1 << LayerMask.NameToLayer("Item"));
        if (fallingPlayer)
        {
            if (tutorialmanager.tutorialLevel == TutorialManager.TUTORIAL_INACTIVE) //we count score only in gameplay scene
            {
                //increase the player's fallPoint by 1
                int playerNumber = fallingPlayer.transform.GetComponent<Player>().playerNumber;
                gameplaymanager.playerFallPoint[playerNumber - 1]++;
            }

            //reset the player's knocbackPoint
            fallingPlayer.transform.GetComponent<Player>().knockbackPoint = 0f;

            //respawn the player around the RespawnPoint
            float randomX = Random.Range(respawnMinX, respawnMaxX);
            fallingPlayer.transform.position = new Vector3(randomX, topY, 0);
        }
        //do the same thing for fallingPlayerPhased
        else if (fallingPlayerPhased)
        {
            if (tutorialmanager.tutorialLevel == TutorialManager.TUTORIAL_INACTIVE) //we count score only in gameplay scene
            {
                //increase the player's fallPoint by 1
                int playerNumber = fallingPlayerPhased.transform.GetComponent<Player>().playerNumber;
                gameplaymanager.playerFallPoint[playerNumber - 1]++;
            }

            //reset the player's knocbackPoint
            fallingPlayerPhased.transform.GetComponent<Player>().knockbackPoint = 0f;

            //respawn the player around the RespawnPoint
            float randomX = Random.Range(respawnMinX, respawnMaxX);
            fallingPlayerPhased.transform.position = new Vector3(randomX, topY, 0);
        }

        if (fallingItem)
        {
            Item item = fallingItem.transform.gameObject.GetComponent<Item>();
            //disable item gameobject
            item.gameObject.renderer.enabled = false;
            item.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            if (tutorialmanager.tutorialLevel == TutorialManager.TUTORIAL_ITEM)
            {
                //respawn item
                StartCoroutine(item.respawnItem());
            }
        }
    }

    //spawn a random item
    IEnumerator spawnItem()
    {
        //wait for a random time between minItemSpawnTime and maxItemSpawnTime
        yield return new WaitForSeconds(Random.Range(minItemSpawnTime, maxItemSpawnTime + 0.1f));

        //spawn the item
        Transform itemInstance = Instantiate(itemPrefab) as Transform;
        //spawn the item around the spawn point
        float randomX = Random.Range(respawnMinX, respawnMaxX);
        itemInstance.position = new Vector3(randomX, topY, 0);
    }
}
