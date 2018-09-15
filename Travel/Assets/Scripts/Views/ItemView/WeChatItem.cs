using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;
using System;

public class WeChatMessage
{
    public string name;
    public string content;
    public DateTime date;
    public Action<WeChatMessage> callback;

    public WeChatMessage(string tname, string tcontent, DateTime tdate)
    {
        name = tname;
        content = tcontent;
        date = tdate;
        callback = null;
    }
}

public class WeChatItem : ItemRender
{
    public Text name;
    public Text content;
    public Text time;
    public Button btn;
    public int num;

    private string maincontent;

    protected override void UpdateView()
    {
        if (m_Data != null)
        {
            WeChatMessage tdata = m_Data as WeChatMessage;
            SetData(tdata);
        }
        base.UpdateView();
    }

    private void SetData(WeChatMessage tdata)
    {
        name.text = tdata.name;
        maincontent = tdata.content;
        if (maincontent.Length > num)
            content.text = maincontent.Substring(0, num) + "...";
        else
            content.text = maincontent;

        btn.onClick.AddListener(delegate()
        {
            
        });

        TimeSpan delta = TimeManager.instance.NowTime - tdata.date;
        double min = delta.TotalMinutes;
        if(min<30)
        {
            time.text = (int)min + "分钟前";
            return;
        }
        else if(min<45)
        {
            time.text = "半小时前";
            return;
        }
        else if(min<60)
        {
            time.text = "45分钟前";
            return;
        }
        else
        {
            double hour = delta.TotalHours;
            if(hour < 24)
            {
                time.text = (int)hour + "小时前";
                return;
            }
            else
            {
                double day = delta.TotalDays;
                time.text = (int)day + "天前";
                return;
            }
        }
    }

}
