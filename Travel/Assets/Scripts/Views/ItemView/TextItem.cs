using UnityEngine;
using System.Collections;
using System;

public class ItemMessage
{
    public string name;
    public string content;
    public Action<ItemMessage> callback;

    public ItemMessage(string tname, string tcontent)
    {
        name = tname;
        content = tcontent;
        callback = null;
    }
}
