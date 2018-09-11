using UnityEngine;
using System.Collections;
using Lucky;

public enum Where
{
    Train = 0,
    AirPlane = 1,
    City = 2
}

public class UserTicketsModel : BaseInstance<UserTicketsModel> {

    public float money = 0;

    public string city;
    public Where where;

    //public Dictionary<long, >

    public UserTicketsModel()
    {
        float temp = PlayerPrefs.GetFloat("money", 0);
        if (temp == 0)
        {
            PlayerPrefs.SetFloat("money", 1001);
            money = 1001;
        }
        else
            money = PlayerPrefs.GetFloat("money", 0);

        money += 3000;

        where = Where.City;
        city = "上海";
    }
    
}
