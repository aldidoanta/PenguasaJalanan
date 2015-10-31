using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class blinky : MonoBehaviour {
	
	bool fading = true;
	byte alpha = 255;
	public bool animating = false;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(animating) {
			if(fading) {
				alpha-=7;
			} else {
				alpha+=7;
			}
			if(alpha <= 40) {
				fading = false;
			} else if(alpha >= 255) {
				fading = true;
			}
			GetComponent<Image>().color = new Color32(255, 255, 255, alpha);
		}
	}
}
