using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugControl : MonoBehaviour {

    public bool isOn = true;
    public bool isPlay = false;

    public GameObject cursor;
    public GameObject eventSystem;

    private InputRecord inputR;
    private NetWorkRecord netR;
    private TimeRecord timeR;
    private AndroidRecord androidR;
    private KeyBoardRecord keyR;

    private InputPlay inputP;
    private NetWorkPlay netP;
    private TimePlay timeP;
    private AndroidPlay androidP;
    private KeyBoardPlay keyP;


    private static DebugControl _instance;

    public static DebugControl instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {

        _instance = this;

        MyInput.Init();

        DontDestroyOnLoad(gameObject);

        if (isOn)
        {
            GameObject go = gameObject;

            if (isPlay)
            {
#if UNITY_EDITOR
                GameObject oldeventSystem = GameObject.Find("EventSystem");
                if (oldeventSystem)
                    oldeventSystem.SetActive(false);

                eventSystem.SetActive(true);
                Get(inputP, go);
                Get(netP, go);
                Get(timeP, go);
                Get(androidP, go);
                Get(keyP, go);
#endif
            }
            else
            {
                eventSystem.SetActive(false);
                Get(inputR, go);
                Get(netR, go);
                Get(timeR, go);
                Get(androidR, go);
                Get(keyR, go);
            }
        }
        
    }

    private void OnDestroy()
    {
        if (isPlay)
        {
#if UNITY_EDITOR
            DestroyCom(inputP);
            DestroyCom(netP);
            DestroyCom(timeP);
            DestroyCom(androidP);
#endif
        }
        else
        {
            DestroyCom(inputR);
            DestroyCom(netR);
            DestroyCom(timeR);
            DestroyCom(androidP);
        }
    }

    public bool isRecord()
    {
        return isOn && !isPlay;
    }

    public bool isPlaying()
    {
        return isOn && isPlay;
    }

    private void Get<T>(T t, GameObject go) where T : MonoBehaviour
    {
        t = go.GetComponent<T>();
        if (inputR == null)
        {
            t = go.AddComponent<T>();
        }
    }

    private void DestroyCom<T>(T t) where T : MonoBehaviour
    {
        if(t!=null)
            GameObject.Destroy(t);
    }

}
