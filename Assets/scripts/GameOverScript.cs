using UnityEngine;


public class GameOverScript : MonoBehaviour {

	public AudioClip music;


	private void Start() {
		GetComponent<AudioSource>().PlayOneShot(music);
	}


	private void Update() {
		// escape
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}
}
