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
        OnePageNoteBook data = new OnePageNoteBook(dt, Sprites.ticket_airplane, "hahahaha");
        data.buttontext.Add("eeee");
        data.buttontext.Add("ok");
        data.buttontext.Add("fuck you");
        data.finaltext.Add("dont laught");
        data.finaltext.Add("hahaha too");
        data.finaltext.Add("asshole");
        MessageBus.Post(data);
    }

    public void EveryLocation(string dst)
    {
        Debug.Log("every location " + dst);
        //MessageBus.Post(data);

    }

}
