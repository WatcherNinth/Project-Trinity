using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TestRoutine : MonoBehaviour {

	// Use this for initialization
	void Start () {
        RoutineOperation operation = new RoutineOperation();
        List <Routine> tickets = operation.GetAllTicket("上海", "杭州", 0,  new DateTime());


<<<<<<< HEAD
        foreach (Routine t in tickets)
=======
        TicketsOperaton ticket_operation = new TicketsOperaton();
        if (ticket_operation.BuyTickets(1)!=0)
>>>>>>> 2b7270659ef67c6e49f637e5141e95b26b970a6c
        {
            Debug.Log(t.GetBeginTime());
            Debug.Log(t.GetTicketName());
        }

        //DateTime begin_time = DateTime.Now;
        //DateTime end_time = DateTime.Now.AddHours(10);

        //operation.InsertTicket("上海", "成都", 0, begin_time, end_time, 10000, "T488");

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
