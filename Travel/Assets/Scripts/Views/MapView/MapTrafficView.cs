using UnityEngine;
using System.Collections;
using System;

public class MapTrafficView : MonoBehaviour {

    public AirLineView airline;
    public GameObject airplane;
    public GameObject train;
    public Animator animator;

    private float traveltime;
    private float cliptime;
    private string animationName;
    private int ticketid;

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
    }

    private void Update()
    {
        if(train.activeSelf)
        {
            AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
            if ((asi.normalizedTime > 1.0f) && (asi.IsName(animationName)))
            {
                Debug.Log("arrive");
                train.SetActive(false);
                TicketsController.Instance.DeleteTickets(ticketid);
            }
        }
        
    }

    public void AirPlaneFly(TicketParam tp)
    {
        Debug.Log("airplane fly"+ tp.rt.GetBeginTime());
        Vector3 start = Vector3.zero;
        LocationsModel.cityslocation.TryGetValue(tp.rt.GetRoutineStartNode(), out start);
        Vector3 stop = Vector3.zero;
        LocationsModel.cityslocation.TryGetValue(tp.rt.GetEndNode(), out stop);
        airline.Show(start, stop);
    }

    public void TrainGo(TicketParam tp)
    {
        Debug.Log("train go" + tp.rt.GetBeginTime());
        string start = tp.rt.GetRoutineStartNode();
        string stop = tp.rt.GetEndNode();

        start = "Shanghai";
        stop = "Beijing";
        //DateTime starttime = tp.rt.GetBeginTime();
        //DateTime stoptime = tp.rt.GetEndTime();
        //TimeSpan ts=stoptime - starttime;

        DateTime starttime = new DateTime(2018, 1, 6, 10, 0, 0);
        DateTime stoptime = new DateTime(2018, 1, 6, 12, 0, 0);
        TimeSpan ts = stoptime - starttime;

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
