using UnityEngine;
using System.Collections;
using System;
using Lucky;


public class EventHappenManager : BaseInstance<EventHappenManager> {

    public IEnumerator Init()
    {
        yield return null;
    }

    public void EveryThirtyMinutes(DateTime dt)
    {
        Debug.Log("every third minutes" + dt);
        OnePageNoteBook data = new OnePageNoteBook();
       // MessageBus.Post(data);
    }

    public void EveryLocation(string dst)
    {
        Debug.Log("every location " + dst);
        OnePageNoteBook data = new OnePageNoteBook();
        //MessageBus.Post(data);

    }

}
