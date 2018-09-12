using UnityEngine;
using System.Collections;
using Lucky;
using System.Collections.Generic;

public class SpriteManager : BaseInstance<SpriteManager> {

    private const string spritePath = "Sprites/";
    private Dictionary<string, Sprite> spriteDics = new Dictionary<string, Sprite>();
    
    public IEnumerator Init()
    {
        string[] s =
        {

        };

        foreach(string name in s)
        {
            ResourceRequest rr = Resources.LoadAsync(name);
            yield return rr;
            if(rr.asset!=null)
            {
                spriteDics.Add(name, (Sprite)rr.asset);
                Debug.Log("load " + name);
            }
            else
                Debug.Log(name + "load failed");
        }
    }

    public Sprite GetSprite(string name)
    {
        if(spriteDics.ContainsKey(name))
        {
            return spriteDics[name];
        }
        else
        {
            Sprite sprite = Resources.Load<Sprite>(spritePath + name);
            spriteDics.Add(name, sprite);
            return sprite;
        }
    }
    

}
