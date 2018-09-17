using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public enum Direction
{
    LeftToRight = 0,
    RightToLeft = 1
}

public class ParentScrollViewRect : ScrollRect
{
    private float triggerspeed = 2000.0f;
    protected int index;

    private float time = 0.3f;
    private float width;
    private RectTransform rt;

    private bool drag = true;
    private float firstx;
    private Direction direct;
    private float starttime;

    protected override void Awake()
    {
        base.Awake();
        index = 0;
        rt = content.GetComponent<RectTransform>();
    }

    protected override void Start()
    {
        base.Start();
        width = GetComponent<RectTransform>().rect.width;
        int count = base.content.transform.childCount;
        rt.anchoredPosition = new Vector2(width * (1- count), rt.anchoredPosition.y);
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (drag)
        {
            base.OnBeginDrag(eventData);
            firstx = rt.anchoredPosition.x;
            starttime = Time.realtimeSinceStartup;
        }

    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (drag)
        {
            base.OnDrag(eventData);
            if(firstx ==0 )
            {
                if (rt.anchoredPosition.x * 2 > width)
                    rt.anchoredPosition = new Vector2(width / 2, rt.anchoredPosition.y);
            }

            int count = base.content.transform.childCount;
            if (firstx == (1 - count) * width)
            {
                if (rt.anchoredPosition.x < (0.5 -  count) * width)
                    rt.anchoredPosition = new Vector2((0.5f - count) * width, rt.anchoredPosition.y);
            }

        }
            
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        float x = base.content.GetComponent<RectTransform>().anchoredPosition.x;

        if (x > firstx)
        {
            direct = Direction.LeftToRight;
        }
        else
        {
            direct = Direction.RightToLeft;
        }

        drag = false;

        float dst = 0;
        float move = Mathf.Abs(x - firstx);
        int k = (int)(-firstx / width);

        //速度
        float deltatime = Time.realtimeSinceStartup - starttime;
        float speed = move / deltatime;
        Lucky.LuckyUtils.Log("get speed "+speed);
        if(speed > triggerspeed)
        {
            if (direct == Direction.LeftToRight)
            {
                if (firstx == 0)
                    dst = 0;
                else
                    dst = (k - 1) * width;
            }
            else
            {
                int count = base.content.transform.childCount;
                if (firstx == (1 - count) * width)
                    dst = (count - 1) * width;
                else
                    dst = (k + 1) * width;
            }
        }
        else
        {
            //滑动距离
            if (direct == Direction.LeftToRight)
            {
                if (move * 2 > width)
                {
                    if (firstx == 0)
                        dst = 0;
                    else
                        dst = (k - 1) * width;
                }
                else
                {
                    dst = k * width;
                }
            }
            else
            {
                if (move * 2 > width)
                {
                    int count = base.content.transform.childCount;
                    if (firstx == (1 - count) * width)
                        dst = (count - 1) * width;
                    else
                        dst = (k + 1) * width;
                }
                else
                {
                    dst = k * width;
                }
            }
        }

        
        index =(int) (dst / width);
        Move(x, dst);
    }

    private void Move(float start, float end)
    {
        end = -end;
        Tween tween = DOTween.To(
               () => start,
               t => start = t,
               end,
               time
            );

        tween.OnUpdate
        (
             () => onMoveUpdate(start)
        );

        tween.OnComplete
        (
            () => Complete()
        );
    }

    private void onMoveUpdate(float num)
    {
        rt.anchoredPosition = new Vector2(num, rt.anchoredPosition.y);
    }

    protected virtual void Complete()
    {
        drag = true;
    }
}
