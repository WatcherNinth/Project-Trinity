using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using Lucky;

public class TimeExecuteParam
{
    public BaseAccident accident;
    public Func<BaseAccident, MultiYield> exectuteCallback;
    public bool isDestroy;

    public TimeExecuteParam(BaseAccident taccident, Func<BaseAccident, MultiYield> tcallback, bool destroy=false)
    {
        accident = taccident;
        exectuteCallback = tcallback;
        isDestroy = destroy;
    }

    public IEnumerator Callback(BaseAccident value)
    {
        if (exectuteCallback != null)
        {
            Type type = value.GetType();
            yield return exectuteCallback(value);
        }
        else
            yield return null;
    }
}

public class MessageParam<T>
{
    public T message;
    public Action<T> callback;

    public MessageParam(T tmessage, Action<T> tcallback)
    {
        message = tmessage;
        callback = tcallback;
    }

    public void Callback()
    {
        if(callback!=null)
        {
            callback(message);
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

    private Text timeText;
    private MapsView mv;

    public float normalSpeed= 0.0166f;
    public float fastSpeed = 10f;

    private float timespeed;
    public float TimeSpeed
    {
        get { return timespeed; }
        set
        {
            timespeed = value;
            if(timespeed == normalSpeed)
            {
                if(mv!=null)
                {
                    mv.ChangeGoButton();
                }    
            }
            MapTrafficView.instance.SetAnimatorSpeed();
        }
    }

    private float time = 0;
    private object accidentlock = new object();
    private object golock = new object();

    private DateTime nextTime;
    private DateTime nextSlowTime;

    private DateTime nowTime;
    public DateTime NowTime
    {
        get { return nowTime; }
    }

    private int i = 0;

    private SortedDictionary<DateTime, List<TimeExecuteParam>> waitingAccidents = new SortedDictionary<DateTime, List<TimeExecuteParam>>();
    private SortedDictionary<DateTime, Dictionary<long, TicketParam>> waitingGo = new SortedDictionary<DateTime, Dictionary<long, TicketParam>>();
    private SortedDictionary<DateTime, List<MessageParam<WeChatMessage>>> waitingWeChat = new SortedDictionary<DateTime, List<MessageParam<WeChatMessage>>>();
    private SortedDictionary<DateTime, List<MessageParam<NewMessage>>> waitingNew = new SortedDictionary<DateTime, List<MessageParam<NewMessage>>>();

    private Dictionary<long, DateTime> GoId = new Dictionary<long, DateTime>();

    private List<TimeExecuteParam> doAccidents = new List<TimeExecuteParam>();
    private List<TicketParam> doTickets = new List<TicketParam>();
    private List<MessageParam<WeChatMessage>> doWechat = new List<MessageParam<WeChatMessage>>();
    private List<MessageParam<NewMessage>> doNew = new List<MessageParam<NewMessage>>();

    private string DateFormat = "MM/dd\nHH:mm:ss";

    private float oldspeed=0;

    private void Awake()
    {
        timespeed = normalSpeed;
        _instance = this;
    }

    // Use this for initialization
    void Start () {
        nowTime = GameModel.Instance.Start;
        nextTime = nowTime.AddMinutes(30);
        Time.fixedDeltaTime = 1.0f/60.0f;
        DontDestroyOnLoad(gameObject);
    }

    private void FixedUpdate()
    {
        if(TimeSpeed!=0)
        {
            i++;
            if (i % (15) == 0)
            {
                nowTime = nowTime.AddMinutes(TimeSpeed / 4);
                if (timeText != null)
                    timeText.text = nowTime.ToString(DateFormat);
                i = 0;
                if (timespeed != 0)
                    Check();
            }
        }
    }

    public void Check()
    {
        doTickets.Clear();
        doAccidents.Clear();
        doNew.Clear();
        doWechat.Clear();

        if(DateTime.Compare(nextSlowTime, nowTime)<0)
        {
            nextSlowTime = DateTime.MaxValue;
        }

        if(DateTime.Compare(nextTime, nowTime)<0)
        {
            nextTime = nowTime.AddMinutes(30);
            EventHappenManager.Instance.EveryThirtyMinutes(nowTime);
        }

        //事故事件查找
        if(waitingAccidents.Count!=0)
        {
            var enumerator = waitingAccidents.GetEnumerator();
            enumerator.MoveNext();
            //Lucky.LuckyUtils.Log("next starttiem "+enumerator.Current.Key);
            if (DateTime.Compare(enumerator.Current.Key, NowTime) < 0)
            {
                List<TimeExecuteParam> teps = enumerator.Current.Value;
                doAccidents.AddRange(teps);
                waitingAccidents.Remove(enumerator.Current.Key);
            }
        }      

        //事故事件执行
        foreach (TimeExecuteParam tep in doAccidents)
        {
            
            if (!tep.isDestroy)
            {
                Lucky.LuckyUtils.Log("new accident");
                StartCoroutine(tep.Callback(tep.accident));
                MapTrafficView.instance.ShowAccidentMessage(tep.accident);
            }
            else
            {
                MapTrafficView.instance.DisplayMessage(tep.accident);
            }
        }

        //旅游路线查找
        lock (golock)
        {
            
            if(waitingGo.Count!=0)
            {
                var etor = waitingGo.GetEnumerator();
                etor.MoveNext();
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
            //TimeSpeed = 1.0f;
            if (tp.rt.Type() == 0)
                MapTrafficView.instance.TrainGo(tp);
            else
                MapTrafficView.instance.AirPlaneFly(tp);
        }

        //微信事件查找
        if(waitingWeChat.Count!=0)
        {
            var etor = waitingWeChat.GetEnumerator();
            etor.MoveNext();
            if (DateTime.Compare(etor.Current.Key, NowTime) < 0)
            {

                List<MessageParam<WeChatMessage>> teps = etor.Current.Value;
                doWechat.AddRange(teps);
                waitingWeChat.Remove(etor.Current.Key);
            }
        }

        foreach(MessageParam<WeChatMessage> mp in doWechat)
        {
            //TimeSpeed = 1.0f;
            mp.Callback();
            MessageBus.Post(new MessageObject(mp.message));
        }

        //新闻事件查找
        if (waitingNew.Count != 0)
        {
            var etor = waitingNew.GetEnumerator();
            etor.MoveNext();
            if (DateTime.Compare(etor.Current.Key, NowTime) < 0)
            {

                List<MessageParam<NewMessage>> teps = etor.Current.Value;
                doNew.AddRange(teps);
                waitingNew.Remove(etor.Current.Key);
            }
        }

        foreach (MessageParam<NewMessage> mp in doNew)
        {
            //TimeSpeed = 1.0f;
            mp.Callback();
            MessageBus.Post(new MessageObject(mp.message));
        }

        if(DateTime.Compare(GameModel.Instance.tomorrow, nowTime)<0)
        {
            InfoView.Show(new InfoMessage("失败","垃圾"));
            timespeed = 0.0f;
        }

    }

    public bool AddNews(NewMessage message, Action<NewMessage> callback)
    {
        if(DateTime.Compare(message.date, nowTime) < 0)
        {
            Lucky.LuckyUtils.Log("error " + message.title);
            return false;
        }
        else
        {
            if(waitingNew.ContainsKey(message.date))
            {
                List<MessageParam<NewMessage>> list;
                waitingNew.TryGetValue(message.date, out list);
                if (list != null)
                    list.Add(new MessageParam<NewMessage>(message, callback));
            }
            else
            {
                List<MessageParam<NewMessage>> list = new List<MessageParam<NewMessage>>();
                list.Add(new MessageParam<NewMessage>(message, callback));
                waitingNew.Add(message.date, list);
            }
            return true;
        }
    }

    public bool AddWeChat(WeChatMessage message, Action<WeChatMessage> callback)
    {
        if (DateTime.Compare(message.date, nowTime) < 0)
        {
            Lucky.LuckyUtils.Log("error " + message.date);
            return false;
        }
        else
        {
            if (waitingWeChat.ContainsKey(message.date))
            {
                List<MessageParam<WeChatMessage>> list;
                waitingWeChat.TryGetValue(message.date, out list);
                if (list != null)
                    list.Add(new MessageParam<WeChatMessage>(message, callback));
            }
            else
            {
                List<MessageParam<WeChatMessage>> list = new List<MessageParam<WeChatMessage>>();
                list.Add(new MessageParam<WeChatMessage>(message, callback));
                waitingWeChat.Add(message.date, list);
            }
            return true;
        }
    }

    public bool AddAccidentExecute (BaseAccident value, Func<BaseAccident, MultiYield> callback, bool isDestroy=false)
    {
        if (DateTime.Compare(value.starttime, nowTime) < 0)
        {
            Lucky.LuckyUtils.Log("error " + value.starttime+nowTime);
            return false;
        }
        lock (accidentlock)
        {
            if (waitingAccidents.ContainsKey(value.starttime))
            {
                List<TimeExecuteParam> list;
                waitingAccidents.TryGetValue(value.starttime, out list);
                if (list != null)
                    list.Add(new TimeExecuteParam(value, callback, isDestroy));
            }
            else
            {
                List<TimeExecuteParam> list = new List<TimeExecuteParam>();
                list.Add(new TimeExecuteParam(value, callback, isDestroy));
                waitingAccidents.Add(value.starttime, list);
            }
        }
        return true;
    }

    public bool AddGo(TicketParam value)
    {
        /*
        if (DateTime.Compare(value.rt.GetBeginTime(), nowTime) < 0)
        {
            Lucky.LuckyUtils.Log("error " + value.rt.GetBeginTime());
            return false;
        }
        */
        lock (golock)
        {
            long id = value.rt.GetRoutineId();
            DateTime start = value.rt.GetBeginTime();
            Lucky.LuckyUtils.Log("add routine id "+id);
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
            Lucky.LuckyUtils.Log("goid " + GoId.Count);
        }
        return true;
    }

    public void RemoveGo(long id)
    {
        Lucky.LuckyUtils.Log("remove goid " + GoId.Count);
        Lucky.LuckyUtils.Log("remove id " + id);
        lock (golock)
        {
            if (GoId.ContainsKey(id))
            {
                Lucky.LuckyUtils.Log("contain id "+id);
                //获取出发时间
                DateTime start;
                GoId.TryGetValue(id, out start);
                if (start != null)
                {
                    Lucky.LuckyUtils.Log("contain start "+start);
                    //获取出发时间对应的事件
                    Dictionary<long, TicketParam> dicts;
                    waitingGo.TryGetValue(start, out dicts);
                    if (dicts != null)
                    {
                        Lucky.LuckyUtils.Log("contain ticket param ");
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

    public void Delay(List<int> delayid)
    {
        foreach (int id in delayid)
        {
            lock (golock)
            {
                if (GoId.ContainsKey(id))
                {
                    RemoveGo(id);
                    TicketsOperaton ticket_operation = new TicketsOperaton();
                    RoutineTicket ticket = ticket_operation.GetTicketByTickedId(id);
                    Lucky.LuckyUtils.Log("delay " + ticket.GetActualBeginTime());
                    AddGo(new TicketParam(ticket));
                }
            }
        }
    }

    public void GoToNextStartTime()
    {
        DateTime dt = DateTime.MaxValue;
        if(waitingAccidents.Count!=0)
        {
            var etor = waitingAccidents.GetEnumerator();
            etor.MoveNext();
            Lucky.LuckyUtils.Log("accident next start time " + etor.Current.Key);
            if (DateTime.Compare(etor.Current.Key, dt) < 0)
            {
                //if(etor.Current.Value.)
                List<TimeExecuteParam> list = etor.Current.Value;
                int i = 0;
                for (;i<list.Count;i++)
                {
                    Lucky.LuckyUtils.Log("get accident "+list[i].accident.starttime +" destroy "+list[i].isDestroy);
                    if (list[i].isDestroy == false)
                        break;
                }
                if(i!=list.Count)
                    dt = etor.Current.Key;
            }
        }

        /*
        if(waitingGo.Count!=0)
        {
            var etor = waitingGo.GetEnumerator();
            etor.MoveNext();
            Lucky.LuckyUtils.Log("go next start time " + etor.Current.Key);
            if (DateTime.Compare(etor.Current.Key, dt) < 0)
            {
                dt = etor.Current.Key;
            }
        }
        */

        if(DateTime.Compare(dt , DateTime.MaxValue) != 0)
        {
            nextSlowTime = dt.AddMinutes(-5);
        }
        SetFastSpeed();

    }

    public void SetText(Text t)
    {
        timeText = t;
    }

    public void SetMapsView(MapsView tmv)
    {
        mv = tmv;
    }

    public void StopTimeManager()
    {
        oldspeed = timespeed;
        TimeSpeed = 0;
    }

    public void StartTimeManager()
    {
        TimeSpeed = oldspeed;
    }

    public void SetNormalSpeed()
    {
        TimeSpeed = normalSpeed;
    }

    public void SetFastSpeed()
    {
        TimeSpeed = fastSpeed;
    }

    public bool HasTickets()
    {
        if (waitingGo.Count == 0 && doTickets.Count == 0)
        {
            return false;
        }
        else
            return true;
    }

}
