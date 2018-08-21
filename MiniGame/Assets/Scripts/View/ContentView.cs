using UnityEngine;
using System.Collections;

public class ContentView : MonoBehaviour {

    public string[] tools;
    public GameObject FuncObject;

	// Use this for initialization
	void Start () {

	    foreach(string s in tools)
        {
            GameObject temp = Instantiate(FuncObject);
            temp.transform.SetParent(transform);
            temp.SetActive(true);
            temp.GetComponent<RectTransform>().localScale = Vector3.one;
            temp.GetComponent<FuncView>().SetText(s);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
