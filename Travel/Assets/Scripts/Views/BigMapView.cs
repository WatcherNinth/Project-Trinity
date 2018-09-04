using UnityEngine;
using System.Collections;
using HedgehogTeam.EasyTouch;
using UnityEngine.UI;

public class BigMapView : MonoBehaviour {

    private QuickDrag qd;
    private QuickPinch qpin;
    private QuickPinch qpout;

    private void Awake()
    {
        qd = GetComponent<QuickDrag>();
        QuickPinch[] qps = GetComponents<QuickPinch>();
        foreach (QuickPinch qp in qps)
        {
            if (qp.quickActionName == "PinchIn")
                qpin = qp;
            else
                qpout = qp;
        }
    }

    // Use this for initialization
    void Start () {
        Debug.Log("help");
        qd.onDragStart.AddListener(DragStart);
        qd.onDrag.AddListener(Drag);
        qpin.onPinchAction.AddListener(PinchIn);
        qpout.onPinchAction.AddListener(PinchOut);
    }

    private void PinchIn(Gesture gesture)
    {
        Debug.Log("PinchIn");
    }

    private void PinchOut(Gesture gesture)
    {
        Debug.Log("PinchOut");
    }

    private void DragStart(Gesture gesture)
    {
        Debug.Log("drag start");
        if (gesture.touchCount == 1)
        {
            Vector3 position = gesture.GetTouchToWorldPoint(1);
            Debug.Log("start " + position);
        }
    }

    private void Drag(Gesture gesture)
    {
        Debug.Log("drag");
        if (gesture.touchCount == 1)
        {
            Vector3 position = gesture.GetTouchToWorldPoint(1);
            Debug.Log("drag " + position);
            //deltaPosition = position - transform.position;

        }
    }
}
