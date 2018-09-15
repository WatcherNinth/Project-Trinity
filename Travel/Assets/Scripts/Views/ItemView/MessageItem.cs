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

    private string maincontent;
    private float height;
    private float EffectDisposeTime = 0.25f;

    private RectTransform rt;

    protected override void Start()
    {
        base.Start();
        rt = GetComponent<RectTransform>();
        height = rt.rect.y;
    }

    public void ShowItem()
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
    }

    private void HideItem()
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
    }

    private void onDisposeUpdate(float num)
    {
        rt.anchoredPosition = new Vector2(rt.anchoredPosition.x, num);
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
                WeChatMessage tdata = m_Data as WeChatMessage;
                SetData(tdata);
            }
            else if(classtype == typeof(Accident))
            {
                Accident tdata = m_Data as Accident;
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
        image.sprite = SpriteManager.Instance.GetSprite(Sprites.wechat);
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
                Debug.Log("show");
                tdata.callback(tdata);
            });
            time.text = GetTime(tdata.date);
        }
    }

    private void SetData()
    {

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
