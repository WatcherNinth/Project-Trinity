using UnityEngine;
using System.Collections;
using Lucky;
using System.Collections.Generic;

public class Texts
{
    public const string Event = "Event";
    public const string WeChat = "wechat";
}

public class TextManager : BaseInstance<TextManager> {

    private string dicname = "Data/";
    private Dictionary<string, TextAsset> dict = new Dictionary<string, TextAsset>();

    public TextAsset GetText(string name)
    {
        if (dict.ContainsKey(name))
            return dict[name];
        else
        {
            TextAsset ta = Resources.Load<TextAsset>(dicname + name);
            dict.Add(name,ta);
            return ta;
        }
    }

    public IEnumerator Init()
    {
        string[] files =
        {
            Texts.Event,
            Texts.WeChat
        };

        foreach(string file in files)
        {
            ResourceRequest rr = Resources.LoadAsync(dicname + file);
            yield return rr;
            if(rr.asset!=null)
            {
                dict.Add(file, rr.asset as TextAsset);
            }
        }
    }
}
