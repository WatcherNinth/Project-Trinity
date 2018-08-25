using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginView : MonoBehaviour {

    public Button btn;

	// Use this for initialization
	void Start () {
        btn.onClick.AddListener(ChangeSence);
	}
	
	public void ChangeSence()
    {
        SceneManager.LoadSceneAsync("Main");
    }
}
