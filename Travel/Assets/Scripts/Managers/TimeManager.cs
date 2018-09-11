using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class TimeExecuteParam
{
    public BaseAccident accident;
    public Action<BaseAccident> exectuteCallback;

    public TimeExecuteParam(BaseAccident taccident, Action<BaseAccident> tcallback)
    {
        accident = taccident;
        exectuteCallback = tcallback;
    }

    public void Callback(BaseAccident value)
    {
        if(exectuteCallback!=null)
        {
            Type type = value.GetType();
            exectuteCallback(value);
        }
    }
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

    public float timespeed = 0.25f;
    //public float timespeed = 1.0f;
    public float TimeSpeed
    {
        get { return timespeed; }
        set { timespeed = value; }
    }

    private float time = 0;
    private object accidentlock = new object();
    private object golock = new object();

    public DateTime nowTime;

    public DateTime NowTime
    {
        get { return nowTime; }
    }

    private int i = 0;

    private SortedDictionary<DateTime, List<TimeExecuteParam>> waitingAccidents = new SortedDictionary<DateTime, List<TimeExecuteParam>>();

    private SortedDictionary<DateTime, Dictionary<long, TicketParam>> waitingGo = new SortedDictionary<DateTime, Dictionary<long, TicketParam>>();
    private Dictionary<long, DateTime> GoId = new Dictionary<long, DateTime>();

    private List<TimeExecuteParam> doAccidents = new List<TimeExecuteParam>();
    private List<TicketParam> doTickets = new List<TicketParam>();

    private string DateFormat = "MM/dd\nhh:mm";


    private void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start () {
        nowTime = GameModel.Instance.Start;
        Time.fixedDeltaTime = 1.0f/60.0f;
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        i++;
        //if(i%(15) ==0)
        if(i%(15) ==0)
        {
            //if (timespeed < )
            nowTime = nowTime.AddMinutes(timespeed);
            /*
            else if (timespeed < 1440)
                nowTime = nowTime.AddHours(timespeed / 60.0f);
            else
                nowTime = nowTime.AddDays(timespeed / 1440.0f);
            */
            if(timeText!=null)
                timeText.text = nowTime.ToString(DateFormat);
            i = 0;
            //Debug.Log("check");
            Check();
        }
    }

    private void Update()
    {
        if(timeText==null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("TimeText");
            if (go!=null)
                timeText = go.GetComponent<Text>();
        }
    }

    public void Check()
    {
        doTickets.Clear();
        doAccidents.Clear();

        //事故事件查找
        lock(accidentlock)
        {
            if(waitingAccidents.Count!=0)
            {
                var enumerator = waitingAccidents.GetEnumerator();
                enumerator.MoveNext();
                if (DateTime.Compare(enumerator.Current.Key, NowTime) < 0)
                {
                    List<TimeExecuteParam> teps = enumerator.Current.Value;
                    doAccidents.AddRange(teps);
                    waitingAccidents.Remove(enumerator.Current.Key);
                }
            }      
        }

        //事故事件执行
        foreach (TimeExecuteParam tep in doAccidents)
        {
            tep.Callback(tep.accident);
        }

        //旅游路线查找
        lock (golock)
        {
            
            if(waitingGo.Count!=0)
            {
                var etor = waitingGo.GetEnumerator();
                etor.MoveNext();
                Debug.Log("start time " + etor.Current.Key);
                if (DateTime.Compare(etor.Current.Key, NowTime) < 0)
                {
                    
                    Dictionary<long, TicketParam> dics = etor.Current.Value;
                    foreach (KeyValuePair<long, TicketParam> kvp in dics)
                    {
                        GoId.Remove(kvp.Key);
                        doTickets.Add(kvp.Value);
                    }
                    waitingGo.Remove(etor.Current.Key);
                }
            } 
            
        }

        //旅游路线执行
        foreach (TicketParam tp in doTickets)
        {
            Debug.Log("actual time" + nowTime);
            if (tp.rt.Type() == 0)
                MapTrafficView.instance.TrainGo(tp);
            else
                MapTrafficView.instance.AirPlaneFly(tp);
        }
    }

    public void AddAccidentExecute (BaseAccident value, Action<BaseAccident> callback)
    {
        lock(accidentlock)
        {
            if (waitingAccidents.ContainsKey(value.starttime))
            {
                List<TimeExecuteParam> list;
                waitingAccidents.TryGetValue(value.starttime, out list);
                if (list != null)
                    list.Add(new TimeExecuteParam(value, callback));
            }
            else
            {
                List<TimeExecuteParam> list = new List<TimeExecuteParam>();
                list.Add(new TimeExecuteParam(value, callback));
                waitingAccidents.Add(value.starttime, list);
            }
        }
    }

    public void AddGo(TicketParam value)
    {
        lock (golock)
        {
            long id = value.rt.GetRoutineId();
            DateTime start = value.rt.GetBeginTime();
            Debug.Log("add routine id "+id);
            GoId.Add(id, start);
            if (waitingGo.ContainsKey(start))
            {
                Dictionary<long, TicketParam> dicts;
                waitingGo.TryGetValue(start, out dicts);
                dicts.Add(id, value);
            }
            else
            {
                Dictionary<long, TicketParam> dicts = new Dictionary<long, TicketParam>();
                dicts.Add(id, value);
                waitingGo.Add(start, dicts);
            }
            Debug.Log("goid " + GoId.Count);
        }
    }

    public void RemoveGo(long id)
    {
        Debug.Log("remove goid " + GoId.Count);
        Debug.Log("remove id " + id);
        lock (golock)
        {
            if (GoId.ContainsKey(id))
            {
                Debug.Log("contain id "+id);
                //获取出发时间
                DateTime start;
                GoId.TryGetValue(id, out start);
                if (start != null)
                {
                    Debug.Log("contain start "+start);
                    //获取出发时间对应的事件
                    Dictionary<long, TicketParam> dicts;
                    waitingGo.TryGetValue(start, out dicts);
                    if (dicts != null)
                    {
                        Debug.Log("contain ticket param ");
                        dicts.Remove(id);
                    }
                    if (dicts.Count == 0)
                    {
                        waitingGo.Remove(start);
                    }
                }
                GoId.Remove(id);
            }
        }
    }

    public bool isTicketBuy(long id)
    {
        lock(golock)
        {
            return GoId.ContainsKey(id);
        }
    }

    public void GoToNextStartTime()
    {

    }
}
