using UnityEngine;
using System.Collections;
using Lucky;
using System.Collections.Generic;

public class LocationsModel : BaseInstance<LocationsModel>
{

    public static Dictionary<string, Vector3> cityslocation = new Dictionary<string, Vector3>
    {
        { "杭州", new Vector3() },
        { "上海", new Vector3(961,-300,0) },
        { "南京", new Vector3() },
        { "济南", new Vector3(584,389,0) },
        { "合肥", new Vector3() },
        { "天津", new Vector3() },
        { "北京", new Vector3() },
        { "沈阳", new Vector3() },
        { "郑州", new Vector3() },
        { "石家庄", new Vector3() }
    };
}
