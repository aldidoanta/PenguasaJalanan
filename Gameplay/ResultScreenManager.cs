using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//manages ResultScreen scene
public class ResultScreenManager : MonoBehaviour
{
    GameplayManager gameplaymanager;
    public int[] playerFallPoint; //player fall point, taken from a variable with the same name in GameplayManager

    public GameObject textWinner; //winner text, indicating the winner
    public GameObject textQuit; //a text instructing the user to quit the game
    public GameObject fallCounter; //player's score counter

    // Use this for initialization
    void Start()
    {
        //get information from GameplayManager
        gameplaymanager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();

        playerFallPoint = new int[gameplaymanager.players.Length];
        for (int i = 0; i < playerFallPoint.Length; i++)
        {
            playerFallPoint[i] = gameplaymanager.playerFallPoint[i];
        }

        //destroy GameplayManager, since we won't use it anymore
        Destroy(GameObject.Find("GameplayManager"));

        //"play" the scene
        StartCoroutine(playscene());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("UIback"))
        {
            Application.LoadLevel("MainMenu");
        }
    }

    //"play" the ResultScreen scene
    IEnumerator playscene()
    {
        yield return new WaitForSeconds(1.0f);

        //show score for each player
        HUDPlayerFall[] hudplayerfallArray = fallCounter.GetComponentsInChildren<HUDPlayerFall>();
        foreach (HUDPlayerFall hudplayerfall in hudplayerfallArray)
        {
            hudplayerfall.isShowScore = true;
        }

        yield return new WaitForSeconds(1.0f);

        //show the winner
        int winnerPlayer = -1;
        int winnerScore = 999; //maximum value for 3-figure score
        bool isDraw = false; //check if the match results in a draw

        //determine the winner
        for (int i = 0; i < playerFallPoint.Length; i++)
        {
            if (playerFallPoint[i] < winnerScore)
            {
                winnerPlayer = i;
                winnerScore = playerFallPoint[i];
            }
            if ((i != winnerPlayer) && (winnerScore == playerFallPoint[i]))
            {
                isDraw = true;
            }
        }
        
        if(winnerPlayer != -1)
        {
            if (!isDraw) //if the game doesn't result in a draw
            {
                //show winner text
                GameObject winnerPlayerIcon = GameObject.Find("Player" + (winnerPlayer + 1) + "Icon");
                RectTransform winnerPlayerIconTransform = winnerPlayerIcon.GetComponent<RectTransform>();
                textWinner.GetComponent<Text>().enabled = true;
                textWinner.GetComponent<RectTransform>().position = new Vector3(winnerPlayerIconTransform.position.x,
                                                                                 winnerPlayerIconTransform.position.y - 50,
                                                                                 winnerPlayerIconTransform.position.z);
            }
            else
            {
                textWinner.GetComponent<Text>().enabled = true;
                textWinner.GetComponent<Text>().text = "Draw!";
            }
        }

        yield return new WaitForSeconds(1.0f);

        //show quitText
        textQuit.GetComponent<Text>().enabled = true;
        textQuit.GetComponentInChildren<Image>().enabled = true;
    }
}
