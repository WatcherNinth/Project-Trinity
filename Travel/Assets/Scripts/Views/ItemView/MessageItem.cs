using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class MessageItem : ItemRender
{
    public Image image;
    public Text title;
    public Text content;
    public Text time;
    public Button btn;
    public int num;
    public Image bg;

    private string maincontent;
    private float height;
    private float EffectDisposeTime = 0.5f;
    private int timeCountDown=5;
    private int timeFade = 0;
    private bool show=false;
    private int i = 0;

    public bool Showing
    {
        set { show = value; }
    }

    private RectTransform rt;

    protected override void Awake()
    {
        base.Awake();
    }

    private void FixedUpdate()
    {
        if(i%60==0)
        {
            if (timeFade != 0)
            {
                if (timeCountDown < 0)
                {
                    HideItem();
                    timeFade = 0;
                }
                timeCountDown -= timeFade;
            }
            i = 0;
        }
        i++;
        
        
    }

    protected override void Start()
    {
        rt = GetComponent<RectTransform>();
        height = Mathf.Abs(rt.rect.y);
        base.Start();
        if(show)
        {
            ShowItem();
        }
        
    }

    public void EnableBg()
    {
        bg.enabled = true;
    }

    protected override void InitUI()
    {
        base.InitUI();
        if(show)
            rt.anchoredPosition = new Vector2(0, height);
    }

    public void ShowItem()
    {
        float num = height;
        Tween tween = DOTween.To(
               () => num,
               x => num = x,
               0,
               EffectDisposeTime
           );

        tween.OnUpdate
        (
             () => onDisposeUpdate(num)
        );

        tween.OnComplete
        (
            () => onShowCompute()
        );
    }

    public void StartCountDown()
    {
        timeFade = 1;
    }

    public void StopCountDown()
    {
        timeFade = 0;
    }

    private void HideItem()
    {
        float num = 0;
        Tween tween = DOTween.To(
               () => num,
               x => num = x,
               height,
               EffectDisposeTime
           );

        tween.OnUpdate
        (
             () => onDisposeUpdate(num)
        );

        tween.OnComplete
        (
            () => onHideCompute()
        );
    }

    private void onDisposeUpdate(float num)
    {
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, num);
    }

    private void onHideCompute()
    {
        MessagePopUpView.instance.DestroyMessage();
        Destroy(gameObject);
    }

    private void onShowCompute()
    {
        StartCountDown();
    }

    protected override void UpdateView()
    {
        if (m_Data != null)
        {
            Type classtype = m_Data.GetType();
            if(classtype == typeof(NewMessage))
            {
                NewMessage tdata = m_Data as NewMessage;
                SetData(tdata);
            }
            else if(classtype==typeof(WeChatMessage))
            {
                Lucky.LuckyUtils.Log("we chat message get");
                WeChatMessage tdata = m_Data as WeChatMessage;
                SetData(tdata);
            }
            else if(classtype == typeof(Accident))
            {
                Accident tdata = m_Data as Accident;
                SetData(tdata);
            }
            else if(classtype == typeof(ItemMessage))
            {
                ItemMessage tdata = m_Data as ItemMessage;
                SetData(tdata);
            }
            
        }
        base.UpdateView();
    }

    private void SetData(NewMessage tdata)
    {
        image.sprite = SpriteManager.Instance.GetSprite(Sprites.news);
        title.text = tdata.title;
        maincontent = tdata.content;
        if (maincontent.Length > num)
            content.text = maincontent.Substring(0, num) + "...";
        else
            content.text = maincontent;

        time.text = "现在";

        btn.onClick.RemoveAllListeners();
        if (tdata.callback==null)
        {
            btn.onClick.AddListener(delegate ()
            {
                HideItem();
            });
            time.text = "现在";
        }
        else
        {
            btn.onClick.AddListener(delegate ()
            {
                tdata.callback(tdata);
            });
            time.text = GetTime(tdata.date);
        }

        return;
    }

    private void SetData(WeChatMessage tdata)
    {
        string path = "";
        switch(tdata.name)
        {
            case "妈妈":
                path = Sprites.mother;
                break;
            case "爸爸":
                path = Sprites.father;
                break;
            default:
                path = Sprites.sister;
                break;
        }
        image.sprite = SpriteManager.Instance.GetSprite(path);
        title.text = tdata.name;
        maincontent = tdata.content;
        if (maincontent.Length > num)
            content.text = maincontent.Substring(0, num) + "...";
        else
            content.text = maincontent;


        btn.onClick.RemoveAllListeners();
        if (tdata.callback == null)
        {
            btn.onClick.AddListener(delegate ()
            {
                HideItem();
            });
            time.text = "现在";
        }
        else
        {
            btn.onClick.AddListener(delegate ()
            {
                Lucky.LuckyUtils.Log("show");
                tdata.callback(tdata);
            });
            time.text = GetTime(tdata.date);
        }
    }

    private void SetData(ItemMessage itemMessage)
    {
        image.sprite = SpriteManager.Instance.GetSprite(Sprites.shorttext);
        title.text = itemMessage.name;
        maincontent = itemMessage.content;
        if (maincontent.Length > num)
            content.text = maincontent.Substring(0, num) + "...";
        else
            content.text = maincontent;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(delegate ()
        {
            HideItem();
        });
        time.text = "现在";
    }

    private void SetData(Accident tdata)
    {
        title.text = tdata.text.title;
        maincontent = tdata.text.description;
        if (maincontent.Length > num)
            content.text = maincontent.Substring(0, num) + "...";
        else
            content.text = maincontent;

        time.text = "现在";

        btn.onClick.AddListener(delegate ()
        {

        });
    }

    public string GetTime(DateTime dt)
    {
        TimeSpan delta = TimeManager.instance.NowTime - dt;
        double min = delta.TotalMinutes;
        if (min < 30)
        {
            return (int)min + "分钟前";
        }
        else if (min < 45)
        {
            return "半小时前";
        }
        else if (min < 60)
        {
            return "45分钟前";
        }
        else
        {
            double hour = delta.TotalHours;
            if (hour < 24)
            {
                return (int)hour + "小时前";
            }
            else
            {
                double day = delta.TotalDays;
                return (int)day + "天前";
            }
        }
    }

}
