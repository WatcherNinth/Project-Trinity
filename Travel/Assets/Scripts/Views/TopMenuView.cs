using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Lucky;
using System;

public class UseMoney
{
    public float money;

    public UseMoney(float tmoney)
    {
        money = tmoney;
    }
}

public class TopMenuView : MonoBehaviour {

    public Transform WeChat;
    public Button BtnWeChat;
    public Transform BtnWeChatBg;
    public Text Money;

    private RectTransform rt;
    private float num;
    private bool show;

    private void Awake()
    {
        show = false;
        rt = WeChat.GetComponent<RectTransform>();
        rt.gameObject.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        RegisterMsg(true);
        Money.text = "￥" + UserTicketsModel.Instance.money;
        BtnWeChat.onClick.AddListener(onShowOrHide);
	}

    private void OnDestroy()
    {
        RegisterMsg(false);
    }

    private void onShowOrHide()
    {
        AudioManager.Instance.PlayMusic(Audios.ButtonClip);
        BtnWeChat.interactable = false;
        rt.gameObject.SetActive(true);
        if (!show)
        {
            num = 0;
            Tween t = DOTween.To
            (
                () => num,
                x => rt.sizeDelta = new Vector2(rt.sizeDelta.x, x),
                1728,
                1
            );
            t.onComplete = delegate ()
            {
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, 1728);
                show = true;
                BtnWeChat.interactable = true;
                BtnWeChatBg.localEulerAngles = new Vector3(0, 0, 0);
            };
        }
        else
        {
            num = 1728;
            Tween t = DOTween.To
            (
                () => num,
                x => rt.sizeDelta = new Vector2(rt.sizeDelta.x, x),
                0,
                1
            );
            t.onComplete = delegate ()
            {
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, 0);
                show = false;
                BtnWeChat.interactable = true;
                rt.gameObject.SetActive(false);
                BtnWeChatBg.localEulerAngles = new Vector3(0, 0, 180);
            };
        }
        
    }

    private void onUpdateValue(float value)
    {
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, value);
    }

    private void onCompleteValue()
    {
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, 1728);
    }

    private void RegisterMsg(bool isOn)
    {
        if (isOn)
        {
            MessageBus.Register<WeChatMessage>(AddWeChatMessage);
            MessageBus.Register<NewMessage>(AddNewMessage);
            MessageBus.Register<UseMoney>(HandleMoney);
        }
        else
        {
            MessageBus.UnRegister<WeChatMessage>(AddWeChatMessage);
            MessageBus.UnRegister<NewMessage>(AddNewMessage);
            MessageBus.UnRegister<UseMoney>(HandleMoney);
        }
}

    private bool HandleMoney(UseMoney m)
    {
        float money = Convert.ToSingle(Money.text.Substring(1));
        money += m.money;
        Money.text = "￥" + money;
        UserTicketsModel.Instance.money = money;
        PlayerPrefs.SetFloat("money", money);
        return false;
    }

    private bool AddWeChatMessage(WeChatMessage data)
    {
        MessageModel.Instance.WeChatList.Add(data);
        return false;
    }

    private bool AddNewMessage(NewMessage data)
    {
        MessageModel.Instance.NewsList.Add(data);
        return false;
    }
}
