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
            string city = "";
            Accident accident = data as Accident;
            if(accident.type == AccidentType.airport)
            {
                city = CityUtil.Instance.GetCityName(accident.location);
                RectTransform cityrt = MapTrafficView.instance.FindPlace(city);
                Vector3 pos = cityrt.anchoredPosition3D + new Vector3(cityrt.rect.width / 2, 0, 0);
                Lucky.LuckyUtils.Log("city warning" + city);
                rt.anchoredPosition3D = pos;
            }
            else
            {
                RectTransform railwayrt = MapTrafficView.instance.FindRailway(accident.location);
                Vector3 pos = railwayrt.anchoredPosition3D;
                rt.anchoredPosition3D = pos;
            }
            rt.localScale = new Vector3(1.5f, 1.5f, 0);

            //news = city + "灾害持续时间： "+accident.duration+"分钟";
            news = accident.text.description;
            Lucky.LuckyUtils.Log("add listener");
            callback=delegate ()
            {
                InfoView.Show(new InfoMessage(news, "寒潮已经到达！"));
            };
        }
        else if(data.GetType() == typeof(AccidentWarning))
        {
            string city = "";
            AccidentWarning warning = data as AccidentWarning;
            if (warning.type == AccidentType.airport)
            {
                Lucky.LuckyUtils.Log("location number " + warning.location);
                city = CityUtil.Instance.GetCityName(warning.location);
                RectTransform cityrt = MapTrafficView.instance.FindPlace(city);
                Vector3 pos = cityrt.anchoredPosition3D + new Vector3(cityrt.rect.width / 2, 0, 0);
                rt.anchoredPosition3D = pos;
                news = "寒潮即将到达" + city + "，请注意出行安全\n\n"
                    + "<size=45>寒潮预计到达时间: " + warning.Accidentstarttime.ToString("HH:mm") + "</size>\n\n"
                    + "<size=45>寒潮预计持续时间： " + warning.min + "分钟 ~ " + warning.max + "分钟</size>";
            }
            else
            {
                RectTransform railwayrt = MapTrafficView.instance.FindRailway(warning.location);
                Vector3 pos = railwayrt.anchoredPosition3D;
                rt.anchoredPosition3D = pos;
                CityMapping edge=CityUtil.Instance.GetEdgeCity(warning.location);
                news = "寒潮即将到达" + edge.start_node + "-" + edge.end_node + "，请注意出行安全\n\n"
                    + "<size=45>寒潮预计到达时间: " + warning.Accidentstarttime.ToString("HH:mm") + "</size>\n\n"
                    + "<size=45>寒潮预计持续时间： " + warning.min + "分钟 ~ " + warning.max + "分钟</size>";
            }

            rt.localScale = new Vector3(1.5f, 1.5f, 0);

            //news = ""+ city +"寒潮即将到达此处，请注意出行安全\n\n"
               // + "<size=45>寒潮预计到达时间: " + warning.Accidentstarttime.ToString("HH:mm") + "</size>\n\n"
               // + "<size=45>寒潮预计持续时间： " + warning.min + "分钟 ~ " + warning.max + "分钟</size>";
           callback=delegate ()
            {
                InfoView.Show(new InfoMessage(news, "【寒潮天气预警】"));
            };
        }
    }
}
