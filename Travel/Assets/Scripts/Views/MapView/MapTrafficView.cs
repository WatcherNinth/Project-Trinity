using UnityEngine;
using System.Collections;
using System;

public class MapTrafficView : MonoBehaviour {

    public AirLineView airline;
    public GameObject airplane;

    public Animator animator;
    private AnimatorOverrideController aoc;
    public double speed;

    private static MapTrafficView _instance;
    public static MapTrafficView instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

	// Use this for initialization
	void Start () {
        
        
    }
	
	// Update is called once per frame
	void Update () {
        animator.speed = (float)speed;
        if (speed == 0.5)
            animator.Stop();
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

        double realtime = ts.TotalMinutes / TimeManager.instance.TimeSpeed;
        string animationname = start + "To" + stop;

        AnimationClip clip = FindClip(animator, animationname);
        if(clip!=null)
        {
            speed = clip.length / realtime;
        }
        animator.Play(animationname, 0, 0);
        animator.speed = (float)speed;


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
}
