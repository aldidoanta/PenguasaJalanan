using UnityEngine;
using System.Collections;

//handles attack collision
public class Attack : MonoBehaviour
{

    public int playerNumber;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //check if the attack collider hits the enemy collider
        Player enemyPlayer = other.GetComponentInParent<Player>();
        Player myPlayer = this.GetComponentInParent<Player>();
        //check if the attack collider hits the item collider
        Item item = other.GetComponentInParent<Item>();

        if(enemyPlayer != null)
        {
            if (enemyPlayer.playerNumber != this.playerNumber)
            {
                enemyPlayer.knockbackDirection = new Vector2(Mathf.Sign(myPlayer.transform.localScale.x) * 1.2f, 0.6f); //get attack direction
            }
        }
        if (item != null)
        {
            item.activateItem(myPlayer);
        }
        
    }
}
