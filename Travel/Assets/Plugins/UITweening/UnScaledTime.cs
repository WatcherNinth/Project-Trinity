using UnityEngine;
using System.Collections;

public class UnScaledTime : MonoBehaviour
{
    static UnScaledTime Instance;

    float RealTime = 0f;
    float RealDeltaTime = 0f;

    static public float time
    {
        get
        {
            if (!Application.isPlaying) return Time.realtimeSinceStartup;
            if (Instance == null) Spawn();
            if (Instance == null) Instance = new UnScaledTime();
            return Instance.RealTime;
        }
    }

    static public float deltaTime
    {
        get
        {
            if (!Application.isPlaying) return 0f;
            if (Instance == null) Spawn();
            if (Instance == null) Instance = new UnScaledTime();
            return Instance.RealDeltaTime;
        }
    }

    static void Spawn()
    {
        GameObject go = new GameObject("_UnScaledTime");
        DontDestroyOnLoad(go);
        Instance = go.AddComponent<UnScaledTime>();
        if (Instance != null) 
        {
            Instance.RealTime = Time.realtimeSinceStartup;
        }
    }

    void Update()
    {
        float rt = Time.realtimeSinceStartup;
        RealDeltaTime = Mathf.Clamp01(rt - RealTime);
        RealTime = rt;
    }
}
