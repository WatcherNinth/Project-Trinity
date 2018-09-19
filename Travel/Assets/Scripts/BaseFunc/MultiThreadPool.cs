using UnityEngine;
using System.Collections;
using Lucky;
using System;
using System.Threading;
using System.Collections.Generic;

namespace Lucky
{
    public class MultiYield : CustomYieldInstruction
    {
        public List<TrafficMessage> result=null;

        private bool isOver=false;
        public override bool keepWaiting
        {
            get { return !isOver; }
        }

        public void SetBool(bool value)
        {
            isOver = value;
        }
    }

    public class ThreadParam
    {
        public MultiYield my;
        public Func<System.Object, List<TrafficMessage>> callback;
        public static object objlock = new object();

        public void Callback(System.Object param)
        {
            lock(objlock)
            {
                int time = DateTime.Now.Millisecond;
                Debug.Log("start callback ");
                if (callback != null)
                    my.result = callback(param);
                my.SetBool(true);
                Debug.Log("last time " + (DateTime.Now.Millisecond - time));
            }
            
        }

        public ThreadParam(MultiYield tmy, Func<System.Object, List<TrafficMessage>> tcallback)
        {
            my = tmy;
            callback = tcallback;
        }
    }

    public class MultiThreadPool
    {
        public static MultiYield AddNewMission(System.Object param, Func<System.Object, List<TrafficMessage>> callback)
        {
            MultiYield my = new MultiYield();
            ThreadParam tp = new ThreadParam(my, callback);
            ThreadPool.QueueUserWorkItem(new WaitCallback(tp.Callback),param);
            
            return my;
        }
    }
}

