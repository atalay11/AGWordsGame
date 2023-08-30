using UnityEngine;

// <> denotes this is a generic class
public class GenericSingleton<T> : MonoBehaviour where T : Component
{
    [SerializeField] private bool persistAcrossScenes;
    
    // create a private reference to T instance
    private static T instance;

    public static T Instance
    {
        get
        {
            // if instance is null
            if (instance == null)
            {
                // find the generic instance
                instance = FindObjectOfType<T>();

                // if it's null again create a new object
                // and attach the generic instance
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    instance = obj.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        // create the instance
        if (instance == null)
        {
            instance = this as T;
            if (persistAcrossScenes)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }

        AwakeImpl();
    }

    protected virtual void AwakeImpl() {}
}