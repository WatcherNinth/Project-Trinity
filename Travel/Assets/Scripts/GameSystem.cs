using UnityEngine;
using System.Collections;
using Lucky;

public class GameSystem : MonoBehaviour {

    private void Awake()
    {
        
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        MessageBus.Update(20);
    }
	
}
