using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour {

    private static GameSystem gs;
    private float lasttime;

    public static GameSystem instance
    {
        get { return gs; }
    }

    private void Awake()
    {
        gs = this;
        Screen.orientation = ScreenOrientation.Portrait;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(Init());
        lasttime = Time.realtimeSinceStartup;

    }

    void Update()
    {
        MessageBus.Update(20);
        PopUpManager.Instance.Update();
        if( Time.realtimeSinceStartup-lasttime > 20 )
        {
            Resources.UnloadUnusedAssets();
            System.GC.Collect();
            lasttime = Time.realtimeSinceStartup;
        }
    }

    private IEnumerator Init()
    {
        Lucky.LuckyUtils.Log("load text");
        yield return StartCoroutine(TextManager.Instance.Init());
        Lucky.LuckyUtils.Log("load prefab");
        yield return StartCoroutine(PrefabManager.Instance.Init());
        Lucky.LuckyUtils.Log("load accident");
        yield return StartCoroutine(AccidentGenerator.Instance.Init());
        Lucky.LuckyUtils.Log("load event");
        yield return StartCoroutine(EventHappenManager.Instance.Init());
        Lucky.LuckyUtils.Log("load audio");
        yield return StartCoroutine(AudioManager.Instance.Init());
        Lucky.LuckyUtils.Log("load wechat");
        yield return StartCoroutine(WeChatManager.Instance.Init());
        Lucky.LuckyUtils.Log("load new ");
        yield return StartCoroutine(NewManager.Instance.Init());
        Lucky.LuckyUtils.Log("load scene");
        AsyncOperation ao = SceneManager.LoadSceneAsync("Main");
        yield return ao;
        Lucky.LuckyUtils.Log("load finish");
        AudioManager.Instance.PlayMusic(Audios.BG, true);
    }
	
}
