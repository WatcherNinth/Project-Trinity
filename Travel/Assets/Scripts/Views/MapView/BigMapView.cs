using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;

public class BigMapView : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler
{

    private GameObject Map;
    private GameObject m_3DMap;
    private RectTransform rt;

    private float DoubleTouchLastDis = 0;
    private Vector3 LastPosition = Vector3.zero;
    private float ImageHeight;
    private float ImageWidth;
    private float top;
    private float bottom;
    private float left;
    private float right;
    private RectTransform parent;
    private bool isMove;
    private Vector2 End;

    private Vector3 initScale = new Vector3(0.65f, 0.65f, 1);
    private Vector2 initPos = new Vector2(-370, -87);

    private void Awake()
    {
        Map = GameObject.FindGameObjectWithTag("MapCanvas");
        m_3DMap = GameObject.FindGameObjectWithTag("3DMap");
        rt = GetComponent<RectTransform>();
        parent = transform.parent.gameObject.GetComponent<RectTransform>();
        isMove = false;
    }

    private void Start()
    {
        ImageHeight = rt.sizeDelta.y;
        ImageWidth = rt.sizeDelta.x;

        top = parent.sizeDelta.y / 2;
        bottom = -top;
        right = parent.sizeDelta.x / 2;
        left = -right;

        transform.localScale = initScale;
        Map.transform.localScale = transform.localScale;
        m_3DMap.transform.localScale *= 0.65f;

        rt.anchoredPosition = initPos;
        Map.transform.position = new Vector3(rt.position.x,rt.position.y,Map.transform.position.z);
        m_3DMap.transform.position = new Vector3(rt.position.x, rt.position.y, m_3DMap.transform.position.z);
    }

    private void Update()
    {
        if(!isMove)
        {
            if ((Input.touchCount == 2) && (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(1).phase == TouchPhase.Began))
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                float DoubleTouchCurrDis = Vector2.Distance(touch1.position, touch2.position);
                DoubleTouchLastDis = DoubleTouchCurrDis;
            }
            else if ((Input.touchCount == 2) && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved))
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                float DoubleTouchCurrDis = Vector2.Distance(touch1.position, touch2.position);
                float deltaPinch = DoubleTouchCurrDis - DoubleTouchLastDis;
                float zoom = Time.deltaTime * deltaPinch / 25;
                Vector3 scale = transform.localScale;
                float smaller = 1.0f;
                if (zoom < 0)
                {
                    if (scale.x + zoom > 0.5)
                    {
                        smaller = (scale.x + zoom) / scale.x;
                        transform.localScale = new Vector3(scale.x + zoom, scale.y + zoom, 1f);
                    }
                }
                else
                {
                    if (scale.x + zoom < 1.5)
                    {
                        smaller = (scale.x + zoom) / scale.x;
                        transform.localScale = new Vector3(scale.x + zoom, scale.y + zoom, 1f);
                    }
                }
                Map.transform.localScale = transform.localScale;
                m_3DMap.transform.localScale *= smaller;
                DoubleTouchLastDis = DoubleTouchCurrDis;
            }
            else if ((Input.touchCount == 2) && (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(1).phase == TouchPhase.Ended))
            {
                isInside();
            }
        }
        else
        {

            if (Vector2.Distance(rt.anchoredPosition, End) > 10)
                rt.anchoredPosition = Vector2.Lerp(rt.anchoredPosition, End, Time.deltaTime * 5);
            else
            {
                rt.anchoredPosition = End;
                isMove = false;
            }

            Map.transform.position = new Vector3(rt.position.x, rt.position.y, Map.transform.position.z);
            m_3DMap.transform.position = new Vector3(rt.position.x, rt.position.y, m_3DMap.transform.position.z);
        }
        

        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if( Input.touchCount == 1 )
        {
            Lucky.LuckyUtils.Log("drag");
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                Vector3 deltaPosition = globalMousePos - LastPosition;
                transform.position += deltaPosition;
                Map.transform.position += deltaPosition;
                Vector3 tempdeltaPostion = new Vector3(deltaPosition.x, deltaPosition.y, 0);
                m_3DMap.transform.position += tempdeltaPostion;
                LastPosition = globalMousePos;   
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Input.touchCount == 1)
        {
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                LastPosition = globalMousePos;    
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isInside();
    }

    public bool isInside()
    {
        Vector3 pos = rt.anchoredPosition;
        float h = rt.localScale.y * ImageHeight / 2;
        float w = rt.localScale.x * ImageWidth / 2;

        float nowleft = pos.x - w;
        float nowright = pos.x + w;
        float nowtop = pos.y + h;
        float nowbottom = pos.y - h;

        float leftdelta = nowleft - left;
        float rightdelta = nowright - right;
        float topdelta = nowtop - top;
        float bottomdelta = nowbottom - bottom;

        float deltax = 0.0f;
        float deltay = 0.0f;

        if(leftdelta>0)
        {
            deltax = leftdelta;
            isMove = true;
        }
        if(bottomdelta>0)
        {
            deltay = bottomdelta;
            isMove = true;
        }
        if(rightdelta<0)
        {
            deltax = rightdelta;
            isMove = true;
        }
        if(topdelta<0)
        {
            deltay = topdelta;
            isMove = true;
        }
        if(isMove)
        {
            End = new Vector2(rt.anchoredPosition.x - deltax, rt.anchoredPosition.y - deltay);
        }
        
        return true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PassEvent(eventData, ExecuteEvents.submitHandler);
        PassEvent(eventData, ExecuteEvents.pointerClickHandler);
    }

    public void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function)
        where T : IEventSystemHandler
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        GameObject current = data.pointerCurrentRaycast.gameObject;
        for (int i = 0; i < results.Count; i++)
        {
            if (current != results[i].gameObject)
            {
                if(results[i].gameObject.CompareTag("Warning"))
                    ExecuteEvents.Execute(results[i].gameObject, data, function);
                if (results[i].gameObject.CompareTag("LocationShow"))
                    ExecuteEvents.Execute(results[i].gameObject, data, function);
            }
        }
    }
}
