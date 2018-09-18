
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
    public int id;
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
        Debug.Log(accident.starttime);
        accident = stringProcess.AccidentStringProcess(accident);
        PushAccident(accident);
        CreateAccidentWarning(accident);
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
        Accident eitem = new Accident
        {
            duration = item.duration,
            location = item.location,
            starttime = item.starttime.AddMinutes(item.duration),
            type = item.type,
            text = item.text
        };
        TimeManager.instance.AddAccidentExecute(eitem, null, true);
    }

    void PushAccidentList()
    {
        foreach (Accident item in AccidentList)
        {
            CreateAccidentWarning(item);
            TimeManager.instance.AddAccidentExecute(item, HandleAccident,false);
            Accident eitem = new Accident
            {
                duration = item.duration,
                location = item.location,
                starttime = item.starttime.AddMinutes(item.duration),
                type = item.type,
                text = item.text
            };
            TimeManager.instance.AddAccidentExecute(eitem, null, true);

            //timemanager callback
        }
        foreach (AccidentWarning item in AccidentWarningList)
        {
            //timemanager callback
            TimeManager.instance.AddAccidentExecute(item, null);
            AccidentWarning eitem = new AccidentWarning
            {
                min = item.min,
                location = item.location,
                starttime = item.starttime.AddMinutes(item.min),
                type = item.type,
                max = item.max
            };
            TimeManager.instance.AddAccidentExecute(eitem, null, true);
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


        CreateAccident(AccidentType.rail, 10, 480, SetTime(9, 0, 0), rnd.Next(0, RailAccidentTexts.Count));
        CreateAccident(AccidentType.rail, 4, 30, SetTime(9, 0, 0), rnd.Next(0, RailAccidentTexts.Count));
        CreateAccident(AccidentType.rail, 5, 60, SetTime(10, 30, 0), rnd.Next(0, RailAccidentTexts.Count));
        CreateAccident(AccidentType.rail, 1, 30, SetTime(12, 0, 0), rnd.Next(0, RailAccidentTexts.Count));
        CreateAccident(AccidentType.rail, 17, 60, SetTime(12, 0, 0), rnd.Next(0, RailAccidentTexts.Count));
        CreateAccident(AccidentType.rail, 6, 30, SetTime(15, 0, 0), rnd.Next(0, RailAccidentTexts.Count));
        CreateAccident(AccidentType.rail, 12, 30, SetTime(16, 0, 0), rnd.Next(0, RailAccidentTexts.Count));
        CreateAccident(AccidentType.rail, 14, 480, SetTime(16, 0, 0), rnd.Next(0, RailAccidentTexts.Count));
        CreateAccident(AccidentType.rail, 18, 480, SetTime(15, 0, 0), rnd.Next(0, RailAccidentTexts.Count));

        CreateAccident(AccidentType.airport, 5, 60, SetTime(10, 30, 0), rnd.Next(0, RailAccidentTexts.Count));
        CreateAccident(AccidentType.airport, 5, 30, SetTime(12, 0, 0), rnd.Next(0, RailAccidentTexts.Count));
        CreateAccident(AccidentType.airport, 4, 60, SetTime(12, 0, 0), rnd.Next(0, RailAccidentTexts.Count));
        CreateAccident(AccidentType.airport, 3, 480, SetTime(15, 0, 0), rnd.Next(0, RailAccidentTexts.Count));
        CreateAccident(AccidentType.airport, 3, 320, SetTime(16, 0, 0), rnd.Next(0, RailAccidentTexts.Count));
        CreateAccident(AccidentType.airport, 2, 240, SetTime(16, 0, 0), rnd.Next(0, RailAccidentTexts.Count));
        CreateAccident(AccidentType.airport, 1, 260, SetTime(19, 0, 0), rnd.Next(0, RailAccidentTexts.Count));
        CreateAccident(AccidentType.airport, 0, 30, SetTime(21, 0, 0), rnd.Next(0, RailAccidentTexts.Count));

        foreach (AccidentWarning item in AccidentWarningList)
        {
            //timemanager callback
            TimeManager.instance.AddAccidentExecute(item, null);
        }

        yield return null;
        //AccidentGenerate();
    }
}