using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TestRoutine : MonoBehaviour {

	// Use this for initialization
	void Start () {
        RoutineOperation operation = new RoutineOperation();
        List <RoutineTicket> tickets = operation.GetAllTicket("上海", "合肥", true,  new DateTime());


    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
