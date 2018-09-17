
using Lucky;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AccidentType
{
    rail,
    airport
}

public class BaseAccident
{
    public DateTime starttime;
}

public class Accident : BaseAccident
{
    public int location;
    public AccidentType type;
    public int duration;
    public AccidentText text;
}
public class AccidentText
{
    public AccidentType type;
    public string title;
    public string description;
}
public class AccidentWarning : BaseAccident
{
    public int location;
    public AccidentType type;
    public DateTime Accidentstarttime;
    public int min, max;

}
public class AccidentGenerator : BaseInstance<AccidentGenerator>
{

    public List<Accident> AccidentList;

    List<int> RailList, AirportList;
    int AirportAccident = 8;
    int RailAccident = 8;
    DateTime InitTime = GameModel.Instance.Start.AddMinutes(5);
    List<AccidentText> accidentTexts;
    List<AccidentText> AirportAccidentTexts;
    List<AccidentText> RailAccidentTexts;

    //AccidentWarning property
    public List<AccidentWarning> AccidentWarningList;
    int[] AccidentWarningAccurency = { 60, 180, 300 };

    StringProcessScript stringProcess = new StringProcessScript();

    public void CreateAccident(AccidentType type, int location, int duration, DateTime starttime, int text)
    {
        Accident accident = new Accident
        {
            duration = duration,
            location = location,
            starttime = starttime,
            type = type,
            text = accidentTexts[text]
        };
        accident = stringProcess.AccidentStringProcess(accident);
        PushAccident(accident);
    }

    static System.Random rnd = new System.Random();
    public void AccidentGenerate()
    {
        
        Accident accident = new Accident();
        for (int i = 0; i < AirportAccident; i++)
        {
            accident.type = AccidentType.airport;
            accident.location = AirportList[rnd.Next(0, AirportList.Count)];
            AirportList.RemoveAll(x => x == accident.location);
            accident.duration = rnd.Next(3, 31) * 10;
            accident.starttime = InitTime.AddMinutes(rnd.Next(0, 901));
            accident.text = AirportAccidentTexts[rnd.Next(0, AirportAccidentTexts.Count)];
            accident = stringProcess.AccidentStringProcess(accident);
            AccidentList.Add(accident);
        }
        for (int i = 0; i < RailAccident; i++)
        {
            accident.type = AccidentType.rail;
            accident.location = RailList[rnd.Next(0, RailList.Count)];
            RailList.RemoveAll(x => x == accident.location);
            accident.duration = rnd.Next(3, 31) * 10;
            accident.starttime = InitTime.AddMinutes(rnd.Next(0, 901));
            accident.text = RailAccidentTexts[rnd.Next(0, RailAccidentTexts.Count)];
            accident = stringProcess.AccidentStringProcess(accident);
            AccidentList.Add(accident);
        }
        
        
        PushAccidentList();
    }
    void PushAccident(Accident item)
    {
        TimeManager.instance.AddAccidentExecute(item, HandleAccident, false);
        Accident eitem = item;
        eitem.starttime = eitem.starttime.AddMinutes(item.duration);
        TimeManager.instance.AddAccidentExecute(eitem, null, true);
    }

    void PushAccidentList()
    {
        foreach (Accident item in AccidentList)
        {
            CreateAccidentWarning(item);
            TimeManager.instance.AddAccidentExecute(item, HandleAccident,false);
            Accident eitem = item;
            eitem.starttime = eitem.starttime.AddMinutes(item.duration);
            TimeManager.instance.AddAccidentExecute(eitem, null, true);

            //timemanager callback
        }
        foreach (AccidentWarning item in AccidentWarningList)
        {
            //timemanager callback
            TimeManager.instance.AddAccidentExecute(item, null);
        }
    }
    void CreateAccidentWarning(Accident accident)
    {
        
        AccidentWarning warning = new AccidentWarning();
        System.Random rnd = new System.Random();
        int rndNum;
        for (int i = 0; i < AccidentWarningAccurency.Length; i++)
        {
            warning.starttime = accident.starttime.AddMinutes(-AccidentWarningAccurency[i]);
            warning.location = accident.location;
            warning.type = accident.type;
            warning.Accidentstarttime = accident.starttime;
            rndNum = rnd.Next(0, AccidentWarningAccurency[i] / 2);
            warning.min = accident.duration - rndNum;
            if (warning.min < 0) warning.min = accident.duration;
            warning.max = accident.duration + AccidentWarningAccurency[i] - rndNum;
            if (DateTime.Compare(warning.starttime, InitTime) < 0)
            {
                Debug.Log("too early warning " + warning.starttime+" "+accident.starttime);
                warning.starttime = InitTime;
                AccidentWarningList.Add(warning);
                break;
            }
            AccidentWarningList.Add(warning);
        }
    }

    public MultiYield HandleAccident(BaseAccident taccident)
    {
        return MultiThreadPool.AddNewMission(taccident, HandlingAccident);
    }

    public List<TrafficMessage> HandlingAccident(System.Object taccident)
    {
        //handle ticket delay
        Accident accident = taccident as Accident;
        TicketsOperaton tickets = new TicketsOperaton();
        List<int> routine_id = tickets.DelayTickets(accident.starttime, accident.location, accident.duration, accident.type);
        TimeManager.instance.Delay(routine_id);
        return new List<TrafficMessage>();
    }
    public MultiYield HandleAccidentCancel(BaseAccident taccident)
    {
        return null;
    }
    public MultiYield HandleAccidentWarning(BaseAccident taccident)
    {
        return null;
    }

    DateTime SetTime(int hour,int min,int sec)
    {
        return new DateTime(DateTime.Now.Year, 2, 4, hour, min, sec);
    }

    public IEnumerator Init()
    {
        accidentTexts = new AccidentTextOperation().GetRailAccidentText();
        AirportAccidentTexts = accidentTexts.FindAll(x => x.type == AccidentType.airport);
        RailAccidentTexts = accidentTexts.FindAll(x => x.type == AccidentType.rail);

        yield return null;
        AccidentList = new List<Accident>();
        AccidentWarningList = new List<AccidentWarning>();
        RailList = CityUtil.Instance.GetAllCityEdgeNum();
        AirportList = CityUtil.Instance.GetAllCityNodeNum();

        //RailList.Remove(4);
        //RailList.Remove(10);
        //RailList.Remove(24);
        //RailList.Remove(26);

        //AirportList.Remove(1);

        
        //AccidentList.Add(CreateAccident(AccidentType.rail, 4, 30, SetTime(9, 15, 0), 1));
        //AccidentList.Add(CreateAccident(AccidentType.rail, 10, 30, SetTime(13, 0, 0), 2));
        //AccidentList.Add(CreateAccident(AccidentType.rail, 24, 30, SetTime(17, 0, 0), 1));
        //AccidentList.Add(CreateAccident(AccidentType.rail, 26, 60, SetTime(18, 30, 0), 4));
        //AccidentList.Add(CreateAccident(AccidentType.airport, 1, 30, SetTime(18, 30, 0), 7));
        //AccidentList.Add(CreateAccident(AccidentType.airport, 1, 90, SetTime(20, 50, 0), 7));

        //AccidentList.Add(CreateAccident(AccidentType.airport, 4, 90, SetTime(9, 15, 0), accidentTexts[7]));
        yield return null;
        AccidentGenerate();
    }
}