using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class WarningView : BaseUI, IPointerClickHandler
{

    public Text WarningText;

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
            WarningText.text = "发生灾害";
            
            Accident accident = data as Accident;
            string city = CityUtil.Instance.GetCityName(accident.location);
            Vector3 pos = LocationsModel.cityslocation[city];
            Debug.Log("city warning" + city);
            pos += new Vector3(0, 100, 0);
            rt.anchoredPosition3D = pos;

            news = "灾害持续时间： "+accident.duration+"分钟";
            Debug.Log("add listener");
            callback=delegate ()
            {
                InfoView.Show(new InfoMessage(news, "灾害"));
            };
        }
        else if(data.GetType() == typeof(AccidentWarning))
        {
            WarningText.text = "灾害预警";
            
            AccidentWarning warning = data as AccidentWarning;
            string city = CityUtil.Instance.GetCityName(warning.location);
            Debug.Log("city warning" + city);
            Vector3 pos = LocationsModel.cityslocation[city];
            pos += new Vector3(0, 100, 0);
            rt.anchoredPosition3D = pos;

            news = "灾害预计时间： " + warning.Accidentstarttime.ToString("HH/mm") + "\n"
                + "持续最短时间： " + warning.min + "分钟\n"
                + "持续最长时间： " + warning.max + "分钟";
           callback=delegate ()
            {
                InfoView.Show(new InfoMessage(news, "灾害预警"));
            };
        }
    }
}
