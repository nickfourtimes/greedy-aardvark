using UnityEngine;


public class TitleScreenScript : MonoBehaviour {

	#region Data members

	private float _timeStamp;

	#endregion

	public GameObject txtComplete;
	public GameObject txtMoves;

	#region Unity methods

	private void Start() {
		_timeStamp = Time.time;

		var mesh = txtComplete.GetComponent<TextMesh>();
		mesh.text = "GREEDY\nAARDVARK";

		mesh = txtMoves.GetComponent<TextMesh>();
		mesh.text = "a game by newton64";
	}


	private void Update() {
		if (4.0f < Time.time - _timeStamp) {
			Application.LoadLevel(1);
		}

		// escape
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}

	#endregion
}
