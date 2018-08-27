using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainController : BaseInstance<MainController>{

    private List<Vector2> points;
    private MainView view;

    public MainController()
    {
        points = new List<Vector2>();
    }

    public void SetView(MainView tview)
    {
        view = tview;
    }

    public void AddPoint(Vector2 point)
    {
        points.Add(point);
        if(points.Count==2)
        {

            MainModel.Instance.Start = points[0];
            MainModel.Instance.Stop = points[1];
            view.BuildLine();
            points.Clear();
        }
    }

}
