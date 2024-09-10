using UnityEngine;

public class DontDestroySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static bool _applicationQuit = false;
    public static T Instance
    {
        get
        {
            if(_applicationQuit)
            {
                return null;
            }

            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name, typeof(T));
                    instance = obj.GetComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        if (transform.parent == null)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    protected virtual void OnApplicationQuit()
    {
        _applicationQuit = true;
    }
}
