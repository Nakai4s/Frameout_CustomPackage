using UnityEngine;

namespace Frameout{
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    protected static T instance;

	public static T Instance {
		get {
			if (instance == null) {
				instance = (T)FindObjectOfType (typeof(T));
				
				if (instance == null) {
					Debug.LogWarning(typeof(T) + " is nothing");
				}
			}
			return instance;
		}
	}

    protected virtual void Awake(){
        if(this != Instance){
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
}
