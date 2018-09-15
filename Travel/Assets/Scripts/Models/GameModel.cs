using UnityEngine;
using System.Collections;
using Lucky;
using System;

public class GameModel : BaseInstance<GameModel> {

    public DateTime SqlStart;

    public static string datetimeformat = "yyyy-MM-dd hh:mm:ss";

    private DateTime start;
    public DateTime Start
    {
        get { return start; }
        set { start = value; }
    }

    public DateTime tomorrow;

    public GameModel()
    {
        tomorrow = new DateTime(DateTime.Now.Year, 2, 5, 9, 0, 0);
#if UNITY_EDITOR
        start = new DateTime(DateTime.Now.Year, 2, 4, 9, 0, 0);
#endif

#if UNITY_ANDROID
        
        if(PlayerPrefs.HasKey("time"))
        {
            string temp = PlayerPrefs.GetString("time", "");
            start = Convert.ToDateTime(temp);
        }
        
        //start = new DateTime(DateTime.Now.Year, 2, 4, 9, 0, 0);
#endif

        SqlStart = new DateTime(DateTime.Now.Year, 2, 4, 0, 0, 0);
    }

}
