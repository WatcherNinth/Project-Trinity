using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;
using System;

public class CalendarView : BaseSceneEaseInOut
{
    private Action<DateTime> callback;

    public Button LastMonth;
    public Button NextMonth;
    public Text Month;
    public GameObject DateObject;
    public Transform MonthContent;
    public Color Blue;
    public GridLayoutGroup gridLayoutGroup;
    public float height;

    private string DateFormat = "yyyy年M月";
    private DateTime date;

    public DateTime Date
    {
        set
        {
            date = value;
            InvalidView();
        }
    }

    protected override void InitUI()
    {
        base.InitUI();
        Enter();
        InitButtonEvent();
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();
    }

    private void InitButtonEvent()
    {
        LastMonth.onClick.AddListener(delegate ()
        {
            date = date.AddMonths(-1);
            UpdateView();
        });
        NextMonth.onClick.AddListener(delegate ()
        {
            date = date.AddMonths(1);
            Debug.Log(date.ToString(DateFormat));
            UpdateView();
        });
    }

    protected override void UpdateView()
    {
        base.UpdateView();

        if(MonthContent.childCount!=0)
        {
            for(int i=0;i<MonthContent.childCount;i++)
            {
                Destroy(MonthContent.GetChild(i).gameObject);
            }
        }

        DateTime dt = new DateTime(date.Year, date.Month, 1);
        Month.text = date.ToString(DateFormat);
        int week = Convert.ToInt32(dt.DayOfWeek);
        int day = date.Day;

        int totalDays = DateTime.DaysInMonth(date.Year, date.Month);

        if(totalDays+week > 35)
        {
            gridLayoutGroup.cellSize = new Vector2(gridLayoutGroup.cellSize.x, height / 6);
        }
        else
        {
            gridLayoutGroup.cellSize = new Vector2(gridLayoutGroup.cellSize.x, height / 5);
        }

        for (int i=0;i<week;i++)
        {
            SetDay("");
        }
        for(int i=1;i<= totalDays; i++)
        {
            if (date.Day == i)
                SetDay(i + "", true);
            else
                SetDay(i + "");
        }
    }

    private void SetDay(string day,bool Today=false)
    {
        GameObject temp = Instantiate(DateObject);
        temp.transform.SetParent(MonthContent);
        temp.GetComponent<RectTransform>().localScale = Vector3.one;
        temp.SetActive(true);

        DayView dv = temp.GetComponent<DayView>();
        dv.Day.text = day;
        dv.Btn.onClick.AddListener(delegate()
        {
            date = new DateTime(date.Year, date.Month, Convert.ToInt32(day));
            Debug.Log(date);
            if(callback!=null)
                callback(date);
            Dispose();

        });
        if (Today)
            dv.SetColor(Blue);
    }

    public void AddCallback(Action<DateTime> call)
    {
        callback = null;
        callback = call;
    }

    


}
