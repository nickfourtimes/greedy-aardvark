using UnityEngine;


public class SneerControl : MonoBehaviour {

	#region Constants

	// Cyril's movement
	private const float HORIZ_DISPLACEMENT = 2.0f;
	private const float HORIZ_MAX = 6.0f;
	private const float HORIZ_MIN = -6.0f;
	private const float VERT_DISPLACEMENT = 0.5f;
	private const float VERT_MAX = 5.0f;
	private const float VERT_MIN = -4.0f;

	// money bag stuff
	private const float MONEYBAG_Y = -5.0f;

	#endregion


	#region Parameters

	// prefabs
	public GameObject pfbMoneyBag;

	// sounds
	public AudioClip bleep;
	public AudioClip[] chomp;

	#endregion


	#region Data members

	private int _numEvergreensCollected;
	private bool _inputEnabled = true;

	public int numMoves { get; set; }

	#endregion


	#region Helpers

	private void CheckInput() {
		if (!_inputEnabled) {
			return;
		}

		if (Input.GetButtonDown("left")) {
			var pos = transform.position - HORIZ_DISPLACEMENT * Vector3.right;
			if (HORIZ_MIN <= pos.x && transform.position.x != HORIZ_MIN) {
				pos.y = VERT_MAX;
				transform.position = pos;
				++numMoves;
			} else { }

		} else if (Input.GetButtonDown("right")) {
			var pos = transform.position + HORIZ_DISPLACEMENT * Vector3.right;
			if (HORIZ_MAX >= pos.x && transform.position.x != HORIZ_MAX) {
				pos.y = VERT_MAX;
				transform.position = pos;
				++numMoves;
			} else { }

		} else if (Input.GetButtonDown("up")) {
			var pos = transform.position + VERT_DISPLACEMENT * Vector3.up;
			if (VERT_MAX >= pos.y && transform.position.y != VERT_MAX) {
				transform.position = pos;
				++numMoves;
			} else { }

		} else if (Input.GetButtonDown("down")) {
			var pos = transform.position - VERT_DISPLACEMENT * Vector3.up;
			if (VERT_MIN <= pos.y && transform.position.y != VERT_MIN) {
				transform.position = pos;
				++numMoves;
			} else { }
		}
	}


	private void CheckItems() {
		if (Physics.Raycast(transform.position, new Vector3(0, 0, 1), out var hit)) {
			if ("pfbEvergreenTree(Clone)" == hit.collider.gameObject.name) {
				if (0.01f >= Mathf.Abs(transform.position.y - hit.collider.transform.position.y)) {
					CollectEvergreen(hit.collider.gameObject);
				}
			} else if ("pfbUnitShifter(Clone)" == hit.collider.gameObject.name) {
				if (0.01f >= Mathf.Abs(transform.position.y - hit.collider.transform.position.y)) {
					CollectUnitShifter(hit.collider.gameObject);
				}
			}
		}
	}


	private void CollectEvergreen(Object evergreen) {
		// destroy and play sound
		Destroy(evergreen);
		GetComponent<AudioSource>().PlayOneShot(bleep, 0.5f);

		// check for money bagzzz
		if (2 <= ++_numEvergreensCollected) {
			_numEvergreensCollected = 0;
			GetComponent<AudioSource>().PlayOneShot(chomp[Random.Range(0, chomp.Length)], 10.0f);

			var pos = transform.position;
			pos.y = MONEYBAG_Y;

			// keep checking until we have space for the money bag
			do {
				RaycastHit hit;
				if (Physics.Raycast(pos + new Vector3(0, 0, -5), new Vector3(0, 0, 1), out hit)) {
					pos.y -= 1.0f;
				} else {
					break;
				}
			} while (true);

			Instantiate(pfbMoneyBag, pos, Quaternion.identity);
		}

		// notify the level
		LevelMaster.instance.CollectedThing();
	}


	private void CollectUnitShifter(Object shifter) {
		// destroy and play sound
		Destroy(shifter);
		GetComponent<AudioSource>().PlayOneShot(bleep, 0.5f);

		var pos = transform.position + Vector3.right;
		transform.position = pos;
	}

	#endregion


	#region Methods

	public void EnableInput(bool enable) {
		_inputEnabled = enable;
	}

	#endregion


	#region Unity methods

	private void Update() {
		CheckInput();
		CheckItems();
	}


	private void OnCollisionEnter(Collision collisionInfo) { }

	#endregion
}
