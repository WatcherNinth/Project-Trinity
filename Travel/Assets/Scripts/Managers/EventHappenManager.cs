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
    bool TicketFlag=true;


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
        Lucky.LuckyUtils.Log("where "+UserTicketsModel.Instance.where);
        Lucky.LuckyUtils.Log("Count "+RandomCityList.Count);
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
                    if(RandomTrainList.Count!=0 && TicketFlag)
                    {
                        int temp = rnd.Next(0, RandomTrainList.Count);
                        target = RandomTrainList[temp];
                        if (target != null)
                            RandomTrainList.Remove(target);
                        TicketFlag = false;
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
        Lucky.LuckyUtils.Log("every location " + dst);
        TicketFlag = true;
        if (dst == "沈阳")
        {
            InfoView.Show(new InfoMessage("到家了", "消息！"));
            TimeManager.instance.StopTimeManager();
        }

        MapTrafficView.instance.ShowLocation(dst);
        Event target = EventList.Find(x => x.condition == dst);

        if(target!=null)
        {
            OnePageNoteBook data = new OnePageNoteBook(target, TimeManager.instance.NowTime, ImageList[target.id]);
            MessageBus.Post(data);
        }
        switch (dst)
        {
            case "上海":
                //AccidentGenerator.Instance.CreateAccident(AccidentType.airport, 8, 999, TimeManager.instance.NowTime, 4);
                break;
        }

    }

}