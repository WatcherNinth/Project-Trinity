using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour {

    private static GameSystem gs;

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
    }

    void Update()
    {
        MessageBus.Update(20);
        PopUpManager.Instance.Update();
        EventHappenManager.Instance.Update();
    }

    private IEnumerator Init()
    {
        Debug.Log("load prefab");
        yield return StartCoroutine(PrefabManager.Instance.Init());
        Debug.Log("load accident");
        yield return StartCoroutine(AccidentGenerator.Instance.Init());
        Debug.Log("load event");
        yield return StartCoroutine(EventHappenManager.Instance.Init());
        Debug.Log("load scene");
        AsyncOperation ao = SceneManager.LoadSceneAsync("Main");
        yield return ao;
        Debug.Log("load finish");
    }
	
}
