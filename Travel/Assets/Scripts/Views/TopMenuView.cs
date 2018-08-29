using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class TopMenuView : MonoBehaviour {

    public Transform WeChat;

    public Button BtnWeChat;

    private RectTransform rt;
    private float num;
    private bool show;

    private void Awake()
    {
        show = false;
        rt = WeChat.GetComponent<RectTransform>();
    }

    // Use this for initialization
    void Start () {
        BtnWeChat.onClick.AddListener(onShowOrHide);
	}
	
	// Update is called once per frame
	void Update () {
    
	
	}

    private void onShowOrHide()
    {
        BtnWeChat.interactable = false;
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
            };
        }
        
    }

    private void onUpdateValue(float value)
    {
        Debug.Log("value" + value);
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, value);
    }

    private void onCompleteValue()
    {
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, 1728);
    }
}
