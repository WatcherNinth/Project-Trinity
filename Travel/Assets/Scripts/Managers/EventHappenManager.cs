using UnityEngine;
using System.Collections;
using System;
using Lucky;
using System.Collections.Generic;

public class EventHappenManager : BaseInstance<EventHappenManager>
{

    public List<Event> EventList = EventUtil.Instance.GetAllEvents().data;
    public List<Event> RandomCityList = new List<Event>();
    public List<Event> RandomTrainList = new List<Event>();
    public List<Event> RandomFlightList = new List<Event>();
    List<string> ImageList = new List<string>();
    static System.Random rnd = new System.Random();

    public IEnumerator Init()
    {
        //OnePageNoteBook data = new OnePageNoteBook();
        RandomCityList = EventList.FindAll(x => x.condition == "City");
        RandomTrainList = EventList.FindAll(x => x.condition == "Train");
        //for (int i = 0; i <= 10; i++) ImageList.Add(i.ToString());
        ImageList.Add(Sprites.book1);
        ImageList.Add(Sprites.book2);
        ImageList.Add(Sprites.book3);
        ImageList.Add(Sprites.book4);
        ImageList.Add(Sprites.book5);
        ImageList.Add(Sprites.book6);
        ImageList.Add(Sprites.book7);
        ImageList.Add(Sprites.book8);
        ImageList.Add(Sprites.book1);
        ImageList.Add(Sprites.book2);
        yield return null;
    }

    public void EveryThirtyMinutes(DateTime dt)
    {
        
        Event target=new Event();
        //get current status
        Debug.Log("where "+UserTicketsModel.Instance.where);
        Debug.Log("Count "+RandomCityList.Count);
        switch (UserTicketsModel.Instance.where)
        {
            case Where.City:
                {
                    target = RandomCityList[rnd.Next(0, RandomCityList.Count)];
                    RandomCityList.Remove(target);
                    break;
                }
            case Where.Train:
                {
                    target = RandomTrainList[rnd.Next(0, RandomTrainList.Count)];
                    RandomTrainList.Remove(target);
                    break;
                }
            case Where.AirPlane:
                {
                    break;
                }
        }
        
        //random a event ,pushout 
        //then del it from xxxxList
        OnePageNoteBook data = new OnePageNoteBook(target,TimeManager.instance.NowTime,ImageList[target.id]);
        MessageBus.Post(data);
    }

    public void EveryLocation(string dst)
    {
        Debug.Log("every location " + dst);
        Event target = EventList.Find(x => x.condition == dst);
        OnePageNoteBook data = new OnePageNoteBook(target, TimeManager.instance.NowTime, ImageList[target.id]);
        MessageBus.Post(data);

        if (dst == "沈阳")
        {
            InfoView.Show(new InfoMessage("到家了", "消息！"));
            TimeManager.instance.TimeSpeed = 0.0f;
        }

        MapTrafficView.instance.ShowLocation(dst);

    }

}