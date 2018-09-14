using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TestRoutine : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventUtil util = EventUtil.Instance;
        Events events = util.GetAllEvents();
        Debug.Log(events.data.Count);
        Debug.Log(events.data[0].content[0].text);


        WechatUtil w_util = WechatUtil.Instance;
        WechatContent diag = w_util.GetAllWechatContent();

        Debug.Log(diag.data.Count);
        Debug.Log(diag.data[1].name);
  



        //RoutineOperation operation = new RoutineOperation();
        //List <Routine> tickets = operation.GetAllTicket("上海", "南京", 0,  new DateTime());
        //TicketsOperaton ticket_operation = new TicketsOperaton();

        // DateTime now = GameModel.Instance.SqlStart;
        // ticket_operation.DelayTickets(now, 8, 60, AccidentType.rail);

        // DateTime now = GameModel.Instance.SqlStart;
        //ticket_operation.DelayTickets(now, 8, 60, AccidentType.rail);

        //DateTime begin_time = DateTime.Now;
        //DateTime end_time = DateTime.Now.AddHours(10);

        // operation.InsertTicket("上海", "成都", 0, begin_time, end_time, 10000, "T488");

        //TicketsOperaton ticket_operation = new TicketsOperaton();
        //if (ticket_operation.BuyTickets(1) != 0)
        //{
        //    Debug.Log("success");
        //}

        //RoutineTicket ti = ticket_operation.GetTicketByTickedId(26);
        //Debug.Log("end node "  + ti.GetEndNode());

        //List<RoutineTicket> all_tickets = ticket_operation.GetUserTickets(DateTime.Now);

        //CityUtil util = new CityUtil();
        //util.Init();

        //foreach(int i in util.GetAllCityNodeNum()){
        //    Debug.Log(i);
        //}

        //AccidentTextOperation accident_text_operation = new AccidentTextOperation();
        //List<AccidentText> accident_text = accident_text_operation.GetRailAccidentText();

        //foreach(AccidentText text in accident_text)
        //{
        //    Debug.Log(text.description);
        //}

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
