using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    // Instance
    private static EventSystem instance = null;

    void Awake()
    {
        // only one EventSystem
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // if another EventSystem, destroy
        else
        {
            Destroy(gameObject);
        }

        /// set resolution
        Screen.SetResolution(Screen.width, Screen.width * 19 / 9, true);
    }

    // other classes call instance
    public static EventSystem Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
}
