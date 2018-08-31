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
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        MessageBus.Update(20);
        PopUpManager.Instance.Update();
    }
	
}
