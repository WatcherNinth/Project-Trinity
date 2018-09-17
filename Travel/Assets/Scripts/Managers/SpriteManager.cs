using UnityEngine;
using System.Collections;
using Lucky;
using System.Collections.Generic;

public class Sprites
{
    public const string ticket_airplane = "ticket_airplane";
    public const string ticket_train = "ticket_train";
    public const string wechat = "WeChat";
    public const string news = "News";
    public const string shorttext = "ShortText";
    public const string mother = "mother";
    public const string father = "father";
    public const string sister = "sister";
    public const string book1 = "book1";
    public const string book2 = "book2";
    public const string book3 = "book3";
    public const string book4 = "book4";
    public const string book5 = "book5";
    public const string book6 = "book6";
    public const string book7 = "book7";
    public const string book8 = "book8";
}

public class SpriteManager : BaseInstance<SpriteManager> {

    private const string spritePath = "Sprites/";
    private Dictionary<string, Sprite> spriteDics = new Dictionary<string, Sprite>();
    
    public IEnumerator Init()
    {
        string[] s =
        {
            Sprites.book1,
            Sprites.book2,
            Sprites.book3,
            Sprites.book4,
            Sprites.book5,
            Sprites.book6,
            Sprites.book7,
            Sprites.book8
        };

        foreach(string name in s)
        {
            ResourceRequest rr = Resources.LoadAsync(name);
            yield return rr;
            if(rr.asset!=null)
            {
                spriteDics.Add(name, (Sprite)rr.asset);
                Lucky.LuckyUtils.Log("load " + name);
            }
            else
                Lucky.LuckyUtils.Log(name + "load failed");
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
