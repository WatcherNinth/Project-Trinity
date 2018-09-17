using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Lucky;
using System.Threading;

public class LoginView : MonoBehaviour {

    
    public Button Login;
    public Button call;
    public GameObject Loading;
    public Image bg;

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
        Login.gameObject.SetActive(false);
        Login.interactable = false;
        if (UserTicketsModel.Instance.firstEnter != 0)
        {
            Loading.SetActive(true);
            StartCoroutine(GameSystem.instance.Init());
        }
        else
        {
            bg.sprite = SpriteManager.Instance.GetSprite(Sprites.WeChatCall);
            AudioManager.Instance.PlayMusic(Audios.WeChatCall,true);
            call.onClick.AddListener(delegate() 
            {
                AudioManager.Instance.Stop(Audios.WeChatCall);
                bg.sprite = SpriteManager.Instance.GetSprite(Sprites.WeChatCalling);
                AudioClip ac = AudioManager.Instance.GetAudioClip(Audios.WeChatCalling);
                AudioManager.Instance.PlayMusic(Audios.WeChatCalling);
                StartCoroutine(WaitPlay(ac.length));
            });
            call.gameObject.SetActive(true);
        }
    }

    private IEnumerator WaitPlay(float len)
    {
        yield return new WaitForSeconds(len);
        Loading.SetActive(true);
        yield return GameSystem.instance.Init();
    }

}
