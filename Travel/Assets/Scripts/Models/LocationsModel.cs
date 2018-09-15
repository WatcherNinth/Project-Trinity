using UnityEngine;
using System.Collections;
using Lucky;
using System.Collections.Generic;

public class LocationsModel : BaseInstance<LocationsModel>
{

    
    public static Dictionary<string, Vector3> cityslocation = new Dictionary<string, Vector3>
    {
        { "杭州", new Vector3() },
        { "上海", new Vector3(960.768f, -299.52f,0) },
        { "南京", new Vector3(718.08f, -249.6f, 0) },
        { "济南", new Vector3(560.64f, 418.56f,0) },
        { "合肥", new Vector3(455.04f, -322.56f, 0) },
        { "天津", new Vector3(414.336f, 951.552f, 0) },
        { "北京", new Vector3(414.72f, 979.2f,0) },
        { "沈阳", new Vector3(1320.96f, 1251.84f, 0) },
        { "郑州", new Vector3(2.688f,50.688f, 0 ) },
        { "石家庄", new Vector3(199.68f, 741.12f, 0) }
    };
   
}
