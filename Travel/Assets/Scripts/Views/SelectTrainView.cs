using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;
using System.Collections.Generic;

public class SelectTrainView : BaseUI {

    public Button back;
    public Text Src;
    public Text Dst;
    public Text GoDate;
    public Button yesterday;
    public Button tomorrow;
    public Button BtnGoData;

    public BaseGrid content;

	// Use this for initialization
	void Start () {
        List<TrafficMessage> data = new List<TrafficMessage>();
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007"));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007"));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007"));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007"));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007"));
        data.Add(new TrafficMessage("02:30", "北京", "05:40", "G250", "08:10", "广州", "1007"));

        content.source = data.ToArray();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
