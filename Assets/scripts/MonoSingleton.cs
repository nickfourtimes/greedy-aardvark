using UnityEngine;


public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {
	private static T _mInstance;
	public static T instance {
		get {
			// Instance required for the first time, we look for it
			if (_mInstance == null) {
				_mInstance = FindObjectOfType(typeof(T)) as T;

				// Object not found, we create a temporary one
				if (_mInstance == null) {
					Debug.LogWarning($"No instance of {typeof(T)}, a temporary one is created.");
					_mInstance = new GameObject($"Temp Instance of {typeof(T)}", typeof(T)).GetComponent<T>();

					// Problem during the creation, this should not happen
					if (_mInstance == null) {
						Debug.LogError($"Problem during the creation of {typeof(T)}");
					}
				}
				_mInstance.Init();
			}
			return _mInstance;
		}
	}


	// If no other monobehaviour request the instance in an awake function
	// executing before this one, no need to search the object.
	private void Awake() {
		if (_mInstance == null) {
			_mInstance = this as T;
			_mInstance.Init();
		}
	}

	// This function is called when the instance is used the first time
	// Put all the initializations you need here, as you would do in Awake
	public virtual void Init() { }


	// Make sure the instance isn't referenced anymore when the user quit, just in case.
	private void OnApplicationQuit() {
		_mInstance = null;
	}
}
