using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class WarningView : BaseUI, IPointerClickHandler
{

    private RectTransform rt;
    private string news;

    private Action callback;

    private BaseAccident accidentMessage = null;
    public BaseAccident AccidentMessage
    {
        set
        {
            accidentMessage = value;
            InvalidView();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        callback();
    }

    protected override void Awake()
    {
        base.Awake();
        callback = null;
        rt = GetComponent<RectTransform>();
    }

    protected override void UpdateView()
    {
        base.UpdateView();
        if(accidentMessage!=null)
        {
            SetDate(accidentMessage);
        }
    }

    private void SetDate(BaseAccident data)
    {
        callback = null;
        if(data.GetType()== typeof(Accident))
        {
            
            Accident accident = data as Accident;
            string city = CityUtil.Instance.GetCityName(accident.location);
            RectTransform cityrt = MapTrafficView.instance.FindPlace(city);
            Vector3 pos = cityrt.anchoredPosition3D + new Vector3(cityrt.rect.width / 2, 0, 0);
            Debug.Log("city warning" + city);
            rt.anchoredPosition3D = pos;
            rt.localScale = new Vector3(1.5f, 1.5f, 0);

            news = "灾害持续时间： "+accident.duration+"分钟";
            Debug.Log("add listener");
            callback=delegate ()
            {
                InfoView.Show(new InfoMessage(news, "灾害！"));
            };
        }
        else if(data.GetType() == typeof(AccidentWarning))
        {
            
            AccidentWarning warning = data as AccidentWarning;
            Debug.Log("location number "+warning.location);
            string city = CityUtil.Instance.GetCityName(warning.location);
            RectTransform cityrt = MapTrafficView.instance.FindPlace(city);
            Vector3 pos = cityrt.anchoredPosition3D + new Vector3(cityrt.rect.width / 2, 0, 0);
            rt.anchoredPosition3D = pos;
            rt.localScale = new Vector3(1.5f, 1.5f, 0);

            news = "<b><size=60>此地即将发生灾害事件</size></b>\n\n"
                + "灾害预计发生时间: " + warning.Accidentstarttime.ToString("HH:mm") + "\n\n"
                + "灾害预计持续时间： " + warning.min + "min ~ " + warning.max + "min";
           callback=delegate ()
            {
                InfoView.Show(new InfoMessage(news, "警告！"));
            };
        }
    }
}
