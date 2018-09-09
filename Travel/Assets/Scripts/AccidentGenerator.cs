
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AccidentType
{
    rail,
    airport
}

public class Accident
{
    public int location;
    public AccidentType type;
    public DateTime starttime;
    public int duration;
    public AccidentText text;
}
public class AccidentText
{
    public AccidentType type;
    public string title;
    public string description;
}
public class AccidentWarning
{
    public int location;
    public AccidentType type;
    public DateTime starttime;
    public DateTime Warningstarttime;
    public int min, max;

}
public class AccidentGenerator : MonoBehaviour {

    public List<Accident> AccidentList;
   
    List<int> RailList,AirportList;
    int AirportAccident = 8;
    int RailAccident = 8;
    DateTime InitTime = GameModel.Instance.Start;
    List<AccidentText> accidentTexts = new AccidentTextOperation().GetRailAccidentText();
 
    //AccidentWarning property
    public List<AccidentWarning> AccidentWarningList;
    int[] AccidentWarningAccurency = { 60, 180, 300 };
    // Use this for initialization
    void Start () {
        AccidentList = new List<Accident>();
        CityUtil util = new CityUtil();
        util.Init();
        RailList = util.GetAllCityEdgeNum();
        AirportList = util.GetAllCityNodeNum();
        RailList.Remove(4);
        RailList.Remove(10);
        RailList.Remove(9);
        RailList.Remove(24);
        RailList.Remove(26);
        RailList.Remove(27);
        AirportList.Remove(1);
        AccidentList.Add(CreateAccident(AccidentType.rail, 4, 30, InitTime, accidentTexts[2]));
        AccidentList.Add(CreateAccident(AccidentType.rail, 10, 60, InitTime, accidentTexts[3]));
        AccidentList.Add(CreateAccident(AccidentType.rail, 24, 30, InitTime, accidentTexts[2]));
        AccidentList.Add(CreateAccident(AccidentType.rail, 26, 60, InitTime, accidentTexts[5]));
        AccidentList.Add(CreateAccident(AccidentType.airport, 1, 30, InitTime, accidentTexts[8]));
        AccidentList.Add(CreateAccident(AccidentType.airport, 27, 90, InitTime, accidentTexts[8]));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    Accident CreateAccident(AccidentType type,int location,int duration,DateTime starttime,AccidentText text)
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
    public void AccidentGenerate()
    {
        Accident accident=new Accident();
        System.Random rnd = new System.Random();
        //通过某个函数返回Accidenttext总表
        for(int i = 0; i <= AirportAccident; i++)
        {
            accident.type = AccidentType.airport;
            accident.location = AirportList[rnd.Next(0, AirportList.Count)];
            accident.duration = rnd.Next(0, 30) * 10;
            accident.starttime = InitTime.AddMinutes(rnd.Next(0, 2880));
            accident.text = accidentTexts[rnd.Next(0,4)];
            AccidentList.Add(accident);
        }
        for (int i = 0; i <= RailAccident; i++)
        {
            accident.type = AccidentType.rail;
            accident.location = RailList[rnd.Next(0, RailList.Count)];
            accident.duration = rnd.Next(0, 30) * 10;
            accident.starttime = InitTime.AddMinutes(rnd.Next(0, 2880));
            accident.text = accidentTexts[rnd.Next(5, 7)];
            AccidentList.Add(accident);
        }
        PushAccidentList();
    }
    void PushAccidentList()
    {
        foreach(Accident item in AccidentList)
        {
            CreateAccidentWarning(item);
            //timemanager callback
        }
        foreach(AccidentWarning item in AccidentWarningList)
        {
            //timemanager callback
        }
    }
    void CreateAccidentWarning(Accident accident)
    {
        AccidentWarning warning=new AccidentWarning();
        System.Random rnd = new System.Random();
        int rndNum;
        for(int i = 0; i <= AccidentWarningAccurency.Length; i++)
        {
            warning.location = accident.location;
            warning.type = accident.type;
            warning.starttime = accident.starttime;
            warning.Warningstarttime = accident.starttime.AddMinutes(-AccidentWarningAccurency[i]);
            rndNum = rnd.Next(0, AccidentWarningAccurency[i] / 2);
            warning.min = accident.duration - rndNum;
            warning.max = accident.duration + AccidentWarningAccurency[i] - rndNum;
            AccidentWarningList.Add(warning);
        }
    }
}
