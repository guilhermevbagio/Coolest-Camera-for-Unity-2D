using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    // Public accessor for the instance
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>(true);

                if (instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    instance = obj.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    // Optionally, you can add OnDestroy method to reset the instance on scene change
    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}