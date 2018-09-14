using UnityEngine;
using System.Collections;
using System;
using Lucky;
using System.Collections.Generic;

public class MapTrafficView : MonoBehaviour {

    public AirLineView airline;
    public GameObject airplane;
    public GameObject train;

    private Animator animator;
    private float traveltime;
    private float cliptime;
    private string animationName;
    private int ticketid;
    private string dst;

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
            }
        }
        
    }

    public void DisplayMessage(BaseAccident data)
    {
        if (data.GetType() == typeof(Accident))
        {
            Accident accident = data as Accident;
            Destroy(warndic[accident.location]);
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
        UserTicketsModel.Instance.where = Where.AirPlane;
        Debug.Log("airplane fly"+ tp.rt.GetBeginTime());
        Vector3 startPos = Vector3.zero;
        Debug.Log("start " + tp.rt.GetRoutineStartNode());
        Debug.Log("stop " + tp.rt.GetEndNode());
        LocationsModel.cityslocation.TryGetValue(tp.rt.GetRoutineStartNode(), out startPos);
        Vector3 stopPos = Vector3.zero;
        LocationsModel.cityslocation.TryGetValue(tp.rt.GetEndNode(), out stopPos);
        Debug.Log("start stop pos "+startPos + " " + stopPos);
        airline.Show(startPos, stopPos);

        string start = tp.rt.GetRoutineStartNode();
        string stop = tp.rt.GetEndNode();
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
        AnimationClip clip = FindClip(animator, animationName);
        if (clip != null)
        {
            
            cliptime = clip.length;
            double speed = cliptime / realtime;
            Debug.Log("speed " + speed);
            airplane.SetActive(true);
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
        dst = stop;

        DateTime starttime = tp.rt.GetBeginTime();
        DateTime stoptime = tp.rt.GetEndTime();
        TimeSpan ts=stoptime - starttime;

        ticketid = tp.rt.GetTicketId();
        TicketsController.Instance.DeleteTickets(ticketid);

        traveltime = (float)ts.TotalMinutes;
        double realtime = traveltime / TimeManager.instance.TimeSpeed;
        animationName = start + "To" + stop;

        animator = train.GetComponent<Animator>();
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
}
