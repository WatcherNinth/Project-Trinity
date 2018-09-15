﻿using UnityEngine;
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

    private Dictionary<int, WarningView> warndic = new Dictionary<int, WarningView>();

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
    }

    private void Update()
    {
        if(train.activeSelf)
        {
            AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
            if ((asi.normalizedTime > 1.0f) && (asi.IsName(animationName)))
            {
                Debug.Log("train arrive");
                train.SetActive(false);
                UserTicketsModel.Instance.where = Where.City;
                UserTicketsModel.Instance.city = dst;
                EventHappenManager.Instance.EveryLocation(dst);
                TimeManager.instance.TimeSpeed = 1.0f;
            }
        }

        if(airplane.activeSelf)
        {
            AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
            if ((asi.normalizedTime > 1.0f) && (asi.IsName(animationName)))
            {
                Debug.Log("airplane arrive");
                airplane.SetActive(false);
                
                UserTicketsModel.Instance.where = Where.City;
                UserTicketsModel.Instance.city = dst;
                EventHappenManager.Instance.EveryLocation(dst);
                TimeManager.instance.TimeSpeed = 1.0f;
                airline.gameObject.SetActive(false);
            }
        }
        
    }

    public void DisplayMessage(BaseAccident data)
    {
        if (data.GetType() == typeof(Accident))
        {
            Debug.Log("destroy");
            Accident accident = data as Accident;
            Destroy(warndic[accident.location].gameObject);
            warndic.Remove(accident.location);
        }
    }

    public void ShowAccidentMessage(BaseAccident data)
    {
        if (data.GetType() == typeof(Accident))
        {
            Accident accident = data as Accident;
            if (warndic.ContainsKey(accident.location))
            {
                warndic[accident.location].AccidentMessage = accident;
            }
            else
            {
                GameObject warningPrefab = PrefabManager.Instance.GetPrefabs(Prefabs.Warning);
                GameObject warningObj = Instantiate(warningPrefab);
                LuckyUtils.MakeIndentity(warningObj.transform);
                WarningView wv = warningObj.GetComponent<WarningView>();
                wv.AccidentMessage = accident;
                warndic.Add(accident.location, wv);
                warningObj.transform.SetParent(transform);
                warningObj.SetActive(true);
                LuckyUtils.MakeIndentity(warningObj.transform);
            }
        }
        else if (data.GetType() == typeof(AccidentWarning))
        {
            AccidentWarning warning = data as AccidentWarning;
            if (warndic.ContainsKey(warning.location))
            {
                warndic[warning.location].AccidentMessage = warning;
            }
            else
            {
                GameObject warningPrefab = PrefabManager.Instance.GetPrefabs(Prefabs.Warning);
                GameObject warningObj = Instantiate(warningPrefab);
                LuckyUtils.MakeIndentity(warningObj.transform);
                WarningView wv = warningObj.GetComponent<WarningView>();
                wv.AccidentMessage=warning;
                warndic.Add(warning.location, wv);
                warningObj.transform.SetParent(transform);
                warningObj.SetActive(true);
                LuckyUtils.MakeIndentity(warningObj.transform);
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
            Debug.Log("airplane fly" + tp.rt.GetBeginTime());
            Vector3 startPos = Vector3.zero;
            Debug.Log("start " + tp.rt.GetRoutineStartNode());
            Debug.Log("stop " + tp.rt.GetEndNode());

            string start = tp.rt.GetRoutineStartNode();
            string stop = tp.rt.GetEndNode();
            start = GetCityString(start);
            stop = GetCityString(stop);
            LocationsModel.cityslocation.TryGetValue(start, out startPos);
            Vector3 stopPos = Vector3.zero;
            LocationsModel.cityslocation.TryGetValue(stop, out stopPos);
            Debug.Log("start stop pos " + startPos + " " + stopPos);
            airline.Show(startPos, stopPos);

            
            dst = stop;

            DateTime starttime = tp.rt.GetBeginTime();
            DateTime stoptime = tp.rt.GetEndTime();
            TimeSpan ts = stoptime - starttime;

            ticketid = tp.rt.GetTicketId();
            TicketsController.Instance.DeleteTickets(ticketid);

            traveltime = (float)ts.TotalMinutes;
            Debug.Log("travel time " + ts.TotalMinutes);
            double realtime = traveltime / TimeManager.instance.TimeSpeed;
            Debug.Log("realtime " + realtime);
            animationName = start + "To" + stop;

            animator = airplane.GetComponent<Animator>();
            animator.Stop();
            AnimationClip clip = FindClip(animator, animationName);
            if (clip != null)
            {

                cliptime = clip.length;
                double speed = cliptime / realtime;
                Debug.Log("speed " + speed);
                airplane.SetActive(true);
                animator.Play(animationName);
                animator.speed = (float)speed;
            }
        }
        else
        {
            InfoView.Show(new InfoMessage("你当前不在出发城市，该机票"+ tp.rt.GetTicketName() + "作废！", "亏大了!"));
        }
    }

    public void TrainGo(TicketParam tp)
    {
        TicketsController.Instance.DeleteTickets(tp.rt.GetTicketId());
        if (UserTicketsModel.Instance.where == Where.City && tp.rt.GetRoutineStartNode().Contains(UserTicketsModel.Instance.city))
        {
            AudioManager.Instance.PlayMusic(Audios.RailwayClip);
            UserTicketsModel.Instance.where = Where.Train;
            Debug.Log("train go" + tp.rt.GetBeginTime());
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
            animationName = start + "To" + stop;

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
            AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
            float walktime = asi.normalizedTime * cliptime;
            float travelwalktime = asi.normalizedTime * traveltime;
            float realtime = travelwalktime / TimeManager.instance.TimeSpeed;
            float speed = walktime / realtime;
            animator.speed = speed;
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
            if (child.name == str)
                return child.GetComponent<RectTransform>() ; 
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
