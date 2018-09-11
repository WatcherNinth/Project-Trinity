﻿using UnityEngine;
using System.Collections;
using System;
using Lucky;
using System.Collections.Generic;

public class MapTrafficView : MonoBehaviour {

    public AirLineView airline;
    public GameObject airplane;
    public GameObject train;
    public Animator animator;

    private float traveltime;
    private float cliptime;
    private string animationName;
    private int ticketid;

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
                TicketsController.Instance.DeleteTickets(ticketid);
                UserTicketsModel.Instance.where = Where.City;
                UserTicketsModel.Instance.city = "";
            }
        }

        if(airplane.activeSelf)
        {
            AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
            if ((asi.normalizedTime > 1.0f) && (asi.IsName(animationName)))
            {
                Debug.Log("airplane arrive");
                airplane.SetActive(false);
                TicketsController.Instance.DeleteTickets(ticketid);
                UserTicketsModel.Instance.where = Where.City;
                UserTicketsModel.Instance.city = "";
            }
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
            }
            
        }
        
        
    }

    public void AirPlaneFly(TicketParam tp)
    {
        UserTicketsModel.Instance.where = Where.AirPlane;
        Debug.Log("airplane fly"+ tp.rt.GetBeginTime());
        Vector3 startPos = Vector3.zero;
        LocationsModel.cityslocation.TryGetValue(tp.rt.GetRoutineStartNode(), out startPos);
        Vector3 stopPos = Vector3.zero;
        LocationsModel.cityslocation.TryGetValue(tp.rt.GetEndNode(), out stopPos);
        airline.Show(startPos, stopPos);

        string start = tp.rt.GetRoutineStartNode();
        string stop = tp.rt.GetEndNode();

        DateTime starttime = tp.rt.GetBeginTime();
        DateTime stoptime = tp.rt.GetEndTime();
        TimeSpan ts = stoptime - starttime;

        ticketid = tp.rt.GetTicketId();

        traveltime = (float)ts.TotalMinutes;
        double realtime = traveltime / TimeManager.instance.TimeSpeed;
        animationName = "Airplane" + start + "To" + stop;

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

    public void TrainGo(TicketParam tp)
    {
        UserTicketsModel.Instance.where = Where.Train;
        Debug.Log("train go" + tp.rt.GetBeginTime());
        string start = tp.rt.GetRoutineStartNode();
        string stop = tp.rt.GetEndNode();

        DateTime starttime = tp.rt.GetBeginTime();
        DateTime stoptime = tp.rt.GetEndTime();
        TimeSpan ts=stoptime - starttime;

        ticketid = tp.rt.GetTicketId();

        traveltime = (float)ts.TotalMinutes;
        double realtime = traveltime / TimeManager.instance.TimeSpeed;
        animationName = start + "To" + stop;

        AnimationClip clip = FindClip(animator, animationName);
        if(clip!=null)
        {
            cliptime = clip.length;
            double speed = cliptime / realtime;
            train.SetActive(true);
            animator.Play(animationName, 0, 0);
            animator.speed = (float)speed;
        }

    }

    public void ShowAccident(BaseAccident ba)
    {
        if(ba.GetType() == typeof(Accident))
        {
            
        }
        else if(ba.GetType() == typeof(AccidentWarning))
        {

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
        AnimatorStateInfo asi=animator.GetCurrentAnimatorStateInfo(0);
        float walktime = asi.normalizedTime * cliptime;
        float travelwalktime = asi.normalizedTime * traveltime;
        float realtime = travelwalktime / TimeManager.instance.TimeSpeed;
        float speed = walktime / realtime;
        animator.speed = speed;
    }
    
    public void AnimatorStop()
    {
        animator.Stop();
    }
}
