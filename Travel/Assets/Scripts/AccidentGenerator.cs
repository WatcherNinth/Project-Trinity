
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
    DateTime InitTime = GameModel.Instance.Start;
    List<AccidentText> accidentTexts;

    //AccidentWarning property
    public List<AccidentWarning> AccidentWarningList;
    int[] AccidentWarningAccurency = { 60, 180, 300 };

    Accident CreateAccident(AccidentType type, int location, int duration, DateTime starttime, AccidentText text)
    {
        Accident accident = new Accident
        {
            duration = duration,
            location = location,
            starttime = starttime,
            type = type,
            text = text
        };
        return (accident);
    }
    static System.Random rnd = new System.Random();
    public void AccidentGenerate()
    {
        Accident accident = new Accident();
        StringProcessScript stringProcess = new StringProcessScript();
        for (int i = 0; i < AirportAccident; i++)
        {
            accident.type = AccidentType.airport;
            accident.location = AirportList[rnd.Next(0, AirportList.Count)];
            AirportList.RemoveAll(x => x == accident.location);
            accident.duration = rnd.Next(0, 31) * 10;
            accident.starttime = InitTime.AddMinutes(rnd.Next(0, 2881));
            accident.text = accidentTexts[rnd.Next(0, 5)];
            Debug.Log("location "+accident.location);
            accident = stringProcess.AccidentStringProcess(accident);
            AccidentList.Add(accident);
        }
        for (int i = 0; i < RailAccident; i++)
        {
            accident.type = AccidentType.rail;
            accident.location = RailList[rnd.Next(0, RailList.Count)];
            RailList.RemoveAll(x => x == accident.location);
            accident.duration = rnd.Next(0, 30) * 10;
            accident.starttime = InitTime.AddMinutes(rnd.Next(0, 2880));
            accident.text = accidentTexts[rnd.Next(5, 8)];
            accident = stringProcess.AccidentStringProcess(accident);
            AccidentList.Add(accident);
        }
        PushAccidentList();
    }
    void PushAccidentList()
    {
        foreach (Accident item in AccidentList)
        {
            CreateAccidentWarning(item);
            TimeManager.instance.AddAccidentExecute(item, HandleAccident);

            //timemanager callback
        }
        foreach (AccidentWarning item in AccidentWarningList)
        {
            //timemanager callback
        }
    }
    void CreateAccidentWarning(Accident accident)
    {
        
        AccidentWarning warning = new AccidentWarning();
        System.Random rnd = new System.Random();
        int rndNum;
        for (int i = 0; i < AccidentWarningAccurency.Length; i++)
        {
            warning.location = accident.location;
            warning.type = accident.type;
            warning.starttime = accident.starttime.AddMinutes(-AccidentWarningAccurency[i]);
            warning.Accidentstarttime = accident.starttime;
            rndNum = rnd.Next(0, AccidentWarningAccurency[i] / 2);
            warning.min = accident.duration - rndNum;
            warning.max = accident.duration + AccidentWarningAccurency[i] - rndNum;
            AccidentWarningList.Add(warning);
        }
    }

    public void HandleAccident(BaseAccident taccident)
    {
        //handle ticket delay
        Accident accident = taccident as Accident;

        //delay
    }


    DateTime SetTime(int hour,int min,int sec)
    {
        return new DateTime(DateTime.Now.Year, 2, 4, hour, min, sec);
    }

    public IEnumerator Init()
    {
        accidentTexts = new AccidentTextOperation().GetRailAccidentText();
        yield return null;
        AccidentList = new List<Accident>();
        AccidentWarningList = new List<AccidentWarning>();
        //CityUtil util = new CityUtil();
        //util.Init();
        RailList = CityUtil.Instance.GetAllCityEdgeNum();
        AirportList = CityUtil.Instance.GetAllCityNodeNum();

        RailList.Remove(4);
        RailList.Remove(10);
        RailList.Remove(9);
        RailList.Remove(24);
        RailList.Remove(26);

        AirportList.Remove(27);
        AirportList.Remove(1);

        AccidentList.Add(CreateAccident(AccidentType.rail, 4, 30, InitTime, accidentTexts[1]));
        AccidentList.Add(CreateAccident(AccidentType.rail, 10, 30, SetTime(13, 0, 0), accidentTexts[2]));
        AccidentList.Add(CreateAccident(AccidentType.rail, 24, 30, SetTime(17, 0, 0), accidentTexts[1]));
        AccidentList.Add(CreateAccident(AccidentType.rail, 26, 60, SetTime(18, 30, 0), accidentTexts[4]));
        AccidentList.Add(CreateAccident(AccidentType.airport, 1, 30, SetTime(18, 30, 0), accidentTexts[7]));
        AccidentList.Add(CreateAccident(AccidentType.airport, 27, 90, SetTime(20, 50, 0), accidentTexts[7]));
        yield return null;
        AccidentGenerate();
    }
}