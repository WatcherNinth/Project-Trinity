using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class TimeExecuteParam
{
    public DateTime dateTime;
    public Action exectuteCallback;
}

public class TimeManager : MonoBehaviour {

    private static TimeManager _instance = null;

    public static TimeManager instance
    {
        get
        {
            if(_instance==null)
            {
                _instance = GameSystem.instance.gameObject.AddComponent<TimeManager>();
            }
            return _instance;
        }
    }

    public Text timeText;

    public int timespeed = 1;
    public int TimeSpeed
    {
        get { return timespeed; }
        set { timespeed = value; }
    }

    private float time = 0;
    private DateTime nowTime;

    public DateTime NowTime
    {
        get { return nowTime; }
    }

    private int i = 0;

    private Dictionary<long, TimeExecuteParam> waiting = new Dictionary<long, TimeExecuteParam>();
    private List<TimeExecuteParam> executing = new List<TimeExecuteParam>();

    private string DateFormat = "MM/dd\nhh:mm";


    private void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start () {
        nowTime = GameModel.Instance.Start;
        Time.fixedDeltaTime = 1.0f/60.0f;
    }

    private void FixedUpdate()
    {
        i++;
        if(i%(60) ==0)
        {
            if (timespeed < 60)
                nowTime = nowTime.AddMinutes(timespeed);
            else if (timespeed < 1440)
                nowTime = nowTime.AddHours(timespeed / 60.0f);
            else
                nowTime = nowTime.AddDays(timespeed / 1440.0f);
            timeText.text = nowTime.ToString(DateFormat);
            i = 0;
        }
    }

    public void Check()
    {
        var enumerator = waiting.GetEnumerator();
        while(enumerator.MoveNext())
        {
            TimeExecuteParam param = enumerator.Current.Value;

            DateTime dest = param.dateTime;
            if(nowTime > dest)
            {
                param.exectuteCallback();
            }
        }
    }

    public void AddExecute(long id, TimeExecuteParam param)
    {
        if(!waiting.ContainsKey(id))
            waiting.Add(id, param);
    }

    public void RemoveExecute(long id)
    {
        if (waiting.ContainsKey(id))
            waiting.Remove(id);
    }

    public void ResetExecute(long id, TimeExecuteParam param)
    {
        RemoveExecute(id);
        AddExecute(id, param);
    }
}
