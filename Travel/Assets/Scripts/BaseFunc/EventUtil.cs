using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Lucky;
using Mono.Data.Sqlite;

public class Events {
   public List<Event> data;
}


[Serializable]
public class Content
{
    public string condition;
    public string text;
}

[Serializable]
public class Event
{
    public int id;
    public string type;
    public string condition;
    public List<Content> content;

}
public class EventUtil : BaseInstance<EventUtil> {
    private string content = "";

    public EventUtil()
    {
        TextAsset text = TextManager.Instance.GetText(Texts.Event);
        content = text.text;

        Lucky.LuckyUtils.Log(text.text);
    }

    public Events GetAllEvents()
    {
        List<Event> events = new List<Event>();
        // Lucky.LuckyUtils.Log(this.content);
        Events res = JsonUtility.FromJson <Events>(content);
        return res;
    }

}
