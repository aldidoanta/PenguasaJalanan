using UnityEngine;
using System.Collections;

public class RainParticle : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        //reorder the sortinglayer to the top
        particleSystem.renderer.sortingLayerName = "Environment";
    }

    // Update is called once per frame
    void Update()
    {

    }
}
