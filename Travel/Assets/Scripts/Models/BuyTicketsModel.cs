﻿using UnityEngine;
using System.Collections;
using Lucky;
using System;

public class BuyTicketsModel : BaseInstance<BuyTicketsModel>
{
    public string startlocation;
    public string stoplocation;
    public TrafficType type;
    public DateTime date;

    public BuyTicketsModel()
    {
        date = GameModel.Instance.Start;
        startlocation = "深圳";
        stoplocation = "北京";
        type = TrafficType.Train;
    }
    
	
}