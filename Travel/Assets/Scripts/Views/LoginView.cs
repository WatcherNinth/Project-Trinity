using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Lucky;

public class LoginView : MonoBehaviour {

    public Button login;
    public Text name;

    void Start()
    {
        login.onClick.AddListener(onClick);
    }

    public void onClick()
    {
        StartCoroutine(PrefabManager.Instance.Init("Main"));
        
    }

}
