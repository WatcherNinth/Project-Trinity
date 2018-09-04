using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lucky;
using HedgehogTeam.EasyTouch;

public class MapsView : BaseUI {

    private MainContent mainContent;

    public Button Btn;
    public Camera cam;

    private QuickDrag qd;
    private QuickPinch qpin;
    private QuickPinch qpout;

    private void Awake()
    {
        base.Awake();
        qd = GetComponent<QuickDrag>();
        QuickPinch[] qps = GetComponents<QuickPinch>();
        foreach(QuickPinch qp in qps)
        {
            if (qp.quickActionName == "PinchIn")
                qpin = qp;
            else
                qpout = qp;
        }
    }

    // Use this for initialization
    void Start () {
        mainContent = transform.parent.gameObject.GetComponent<MainContent>();
        InitEvent(true);
        Btn.onClick.AddListener(OnClick);
        //qd.onDragStart.AddListener(DragStart);
        //qd.onDrag.AddListener(Drag);
        //qpin.onPinchAction.AddListener(PinchIn);
        //qpout.onPinchAction.AddListener(PinchOut);
	}

    protected override void OnDestroy()
    {
        base.OnDestroy();
        InitEvent(false);
    }

    private void OnClick()
    {
        mainContent.ShowView(ViewID.BuyTickets);
    }

   

    private void Touch(Gesture gesture)
    {
        Debug.Log("touch down");
    }

    public void InitEvent(bool isOn)
    {
        if(isOn)
        {
            Debug.Log("Init");
            //EasyTouch.On_Drag += Drag;
            //EasyTouch.On_DragStart += DragStart;
            //EasyTouch.On_TouchDown += Touch;
            //EasyTouch.On_PinchIn += PinchIn;
            //EasyTouch.On_PinchOut += PinchOut;

        }
        else
        {
           // EasyTouch.On_PinchIn -= PinchIn;
            //EasyTouch.On_PinchOut -= PinchOut;
            //EasyTouch.On_Drag -= Drag;
            //EasyTouch.On_DragStart -= DragStart;
            //EasyTouch.On_TouchDown -= Touch;
        }
    }


}
