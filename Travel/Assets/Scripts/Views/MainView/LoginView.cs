using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Lucky;
using System.Threading;

public class LoginView : MonoBehaviour {

    
    public Button Login;
    public GameObject Loading;
    public Sprite Change;

    private Image img;

    private void Awake()
    {
        img = Login.GetComponent<Image>();
    }

    void Start()
    {
        Loading.SetActive(false);
        Login.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Loading.SetActive(true);
        //img.sprite = Change;
        Login.interactable = false;
        StartCoroutine(GameSystem.instance.Init());
        if (UserTicketsModel.Instance.firstEnter != 0)
        {
            //StartCoroutine(GameSystem.instance.Init());
        }
        else
        {

        }
    }

}
