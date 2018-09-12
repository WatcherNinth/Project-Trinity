using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InitText : MonoBehaviour {

    private Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }

	// Use this for initialization
	void Start () {
        TimeManager.instance.SetText(text);
	}
}
