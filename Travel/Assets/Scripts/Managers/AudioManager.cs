using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    private static AudioManager _instance;

    private static AudioManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
