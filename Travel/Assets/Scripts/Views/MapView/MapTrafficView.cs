using UnityEngine;
using System.Collections;
using System;
using Lucky;
using System.Collections.Generic;

public class MapTrafficView : MonoBehaviour {

    public AirLineView airline;
    public GameObject airplane;
    public GameObject train;
    public GameObject location;

    private Animator animator;
    private float traveltime;
    private float cliptime;
    private string animationName;
    private int ticketid;
    private string dst;
    private string[] citys;

    private Dictionary<int, WarningView> Trainwarndic = new Dictionary<int, WarningView>();
    private Dictionary<int, WarningView> AirPlanewarndic = new Dictionary<int, WarningView>();

    private static MapTrafficView _instance;
    public static MapTrafficView instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        train.SetActive(false);
        airplane.SetActive(false);
        EventHappenManager.Instance.EveryLocation("上海");
        citys = new string[LocationsModel.cityslocation.Count];
        LocationsModel.cityslocation.Keys.CopyTo(citys, 0);
        TimeManager.instance.SetNormalSpeed();
    }

    private void Update()
    {
        if(train.activeSelf)
        {
            AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
            if ((asi.normalizedTime > 1.0f) && (asi.IsName(animationName)))
            {
                Lucky.LuckyUtils.Log("train arrive");
                train.SetActive(false);
                string city = FindPlaceName(dst);
                UserTicketsModel.Instance.where = Where.City;
                UserTicketsModel.Instance.city = city;
                BuyTicketsModel.Instance.startlocation = city;
                BuyTicketsModel.Instance.stoplocation = "沈阳";
                TimeManager.instance.SetNormalSpeed();
                EventHappenManager.Instance.EveryLocation(city);
                UserTicketsModel.Instance.going = false;
            }
        }

        if(airplane.activeSelf)
        {
            AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
            if ((asi.normalizedTime > 1.0f) && (asi.IsName(animationName)))
            {
                Lucky.LuckyUtils.Log("airplane arrive");
                airplane.SetActive(false);
                
                UserTicketsModel.Instance.where = Where.City;
                string city = FindPlaceName(dst);
                UserTicketsModel.Instance.city = city;
                BuyTicketsModel.Instance.startlocation = city;
                BuyTicketsModel.Instance.stoplocation = "沈阳";
                TimeManager.instance.SetNormalSpeed();
                airline.gameObject.SetActive(false);
                EventHappenManager.Instance.EveryLocation(city);
                UserTicketsModel.Instance.going = false;

            }
        }
        
    }

    public void DisplayMessage(BaseAccident data)
    {
        if (data.GetType() == typeof(Accident))
        {
            Lucky.LuckyUtils.Log("destroy");
            Accident accident = data as Accident;
            Lucky.LuckyUtils.Log(accident.type+"delete acciednt "+accident.location);
            if(accident.type == AccidentType.airport)
            {
                if(AirPlanewarndic.ContainsKey(accident.location))
                {
                    Destroy(AirPlanewarndic[accident.location].gameObject);
                    AirPlanewarndic.Remove(accident.location);
                }
                
            }
            else
            {
                if(Trainwarndic.ContainsKey(accident.location))
                {
                    Destroy(Trainwarndic[accident.location].gameObject);
                    Trainwarndic.Remove(accident.location);
                }
                
            }
            
        }
    }

    public void ShowAccidentMessage(BaseAccident data)
    {
        if (data.GetType() == typeof(Accident))
        {
            Accident accident = data as Accident;
            Debug.Log("show accident " + accident.location);
            if(accident.type == AccidentType.airport)
            {
                if (AirPlanewarndic.ContainsKey(accident.location))
                {
                    AirPlanewarndic[accident.location].AccidentMessage = accident;
                }
                else
                {
                    Debug.Log("add accident " + accident.location);
                    GameObject warningPrefab = PrefabManager.Instance.GetPrefabs(Prefabs.Warning);
                    GameObject warningObj = Instantiate(warningPrefab);
                    LuckyUtils.MakeIndentity(warningObj.transform);
                    WarningView wv = warningObj.GetComponent<WarningView>();
                    wv.AccidentMessage = accident;
                    AirPlanewarndic.Add(accident.location, wv);
                    warningObj.transform.SetParent(transform);
                    warningObj.SetActive(true);
                    LuckyUtils.MakeIndentity(warningObj.transform);
                }
            }
            else
            {
                if (Trainwarndic.ContainsKey(accident.location))
                {
                    Trainwarndic[accident.location].AccidentMessage = accident;
                }
                else
                {
                    Debug.Log("add accident " + accident.location);
                    GameObject warningPrefab = PrefabManager.Instance.GetPrefabs(Prefabs.Warning);
                    GameObject warningObj = Instantiate(warningPrefab);
                    LuckyUtils.MakeIndentity(warningObj.transform);
                    WarningView wv = warningObj.GetComponent<WarningView>();
                    wv.AccidentMessage = accident;
                    Trainwarndic.Add(accident.location, wv);
                    warningObj.transform.SetParent(transform);
                    warningObj.SetActive(true);
                    LuckyUtils.MakeIndentity(warningObj.transform);
                }
            }
            
        }
        else if (data.GetType() == typeof(AccidentWarning))
        {
            AccidentWarning warning = data as AccidentWarning;
            Debug.Log("show accident warning " + warning.location);
            if(warning.type== AccidentType.airport)
            {
                if (AirPlanewarndic.ContainsKey(warning.location))
                {
                    AirPlanewarndic[warning.location].AccidentMessage = warning;
                }
                else
                {
                    Debug.Log("add accident warning " + warning.location);
                    GameObject warningPrefab = PrefabManager.Instance.GetPrefabs(Prefabs.Warning);
                    GameObject warningObj = Instantiate(warningPrefab);
                    LuckyUtils.MakeIndentity(warningObj.transform);
                    WarningView wv = warningObj.GetComponent<WarningView>();
                    wv.AccidentMessage = warning;
                    AirPlanewarndic.Add(warning.location, wv);
                    warningObj.transform.SetParent(transform);
                    warningObj.SetActive(true);
                    LuckyUtils.MakeIndentity(warningObj.transform);
                }
            }
            else
            {
                if (Trainwarndic.ContainsKey(warning.location))
                {
                    Trainwarndic[warning.location].AccidentMessage = warning;
                }
                else
                {
                    Debug.Log("add accident warning " + warning.location);
                    GameObject warningPrefab = PrefabManager.Instance.GetPrefabs(Prefabs.Warning);
                    GameObject warningObj = Instantiate(warningPrefab);
                    LuckyUtils.MakeIndentity(warningObj.transform);
                    WarningView wv = warningObj.GetComponent<WarningView>();
                    wv.AccidentMessage = warning;
                    Trainwarndic.Add(warning.location, wv);
                    warningObj.transform.SetParent(transform);
                    warningObj.SetActive(true);
                    LuckyUtils.MakeIndentity(warningObj.transform);
                }
            }
            
            
        }
        
        
    }

    public void AirPlaneFly(TicketParam tp)
    {
        TicketsController.Instance.DeleteTickets(tp.rt.GetTicketId());
        if (UserTicketsModel.Instance.where==Where.City &&  tp.rt.GetRoutineStartNode().Contains(UserTicketsModel.Instance.city))
        {
            AudioManager.Instance.PlayMusic(Audios.AirPlaneClip);
            UserTicketsModel.Instance.where = Where.AirPlane;
            Lucky.LuckyUtils.Log("airplane fly" + tp.rt.GetBeginTime());
            Vector3 startPos = Vector3.zero;
            Lucky.LuckyUtils.Log("start " + tp.rt.GetRoutineStartNode());
            Lucky.LuckyUtils.Log("stop " + tp.rt.GetEndNode());

            string start = tp.rt.GetRoutineStartNode();
            string stop = tp.rt.GetEndNode();
            start = GetCityString(start);
            stop = GetCityString(stop);
            LocationsModel.cityslocation.TryGetValue(start, out startPos);
            Vector3 stopPos = Vector3.zero;
            LocationsModel.cityslocation.TryGetValue(stop, out stopPos);
            Lucky.LuckyUtils.Log("start stop pos " + startPos + " " + stopPos);
            airline.Show(startPos, stopPos);

            
            dst = stop;

            DateTime starttime = tp.rt.GetBeginTime();
            DateTime stoptime = tp.rt.GetEndTime();
            TimeSpan ts = stoptime - starttime;

            ticketid = tp.rt.GetTicketId();
            TicketsController.Instance.DeleteTickets(ticketid);

            traveltime = (float)ts.TotalMinutes;
            Lucky.LuckyUtils.Log("travel time " + ts.TotalMinutes);
            double realtime = traveltime / TimeManager.instance.TimeSpeed;
            Lucky.LuckyUtils.Log("realtime " + realtime);
            animationName = start + "To" + stop;

            animator = airplane.GetComponent<Animator>();
            animator.Stop();
            AnimationClip clip = FindClip(animator, animationName);
            if (clip != null)
            {

                cliptime = clip.length;
                double speed = cliptime / realtime;
                Lucky.LuckyUtils.Log("speed " + speed);
                airplane.SetActive(true);
                animator.Play(animationName);
                animator.speed = (float)speed;
            }

            UserTicketsModel.Instance.going = true;
        }
        else
        {
            InfoView.Show(new InfoMessage("你当前不在出发城市，该机票"+ tp.rt.GetTicketName() + "作废！", "亏大了!"));
        }
    }

    public void TrainGo(TicketParam tp)
    {
        if(tp==null)
            Debug.Log("tp is null");
        TicketsController.Instance.DeleteTickets(tp.rt.GetTicketId());
        if (UserTicketsModel.Instance.where == Where.City && tp.rt.GetRoutineStartNode().Contains(UserTicketsModel.Instance.city))
        {
            AudioManager.Instance.PlayMusic(Audios.RailwayClip);
            UserTicketsModel.Instance.where = Where.Train;
            Lucky.LuckyUtils.Log("train go" + tp.rt.GetBeginTime());
            string start = tp.rt.GetRoutineStartNode();
            string stop = tp.rt.GetEndNode();
            dst = stop;

            DateTime starttime = tp.rt.GetBeginTime();
            DateTime stoptime = tp.rt.GetEndTime();
            TimeSpan ts = stoptime - starttime;

            ticketid = tp.rt.GetTicketId();
            TicketsController.Instance.DeleteTickets(ticketid);

            traveltime = (float)ts.TotalMinutes;
            double realtime = traveltime / TimeManager.instance.TimeSpeed;
            start = GetCityString(start);
            stop = GetCityString(stop);
            animationName = start + "-" + stop + "_Train";
            Lucky.LuckyUtils.Log("train " + animationName);

            animator = train.GetComponent<Animator>();
            AnimationClip clip = FindClip(animator, animationName);
            if (clip != null)
            {
                cliptime = clip.length;
                double speed = cliptime / realtime;
                train.SetActive(true);
                animator.Play(animationName, 0, 0);
                animator.speed = (float)speed;
            }

            UserTicketsModel.Instance.going = true;
        }
        else
        {
            InfoView.Show(new InfoMessage("你当前不在出发城市，该火车票"+tp.rt.GetTicketName()+"作废！", "亏大了!"));
        }

    }

    private AnimationClip FindClip(Animator animator, string name)
    {
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;

        AnimationClip[] clips = ac.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == name)
                return clip;
        }
        return null;
    }

    public void SetAnimatorSpeed()
    {
        if(airplane.gameObject.activeSelf || train.gameObject.activeSelf)
        {
            if(TimeManager.instance.TimeSpeed!=0)
            {
                AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
                float walktime = asi.normalizedTime * cliptime;
                float travelwalktime = asi.normalizedTime * traveltime;
                float realtime = travelwalktime / TimeManager.instance.TimeSpeed;
                float speed = walktime / realtime;
                animator.speed = speed;
            }
            else
            {
                animator.speed = 0;
            }
        }
    }
    
    public void AnimatorStop()
    {
        
        animator.Stop();
    }

    public void ShowLocation(string str)
    {
        location.SetActive(true);
        RectTransform cityrt = FindPlace(str);
        location.GetComponent<RectTransform>().anchoredPosition3D = cityrt.anchoredPosition3D;
    }

    public void HideLocation()
    {
        location.SetActive(false);
    }

    public RectTransform FindPlace(string str)
    {
        for(int i=0;i<transform.childCount;i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (str.Contains(child.name))
                return child.GetComponent<RectTransform>() ; 
        }
        return null;
    }

    public string FindPlaceName(string str)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (str.Contains(child.name))
                return child.name;
        }
        return null;
    }

    public RectTransform FindRailway(int j)
    {
        string num = j + "";
        for(int i=0;i<transform.childCount;i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.name == num)
                return child.GetComponent<RectTransform>();
        }
        return null;
    }

    public string GetCityString(string st)
    {
        foreach(string city in citys)
        {
            if (st.Contains(city))
                return city;
        }
        return null;
    }

    
}
