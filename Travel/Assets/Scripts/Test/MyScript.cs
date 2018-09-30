using UnityEngine;
using System.Collections;

public class MyScript : MonoBehaviour {

    public int a;
    public int b;
    public type1 type;
    public enum type1
    {
        a,
        b
    }

	// Use this for initialization
	void Start () {
        Debug.Log(b+ " /////////////////////////////// "+a);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
