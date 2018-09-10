using UnityEngine;
using System.Collections;
using System;
using Lucky;

public class BaseEvent
{

}

public class EventHappen<T>
{
    public Func<T, bool> judge;
    public Action<T> executecallback;
}

public class EventHappenManager : BaseInstance<EventHappenManager> {

    // Update is called once per frame
    public void Update() {

    }

    public IEnumerator Init()
    {
        yield return null;
    }
}
