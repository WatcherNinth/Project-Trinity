using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TestRoutine : MonoBehaviour {

	// Use this for initialization
	void Start () {
        RoutineOperation operation = new RoutineOperation();
        List <RoutineTicket> tickets = operation.GetAllTicket("上海", "合肥", 1,  new DateTime());
        DateTime begin_time = DateTime.Now;
        DateTime end_time = DateTime.Now.AddHours(10);

        operation.InsertTicket("合肥", "成都", 0, begin_time, end_time, 1000, "T488");

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
