﻿using UnityEngine;
using System.Collections;
using Lucky;
using System;
using System.Collections.Generic;

public class LocationView : BaseSceneEaseInOut
{

    public BaseGrid baseGrid;

    public static string[] citys =
    {
        "深圳",
        "广州",
        "北京",
        "杭州",
        "南京",
        "上海"
    };

    protected override void InitUI()
    {
        base.InitUI();
        Enter();
    }

    public void SetCallback(Action<string> tcallback, string local)
    {
        List<CityItem> l = new List<CityItem>();
        foreach(string city in citys)
        {
            if (city == local)
                continue;
            CityItem cm = new CityItem(city, tcallback);
            l.Add(cm);
        }
        baseGrid.source = l.ToArray();
    }
}
