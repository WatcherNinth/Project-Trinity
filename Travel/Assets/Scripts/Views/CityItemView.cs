using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Lucky;
using System;

public class CityItem
{
    public string city;
    public Action<string> tcallback;
}

public class CityItemView : ItemRender
{

    public Text City;

    private Button btn;
    private Action<string> callback;

    private void Awake()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(onClick);
    }

    protected override void UpdateView()
    {
        if (m_Data != null)
        {
            CityItem ci = m_Data as CityItem;
            City.text = ci.city;
            callback = ci.tcallback;
        }
    }

    public void onClick()
    {
        callback(City.text);
    }
}
