using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;


public class LevelMaster : MonoSingleton<LevelMaster> {

	public override void Init() { }


	#region Constants

	private const int NUM_COLS = 13;
	private const int NUM_ROWS = 10;
	private const int NUM_LEVELS = 10;
	private readonly int[] _minMovesPerLevel = {
		42, 43, 10, 110, 35, 23, 10, 13, 156, 1
	};

	#endregion


	#region Parameters

	[FormerlySerializedAs("Cyril")]
	public SneerControl cyril;
	public GameObject pfbEvergreenTree;
	public GameObject pfbUnitShifter;
	public GameObject txtComplete;
	public GameObject txtMoves;

	#endregion


	#region Data members

	private int _numThingsCollected;
	private int _currentLevel;
	private Vector3 _cyrilStartPos;

	private readonly int[,] _level1 = {
		{-1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 1, 0, 0},
		{0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
		{1, 0, 1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
		{1, 0, 1, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
	};

	private readonly int[,] _level2 = {
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 1},
		{0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0},
		{1, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 1, 0, 0, 0, 1, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
	};

	private readonly int[,] _level3 = {
		{-1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
	};

	private readonly int[,] _level4 = {
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
		{0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
		{0, 1, 0, 1, 1, 1, 1, 1, 1, 1, 0, 1, 0},
		{0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0},
		{0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
	};

	private readonly int[,] _level5 = {
		{0, 0, 0, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
	};

	private readonly int[,] _level6 = {
		{0, 0, 0, 0, 2, 0, 2, 0, 2, 0, 0, 2, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
	};

	private readonly int[,] _level7 = {
		{0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 0, 0},
		{-1, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
	};

	private readonly int[,] _level8 = {
		{-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{2, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{2, 2, 2, 2, 1, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2},
	};

	private readonly int[,] _level9 = {
		{-1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{1, 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 0},
		{1, 0, 0, 1, 0, 1, 1, 0, 1, 1, 0, 1, 0},
		{1, 0, 0, 1, 0, 1, 1, 1, 1, 0, 1, 0, 0},
		{1, 0, 0, 1, 0, 1, 0, 0, 1, 1, 0, 1, 0},
		{1, 1, 1, 1, 1, 0, 0, 0, 1, 0, 1, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 2},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
		{0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
	};

	#endregion


	#region Private methods

	private void LoadLevel(int lvl) {
		//numThingsCollected = 0;

		var myLevel = new int[NUM_ROWS, NUM_COLS];
		switch (lvl) {
			case 1:
				myLevel = _level1;
				break;
			case 2:
				myLevel = _level2;
				break;
			case 3:
				myLevel = _level3;
				break;
			case 4:
				myLevel = _level4;
				break;
			case 5:
				myLevel = _level5;
				break;
			case 6:
				myLevel = _level6;
				break;
			case 7:
				myLevel = _level7;
				break;
			case 8:
				myLevel = _level8;
				break;
			case 9:
				myLevel = _level9;
				break;
			case 10:
				// i am a bastard
				var playerRows = Random.Range(0, NUM_ROWS);
				var playerCols = Random.Range(0, NUM_COLS);
				myLevel[playerRows, playerCols] = -1;

				const float myx = -6.0f;
				const float myy = 5.0f;
				for (var r = 0; r < NUM_ROWS; ++r)
				for (var c = 0; c < NUM_COLS; ++c) {
					var pos = new Vector3(myx + c, myy - r, 0);

					if (r == playerRows && c == playerCols) {
						continue;
					}

					var foo = Random.Range(1, 4);
					switch (foo) {
						case 1:
							++_numThingsCollected;
							Instantiate(pfbEvergreenTree, pos, Quaternion.identity);
							break;
						case 2:
							Instantiate(pfbUnitShifter, pos, Quaternion.identity);
							break;
					}
				}
				break;
		} // end switch(lvl)

		// actually load the level
		const float baseX = -6.0f;
		const float baseY = 5.0f;

		// fuuuuuuck spaghetti
		if (10 != lvl) {
			_numThingsCollected = 0;
		}

		for (var r = 0; r < NUM_ROWS; ++r)
		for (var c = 0; c < NUM_COLS; ++c) {
			var pos = new Vector3(baseX + c, baseY - r, 0);
			switch (myLevel[r, c]) {
				case 1:
					++_numThingsCollected;
					Instantiate(pfbEvergreenTree, pos, Quaternion.identity);
					break;
				case 2:
					Instantiate(pfbUnitShifter, pos, Quaternion.identity);
					break;
				case -1:
					_cyrilStartPos = pos;
					_cyrilStartPos.z = -1;
					cyril.transform.position = _cyrilStartPos;
					break;
			}
		}
	}


	private IEnumerator FinishLevel() {
		cyril.EnableInput(false);
		cyril.GetComponent<Renderer>().enabled = false;

		// tell them what they've won, James!
		var mesh = txtComplete.GetComponent<TextMesh>();
		mesh.text = "FINISH";
		txtComplete.active = true;

		// look at those moves!
		mesh = txtMoves.GetComponent<TextMesh>();
		mesh.text = "Moves: " + cyril.numMoves + " of " + _minMovesPerLevel[_currentLevel - 1];
		txtMoves.active = true;

		// remove all fuck fucks
		yield return new WaitForSeconds(0.5f);
		foreach (var obj in GameObject.FindGameObjectsWithTag("UnitShifter")) {
			Destroy(obj);
		}

		// remove all money bags
		yield return new WaitForSeconds(0.5f);
		foreach (var obj in GameObject.FindGameObjectsWithTag("MoneyBag")) {
			Destroy(obj);
			yield return new WaitForSeconds(0.25f);
		}

		yield return new WaitForSeconds(1.0f);

		// hide text again
		txtComplete.active = false;
		txtMoves.active = false;

		yield return StartCoroutine(NextLevel());
		cyril.EnableInput(true);
		cyril.GetComponent<Renderer>().enabled = true;
	}


	private IEnumerator NextLevel() {
		++_currentLevel;

		cyril.GetComponent<Renderer>().enabled = false;

		// a short announcement
		var mesh = txtComplete.GetComponent<TextMesh>();
		mesh.text = "LEVEL " + _currentLevel;
		txtComplete.active = true;
		yield return new WaitForSeconds(2.0f);
		txtComplete.active = false;

		// physically load the stuff
		LoadLevel(_currentLevel);

		// put the player back into the game
		cyril.GetComponent<Renderer>().enabled = true;
		cyril.numMoves = 0;
	}


	private void ResetEverything() {
		// remove all trees
		foreach (var obj in GameObject.FindGameObjectsWithTag("EvergreenTree")) {
			Destroy(obj);
		}

		// remove all money bags
		foreach (var obj in GameObject.FindGameObjectsWithTag("MoneyBag")) {
			Destroy(obj);
		}

		// remove all fuck fucks
		foreach (var obj in GameObject.FindGameObjectsWithTag("UnitShifter")) {
			Destroy(obj);
		}

		// reset
		_currentLevel = 0;
		StartCoroutine(NextLevel());
	}

	#endregion


	#region Methods

	public void CollectedThing() {
		--_numThingsCollected;
		_numThingsCollected = Mathf.Max(_numThingsCollected, 0);

		if (_numThingsCollected == 0) {
			if (NUM_LEVELS > _currentLevel) {
				StartCoroutine(FinishLevel());
			} else {
				Application.LoadLevel(2);
			}
		}
	}

	#endregion


	#region Unity methods

	private void Start() {
		// defaults
		_currentLevel = 0;

		// hide text
		txtComplete.active = false;
		txtMoves.active = false;

		// start
		StartCoroutine(NextLevel());
	}


	private void Update() {
		// check inputs
		if (Input.GetKeyDown(KeyCode.R)) {
			ResetEverything();
		}

		// escape
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
	}

	#endregion
}
