using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;
using System;

public class NewMessage
{
    public string title;
    public string content;
    public DateTime date;
    public Action<NewMessage> callback;

    public NewMessage(string ttitle, string tcontent, DateTime tdate)
    {
        title = ttitle;
        content = tcontent;
        date = tdate;
        callback = null;
    }
}

public class NewItem : ItemRender
{
    public Text title;
    public Text content;
    public Text time;
    public Button btn;
    public int num;

    private string maincontent;

    protected override void UpdateView()
    {
        if (m_Data != null)
        {
            NewMessage tdata = m_Data as NewMessage;
            SetData(tdata);
        }
        base.UpdateView();
    }

    private void SetData(NewMessage tdata)
    {
        title.text = tdata.title;
        maincontent = tdata.content;
        if (maincontent.Length > num)
            content.text = maincontent.Substring(0, num) + "...";
        else
            content.text = maincontent;

        btn.onClick.AddListener(delegate ()
        {
            InfoView.Show(new InfoMessage(tdata.content,tdata.title));
        });

        TimeSpan delta = TimeManager.instance.NowTime - tdata.date;
        double min = delta.TotalMinutes;
        if (min < 30)
        {
            time.text = (int)min + "分钟前";
            return;
        }
        else if (min < 45)
        {
            time.text = "半小时前";
            return;
        }
        else if (min < 60)
        {
            time.text = "45分钟前";
            return;
        }
        else
        {
            double hour = delta.TotalHours;
            if (hour < 24)
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
