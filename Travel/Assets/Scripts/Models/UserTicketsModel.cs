using UnityEngine;
using System.Collections;
using Lucky;

public class UserTicketsModel : BaseInstance<UserTicketsModel> {

    public float money = 0;

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
        
    }
    
}
