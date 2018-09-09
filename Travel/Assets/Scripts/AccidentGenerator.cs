
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccidentGenerator : MonoBehaviour {

    public List<Accident> AccidentList;
    List<int> RailList,AirportList;
    int AirportAccident = 8;
    int RailAccident = 8;
    DateTime InitTime = GameModel.Instance.Start;
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
        AccidentList.Add(createAccident(AccidentType.rail, 4, 30, InitTime, null));
        AccidentList.Add(createAccident(AccidentType.rail, 10, 60, InitTime, null));
        AccidentList.Add(createAccident(AccidentType.rail, 24, 30, InitTime, null));
        AccidentList.Add(createAccident(AccidentType.rail, 26, 60, InitTime, null));
        AccidentList.Add(createAccident(AccidentType.airport, 1, 30, InitTime, null));
        AccidentList.Add(createAccident(AccidentType.rail, 27, 90, InitTime, null));
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    Accident createAccident(AccidentType type,int location,int duration,DateTime starttime,AccidentText text)
    {
        Accident accident = new Accident();
        accident.duration = duration;
        accident.location = location;
        accident.starttime = starttime;
        accident.type = type;
        //accident.text=
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
            //accident.text=
            AccidentList.Add(accident);
        }
        for (int i = 0; i <= RailAccident; i++)
        {
            accident.type = AccidentType.rail;
            accident.location = RailList[rnd.Next(0, RailList.Count)];
            accident.duration = rnd.Next(0, 30) * 10;
            accident.starttime = InitTime.AddMinutes(rnd.Next(0, 2880));
            //accident.text=
            AccidentList.Add(accident);
        }
    }
    void PushAccidentList()
    {
        for(int i = 0; i <= AccidentList.Count; i++)
        {

        }
    }
}
