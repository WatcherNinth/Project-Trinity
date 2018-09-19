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
    public Button cancel;

    private Image img;

    private void Awake()
    {
        img = Login.GetComponent<Image>();
    }

    void Start()
    {
        Loading.SetActive(false);
        cancel.gameObject.SetActive(false);
        Login.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        Login.gameObject.SetActive(false);
        Login.interactable = false;
        //Loading.SetActive(true);
        //StartCoroutine(GameSystem.instance.Init());
        
        if (UserTicketsModel.Instance.firstEnter != 0)
        {
            Loading.SetActive(true);
            StartCoroutine(GameSystem.instance.Init());
        }
        else
        {
            bg.sprite = SpriteManager.Instance.GetSprite(Sprites.WeChatCall);

            using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    //调用Android插件中UnityTestActivity中StartActivity0方法，stringToEdit表示它的参数
                    jo.Call("Start");
                }

            }

            AudioManager.Instance.PlayMusic(Audios.WeChatCall,true);
            call.onClick.AddListener(delegate() 
            {
                using (AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        //调用Android插件中UnityTestActivity中StartActivity0方法，stringToEdit表示它的参数
                        jo.Call("Stop");
                    }

                }

                call.gameObject.SetActive(false);
                cancel.gameObject.SetActive(true);
                cancel.onClick.AddListener(delegate()
                {
                    cancel.gameObject.SetActive(false);
                    AudioManager.Instance.Stop(Audios.WeChatCalling);
                    StopCoroutine("WaitPlay");
                    StartCoroutine(WaitPlay(0));
                });
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
