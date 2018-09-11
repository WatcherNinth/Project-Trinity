using UnityEngine;
using System.Collections;
using Lucky;
using System.Collections.Generic;

public class LocationsModel : BaseInstance<LocationsModel>
{

    public static Dictionary<string, Vector3> cityslocation = new Dictionary<string, Vector3>
    {
        { "深圳", new Vector3() },
        { "广州", new Vector3() },
        { "北京", new Vector3() },
        { "杭州", new Vector3(8,-228,0) },
        { "南京", new Vector3() },
        { "上海", new Vector3(335,-416,0) },
        { "合肥", new Vector3() },
        { "成都", new Vector3() }
    };
}
