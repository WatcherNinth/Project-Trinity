using UnityEngine;
using System.Collections;
using HedgehogTeam.EasyTouch;
using UnityEngine.UI;

public class BigMapView : MonoBehaviour {

    private QuickDrag qd;
    private QuickPinch qpin;
    private QuickPinch qpout;

    private GameObject Map;

    private void Awake()
    {
        Map = GameObject.FindGameObjectWithTag("MapCanvas");
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
        qd.onDrag.AddListener(Drag);
        qpin.onPinchAction.AddListener(PinchIn);
        qpout.onPinchAction.AddListener(PinchOut);
    }

    private void PinchIn(Gesture gesture)
    {
        float zoom = Time.deltaTime * gesture.deltaPinch / 25;
        Vector3 scale = transform.localScale;

        if (scale.x - zoom > 0.1)
        {
            transform.localScale = new Vector3(scale.x - zoom, scale.y - zoom, 1f);
            Map.transform.localScale = transform.localScale;
        }
            
    }

    private void PinchOut(Gesture gesture)
    {
        float zoom = Time.deltaTime * gesture.deltaPinch / 25;
        Vector3 scale = transform.localScale;


        if (scale.x + zoom < 3)
        {
            transform.localScale = new Vector3(scale.x + zoom, scale.y + zoom, 1f);
            Map.transform.localScale = transform.localScale;
        }
            
    }

    private void Drag(Gesture gesture)
    {
        if (gesture.touchCount == 1)
        {
            Map.transform.position = transform.position;

        }
    }
}
