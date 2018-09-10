using UnityEngine;
using System.Collections;
using Lucky;

public class GameSystem : MonoBehaviour {

    private static GameSystem gs;

    public static GameSystem instance
    {
        get { return gs; }
    }

    private void Awake()
    {
        gs = this;
        Screen.orientation = ScreenOrientation.LandscapeRight;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(PrefabManager.Instance.Init());
        StartCoroutine(EventHappenManager.Instance.Init());
        /*
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(mainscene);
        */
    }

    void Update()
    {
        MessageBus.Update(20);
        PopUpManager.Instance.Update();
        EventHappenManager.Instance.Update();
    }
	
}
