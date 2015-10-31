using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	
	public GameObject player1;
	public GameObject player2;
	public float margin = 2f; // jarak pandang maksimal dari pemain "terluar"
	public float additionalY = 2; // tambahan y untuk y rata" (bisa di pake buat efek yang banjir/kameranya lebih naik)
	float z0 = 0; // koordinat z dari pemain
	float zCam; // koordinat z kamera pas awal permainan
	Transform p1;
	Transform p2;
	float size;
	float size0;
	float xL; // koordinat kiri paling jauh dari jarak pandang terbaik
	float xR; // koordinat kanan paling jauh dari jarak pandang terbaik
	float yL; // koordinat kiri paling jauh dari jarak pandang terbaik
	float yR; // koordinat kanan paling jauh dari jarak pandang terbaik
	float wScene; // luas kamera pas awal permainan
	
	void Start () {
		//p1 = GameObject.Find("Player1").transform;
		//p2 = GameObject.Find("Player2").transform;
		p1 = player1.transform;
		p2 = player2.transform;
		calculateScreen(p1,p2);
		wScene = xR-xL;
		zCam = transform.position.z - z0;
		size = camera.orthographicSize;
		size0 = camera.orthographicSize;
	}
	
	void FixedUpdate () {
		/* DISCLAIMER
		 *
		 * Ini contoh kodingan yang cuma ada 2 pemain.
		 * Kan game kita bakal ada 2-4 pemain, jadi harusnya dicari 2 player di posisi paling ujung kanan dan ujung kiri
		 * baru kita update jarak pandang "terbaik" dari kedua player "terjauh"
		 * Jadi, tinggal nambahin fungsi nyari pemain terjauh dari 2-4 player sih harusnya :)
		 */
		calculateScreen(p1,p2); // update jarak pandang terbaik
		
		/* Translasi Koordinat X */
		float midX = (xR+xL)/2;
		if(transform.position.x > midX + 0.05 || transform.position.x < midX - 0.05) {
			if(transform.position.x > midX && transform.position.x > -0.675)
				transform.Translate(Vector3.left * 2 * Time.deltaTime);
			else if(transform.position.x < midX && transform.position.x < 0.675)
				transform.Translate(Vector3.right * 2 * Time.deltaTime);
		}
		
		/* Translasi Koordinat Y */
		float midY = ((yR+yL)/2) + additionalY;
		if(transform.position.y > midY + 0.05 || transform.position.y < midY - 0.05) {
			if(transform.position.y > midY && transform.position.y > -1.8)
				transform.Translate(Vector3.down * 2 * Time.deltaTime);
			else if(transform.position.y < midY && transform.position.y < 1.25)
				transform.Translate(Vector3.up * 2 * Time.deltaTime);
		}
		
		/* Translasi Koordinat Z */
		float distance = xR-xL;
		float bestView = zCam*distance/wScene+z0;
		if(distance > wScene) {
			if(transform.position.z > bestView + 0.05 || transform.position.z < bestView - 0.05) {
				if(transform.position.z > bestView)
					transform.Translate(Vector3.back * 2 * Time.deltaTime);
				else if(transform.position.z < bestView)
					transform.Translate(Vector3.forward * 2 * Time.deltaTime);

                if (size < 5.8f) {
                    size = transform.position.z / zCam * size0;
                } else {
                    size = 5.8f;
                }
				camera.orthographicSize = size;
			}
		}
	}
	
	void calculateScreen(Transform p1, Transform p2) {
		if(p1.position.x < p2.position.x) {
			xL = p1.position.x - margin;
			xR = p2.position.x + margin;
		} else {
			xR = p1.position.x - margin;
			xL = p2.position.x + margin;
		}
		
		if(p1.position.y < p2.position.y) {
			yL = p1.position.y - margin;
			yR = p2.position.y + margin;
		} else {
			yR = p1.position.y - margin;
			yL = p2.position.y + margin;
		}
	}
}
