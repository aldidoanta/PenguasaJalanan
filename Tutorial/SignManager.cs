using UnityEngine;
using System.Collections;

//Manages sign behavior shown in training mode
public class SignManager : MonoBehaviour
{

    void Awake()
    {
        
    }

    // Use this for initialization
    void Start()
    {
        //init with a transparent panel
        foreach (Transform child in transform) //get child Transform
        {
            child.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //manages behavior when the player passes the sign
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.layer == LayerMask.NameToLayer("Player"))
            || (other.gameObject.layer == LayerMask.NameToLayer("PlayerPhased")))
        {
            if (other.GetType() == typeof(BoxCollider2D)) //only detect the body part of the player sprite
            {
                foreach (Transform child in transform)
                {
                    StartCoroutine(fadeIn(child));
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if ((other.gameObject.layer == LayerMask.NameToLayer("Player"))
            || (other.gameObject.layer == LayerMask.NameToLayer("PlayerPhased")))
        {
            if (other.GetType() == typeof(BoxCollider2D)) //only detect the body part of the player sprite
            {
                foreach (Transform child in transform)
                {
                    StartCoroutine(fadeOut(child));
                }
            }
        }
    }

    IEnumerator fadeIn(Transform trs)
    {
        //fade the icon in
        for (float f = 0.0f; f <= 0.9f; f += 0.1f)
        {
            trs.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, f);
            yield return null;
        }
    }

    IEnumerator fadeOut(Transform trs)
    {
        //fade the icon out
        for (float f = 1.0f; f >= -0.1f; f -= 0.1f)
        {
            trs.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, f);
            yield return null;
        }
    }
}
