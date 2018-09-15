using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;
using System;

public class NewMessage
{
    public string title;
    public string content;
    public DateTime date;
    public Action<NewMessage> callback;

    public NewMessage(string ttitle, string tcontent, DateTime tdate)
    {
        title = ttitle;
        content = tcontent;
        date = tdate;
        callback = null;
    }
}
