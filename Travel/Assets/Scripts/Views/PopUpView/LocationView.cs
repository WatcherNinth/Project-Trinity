using UnityEngine;
using System.Collections;
using Lucky;
using System;
using System.Collections.Generic;

public class LocationView : BaseSceneEaseInOut
{

    public BaseGrid baseGrid;

    private string[] citys;

    protected override void Awake()
    {
        base.Awake();
        citys = new string[LocationsModel.cityslocation.Count];
        LocationsModel.cityslocation.Keys.CopyTo(citys, 0);
    }

    protected override void InitUI()
    {
        base.InitUI();
        Enter();
    }

    public void SetCallback(Action<string> tcallback, string src, string dst)
    {
        List<CityItem> l = new List<CityItem>();
        foreach(string city in citys)
        {
            if (city == src || city == dst)
                continue;
            CityItem cm = new CityItem(city, tcallback);
            l.Add(cm);
        }
        baseGrid.source = l.ToArray();
    }
}
