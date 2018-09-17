using UnityEngine;
using System.Collections;
using System;
using Lucky;
using System.Collections.Generic;

public class EventHappenManager : BaseInstance<EventHappenManager>
{

    public List<Event> EventList;
    public List<Event> RandomCityList = new List<Event>();
    public List<Event> RandomTrainList = new List<Event>();
    public List<Event> RandomFlightList = new List<Event>();
    List<string> ImageList = new List<string>();
    static System.Random rnd = new System.Random();

    public IEnumerator Init()
    {
        //OnePageNoteBook data = new OnePageNoteBook();
        EventList = EventUtil.Instance.GetAllEvents().data;
        RandomCityList = EventList.FindAll(x => x.condition == "City");
        RandomTrainList = EventList.FindAll(x => x.condition == "Train");
        Debug.Log("city:"+RandomCityList.Count+"Train:"+RandomTrainList.Count);
        yield return null;
        //for (int i = 0; i <= 10; i++) ImageList.Add(i.ToString());
        ImageList.Add(Sprites.book1);
        ImageList.Add(Sprites.book5);
        ImageList.Add(Sprites.book3);
        ImageList.Add(Sprites.book4);
        ImageList.Add(Sprites.book8);
        ImageList.Add(Sprites.book6);
        ImageList.Add(Sprites.book7);
        ImageList.Add(Sprites.book2);
        ImageList.Add(Sprites.book8);
        ImageList.Add(Sprites.book1);
        //ImageList.Add(Sprites.book2);
    }

    public void EveryThirtyMinutes(DateTime dt)
    {
        
        Event target=null;
        //get current status
        Debug.Log("where "+UserTicketsModel.Instance.where);
        Debug.Log("Count "+RandomCityList.Count);
        switch (UserTicketsModel.Instance.where)
        {
            case Where.City:
                {
                    if(RandomCityList.Count!=0)
                    {
                        target = RandomCityList[rnd.Next(0, RandomCityList.Count)];
                        if (target != null)
                            RandomCityList.Remove(target);
                    }
                    break;
                }
            case Where.Train:
                {
                    if(RandomTrainList.Count!=0)
                    {
                        int temp = rnd.Next(0, RandomTrainList.Count);
                        target = RandomTrainList[temp];
                        if (target != null)
                            RandomTrainList.Remove(target);
                    } 
                    break;
                }
            case Where.AirPlane:
                {
                    break;
                }
        }
        

        if(target!=null)
        {
            OnePageNoteBook data = new OnePageNoteBook(target, TimeManager.instance.NowTime, ImageList[target.id]);
            MessageBus.Post(data);
        }
        
    }

    public void EveryLocation(string dst)
    {
        Debug.Log("every location " + dst);

        if (dst == "沈阳")
        {
            InfoView.Show(new InfoMessage("到家了", "消息！"));
            TimeManager.instance.TimeSpeed = 0.0f;
        }

        MapTrafficView.instance.ShowLocation(dst);
        Event target = EventList.Find(x => x.condition == dst);

        if(target!=null)
        {
            OnePageNoteBook data = new OnePageNoteBook(target, TimeManager.instance.NowTime, ImageList[target.id]);
            MessageBus.Post(data);
        }
        

    }

}