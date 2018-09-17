using UnityEngine;
using System.Collections;
using Lucky;
using System.Collections.Generic;
using System;

public class NewManager : BaseInstance<NewManager> {

    public List<NewMessage> NewsList = new List<NewMessage>();
    static System.Random rnd = new System.Random();
    DateTime InitTime = GameModel.Instance.Start;
	public IEnumerator Init()
    {
        NewsGenerate();
        yield return null;
        PushNews();
    }

    public void PostNew(NewMessage data)
    {
        MessageBus.Post(data);
    }
     void NewsGenerate()
    {
        NewMessage item;
        string title, description;
        DateTime StartTime = InitTime.AddMinutes(30);


        title = "【注意】交通运输部发布“铁路停运令”";
        description = "由于寒潮“罗卡”影响，为了规避风险，现停运所有单次路程大于300千米的列车与航班，恢复时间另行通知。";
        item = new NewMessage(title, description, InitTime.AddMinutes(1));
        NewsList.Add(item);

        title = "“罗卡”寒潮或成春运期间最大灾害";
        description = "专家称本次寒潮造成的影响更甚之前“山竹”热带风暴";
        item = new NewMessage(title, description, InitTime.AddMinutes(2));
        NewsList.Add(item);

        title = "“熊孩子车厢”或将提上日程";
        description = "春运四天以来，铁路局收到了千万条建议，希望专门设立”熊孩子车厢“，以规避熊孩子给乘客带来的身体和精神上的双重打击。";
        StartTime = StartTime.AddMinutes(rnd.Next(0, 30));
        item = new NewMessage(title, description, StartTime);
        StartTime = StartTime.AddMinutes(30);
        NewsList.Add(item);

        title = "越骑越热？东西南北很重要";
        description = "东西南北很重要，你我都必须知道。昨日，杭州警方收留了一位骑反方向”回家过年“的摩托哥。”我就纳了闷了，我说怎么越来越热了呢“";
        StartTime = StartTime.AddMinutes(rnd.Next(0, 30));
        item = new NewMessage(title, description, StartTime);
        StartTime = StartTime.AddMinutes(30);
        NewsList.Add(item);

        title = "厕所爆裂，“年味”十足";
        description = "上海开往北京段D2443次列车在行驶过程中厕所爆裂，望欲将行驶该段的乘客及时关闭车窗，避免休克。";
        StartTime = StartTime.AddMinutes(rnd.Next(0, 30));
        item = new NewMessage(title, description, StartTime);
        StartTime = StartTime.AddMinutes(30);
        NewsList.Add(item);

        title = "千响大地红，加班族最后的倔强";
        description = "互联网工作的小胡因不满领导年三十还让大家加班的做法，买了两挂千响”大地红“来领导办公室门口放鞭炮，当即辞职。";
        StartTime = StartTime.AddMinutes(rnd.Next(0, 30));
        item = new NewMessage(title, description, StartTime);
        StartTime = StartTime.AddMinutes(30);
        NewsList.Add(item);

        title = "积雪清扫持续进行";
        description = "在全国铁路部门的努力下，南北段铁路积雪情况已及时清除，为大家回家过年保驾护航。";
        StartTime = StartTime.AddMinutes(rnd.Next(0, 30));
        item = new NewMessage(title, description, StartTime);
        StartTime = StartTime.AddMinutes(30);
        NewsList.Add(item);

        title = "心脏病突发！90后小武及时抢救";
        description = "96年出生的小武虽然还只是医学院的本科生，但凭借出色过硬的专业知识和手法成功抢救了心脏病突发的郑女士，为你点赞！";
        StartTime = StartTime.AddMinutes(rnd.Next(0, 30));
        item = new NewMessage(title, description, StartTime);
        StartTime = StartTime.AddMinutes(30);
        NewsList.Add(item);

        title = "天盼电台春节不打烊";
        description = "春节不打烊，DJ.天盼与您同在。春运枯燥了吗？请及时打开FM95.8兆赫，DJ.天盼与您共度春运时光。";
        StartTime = StartTime.AddMinutes(rnd.Next(0, 30));
        item = new NewMessage(title, description, StartTime);
        StartTime = StartTime.AddMinutes(30);
        NewsList.Add(item);

        title = "程序猿：“春节是什么？”";
        description = "近日，某互联网公司程序猿（其实是策划）蔡某因长时间工作不回家，被领导勒令”暂时辞职“回家过年。";
        StartTime = StartTime.AddMinutes(rnd.Next(0, 30));
        item = new NewMessage(title, description, StartTime);
        StartTime = StartTime.AddMinutes(30);
        NewsList.Add(item);

        title = "过年“大礼包”！非法传销一锅端";
        description = "经过警方的日夜联轴蹲点一个月，江浙沪传销窝点头子”吱吱“终于出山回家过年，警方一举抓获。";
        StartTime = StartTime.AddMinutes(rnd.Next(0, 30));
        item = new NewMessage(title, description, StartTime);
        StartTime = StartTime.AddMinutes(30);
        NewsList.Add(item);

        title = "Lucky一点不Lucky";
        description = "一名自称Lucky的外籍小伙子年末可以一点也不Lucky，归心似箭的他上错了回家的车，醒来后发现已经到离家越来越远。";
        StartTime = StartTime.AddMinutes(rnd.Next(0, 30));
        item = new NewMessage(title, description, StartTime);
        StartTime = StartTime.AddMinutes(30);
        NewsList.Add(item);

        title = "疯狂抄袭！画师糖蛋蛋被揭穿";
        description = "画师唐蛋蛋近日被揭穿，据知情人透露，唐蛋蛋的所有作品均出自盐棒棒之手。";
        StartTime = StartTime.AddMinutes(rnd.Next(0, 30));
        item = new NewMessage(title, description, StartTime);
        StartTime = StartTime.AddMinutes(30);
        NewsList.Add(item);

        title = "现实绿巨人？手撕机器不嫌累！";
        description = "你们放假，后端没假。后端程序员布鲁克因不满公司安排，积蓄已久的气愤瞬间爆发，徒手锤烂了公司近20台服务器。";
        StartTime = StartTime.AddMinutes(rnd.Next(0, 30));
        item = new NewMessage(title, description, StartTime);
        StartTime = StartTime.AddMinutes(30);
        NewsList.Add(item);

    }
    void PushNews()
    {
        foreach(NewMessage item in NewsList)
        {
            TimeManager.instance.AddNews(item, PostNew);
            //Lucky.LuckyUtils.Log("send News " + item.title + " at " + item.date.ToShortTimeString());
        }
    }
}
