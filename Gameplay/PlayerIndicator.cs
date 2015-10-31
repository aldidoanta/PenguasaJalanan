using UnityEngine;
using System.Collections;

//manages how player indicators should be displayed above the players
public class PlayerIndicator : MonoBehaviour
{

    public GameObject[] indicators; //array of GameObject for indicator sprites
    public GameObject[] players; //array of GameObject the player sprites
    private float yDistance; //distance between the indicator and the player

    void Awake()
    {
        if (indicators.Length != players.Length)
        {
            Debug.Log("WARNING - indicators and players array (PlayerIndicator.cs) should have the same length");
        }

    }

    // Use this for initialization
    void Start()
    {
        yDistance = Mathf.Abs(indicators[0].transform.position.y - players[0].transform.position.y); //we only take one instance to represent yDistance of the others
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < indicators.Length; i++)
        {
            indicators[i].transform.position = new Vector3(players[i].transform.position.x,
                                                           players[i].transform.position.y + yDistance,
                                                           players[i].transform.position.z);
        }
    }
}
