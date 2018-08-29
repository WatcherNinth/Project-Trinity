using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginView : MonoBehaviour {

    public Button login;
    public Text name;

    void Start()
    {
        login.onClick.AddListener(onClick);
    }

    public void onClick()
    {
        SceneManager.LoadScene("Main");
    }

}
