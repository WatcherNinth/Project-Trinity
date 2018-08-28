using UnityEngine;
using System.Collections;

public class StartModel : BaseFuncModel {

    private int num;
    public int Num
    {
        get { return num; }
        set { num = value; }
    }

    public StartModel()
    {
        OccupancyType = Type.Num;
    }
}
