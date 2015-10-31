using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//manages thing related to the stage environment
public class StageEnvironment : MonoBehaviour {

    //icon shown after item activation
    private Sprite[] itemIconArray;
    public GameObject itemIconPrefab; //ItemIcon prefab

    //flood situation
    [HideInInspector]public bool isFlood = false;
    private float floodTime = 8.0f; //flood duration, in second
    private float floodLowerTime = 2.0f; //time to lower the flood, in second
    private Image lightningFlash; //white screen used for lightning flash effect
    private SpriteRenderer backgroundSprite; //sprite used as background
    private ParticleSystem rainParticle; //rain particle
    private Transform water; //flood water GameObject
    private Transform floodTopPoint; //maximum height of flood
    private Transform fallingPointLeft; //the falling point for affected GameObjects
    private Transform fallingPointRight;

    //array of GameObject in the game environment
    public GameObject[] envObjects;
    private const int LIGHTNINGFLASH = 0;
    private const int BACKGROUNDSPRITE = 1;
    private const int RAINPARTICLE = 2;
    private const int WATER = 3;
    private const int FLOOD_TOPPOINT = 4;
    private const int FALLINGPOINT_LEFT = 5;
    private const int FALLINGPOINT_RIGHT = 6;

    private AudioPlayer audioplayer;

    void Awake()
    {

        //itemIcon init
        itemIconArray = Resources.LoadAll<Sprite>("ItemIcon");
        if (itemIconArray.Length == 0)
        {
            Debug.Log("itemIcon sprites not found");
        }
        

        //flood init
        lightningFlash = envObjects[LIGHTNINGFLASH].GetComponent<Image>();
        backgroundSprite = envObjects[BACKGROUNDSPRITE].GetComponent<SpriteRenderer>();
        rainParticle = envObjects[RAINPARTICLE].GetComponent<ParticleSystem>();
        rainParticle.enableEmission = false;
        water = envObjects[WATER].transform;
        floodTopPoint = envObjects[FLOOD_TOPPOINT].transform;
        fallingPointLeft = envObjects[FALLINGPOINT_LEFT].transform;
        fallingPointRight = envObjects[FALLINGPOINT_RIGHT].transform;
    }

	// Use this for initialization
	void Start () {

        audioplayer = GameObject.Find("TutorialManager").GetComponent<AudioPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //show icon from the recently activated item
    public IEnumerator showitemicon(int itemType, Transform itemTransform)
    {
        GameObject itemIcon = Instantiate(itemIconPrefab) as GameObject;
        itemIcon.transform.position = itemTransform.position; //set icon itemIcon position
        itemIcon.GetComponent<SpriteRenderer>().sprite = itemIconArray[itemType]; //set itemIcon image
        //fade the icon out
        for (float f = 1.0f; f >= -0.1f; f -= 0.01f)
        {
            itemIcon.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, f);
            yield return null;
        }
        Destroy(itemIcon);
    }

    //start flood situation
    public IEnumerator flood()
    {
        audioplayer.playSound(audioplayer.sounds[audioplayer.THUNDER]);//play the sound

        //lightning effect
        for (float f = 0; f <= 1.1f; f += 0.1f)
        {
            Color color = lightningFlash.color;
            color.a = f;
            lightningFlash.color = color;
            yield return null;
        }
        for (float f = 1.0f; f >= -0.1f; f -= 0.1f)
        {
            Color color = lightningFlash.color;
            color.a = f;
            lightningFlash.color = color;
            yield return null;
        }
        for (float f = 0; f <= 1.1f; f += 0.1f)
        {
            Color color = lightningFlash.color;
            color.a = f;
            lightningFlash.color = color;
            yield return null;
        }
        for (float f = 1.0f; f >= -0.1f; f -= 0.1f)
        {
            Color color = lightningFlash.color;
            color.a = f;
            lightningFlash.color = color;
            yield return null;
        }

        if (!isFlood)
        {
            isFlood = true; //set the flood flag to true

            Color color = new Color(1.0f, 1.0f, 1.0f); //color used for darkening the background
            Color darkestColor = new Color(0.3f, 0.3f, 0.3f);
            Color normalColor = new Color(1.0f, 1.0f, 1.0f);

            audioplayer.playSound(audioplayer.sounds[audioplayer.RAIN]);//play the sound

            //start rain
            rainParticle.enableEmission = true;
            //rise the flood water
            float currentFloodTime = floodTime;
            float yDistance = floodTopPoint.position.y - water.position.y; //y distance between the initial flood height and floodTopPoint
            while (currentFloodTime > 0)
            {
                //darken the background
                if(color.r > darkestColor.r
                    && color.g > darkestColor.g
                    && color.b > darkestColor.b)
                {
                    color.r -= 0.01f;
                    color.g -= 0.01f;
                    color.b -= 0.01f;
                    backgroundSprite.color = color;
                }

                //Water and Boundaries translation
                float yTranslation = Time.deltaTime * (yDistance / floodTime);
                water.Translate(Vector3.up * yTranslation); //move the water sprite up
                fallingPointLeft.Translate(Vector3.up * yTranslation); //move the fallingPoint, too
                fallingPointRight.Translate(Vector3.up * yTranslation);
                currentFloodTime -= Time.deltaTime;
                yield return null;
            }

            //stop rain
            rainParticle.enableEmission = false;

            //lower the flood water
            while (currentFloodTime < floodLowerTime)
            {
                //change the background color back to normal
                if (color.r < normalColor.r
                    && color.g < normalColor.g
                    && color.b < normalColor.b)
                {
                    color.r += 0.01f;
                    color.g += 0.01f;
                    color.b += 0.01f;
                    backgroundSprite.color = color;
                }

                //Water and Boundaries translation
                float yTranslation = Time.deltaTime * (yDistance / floodLowerTime);
                water.Translate(Vector3.up * -1 * yTranslation); //move the water sprite down
                fallingPointLeft.Translate(Vector3.up * -1 * yTranslation);
                fallingPointRight.Translate(Vector3.up * -1 * yTranslation);
                currentFloodTime += Time.deltaTime;
                yield return null;
            }

            isFlood = false; //set the flood flag to false
        }
        else
        {
            yield return null;
        }
    }
}
