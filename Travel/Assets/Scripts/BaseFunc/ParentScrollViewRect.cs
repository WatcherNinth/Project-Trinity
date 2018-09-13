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
    protected int index;

    private float time = 0.3f;
    private float width;
    private RectTransform rt;

    private bool drag = true;
    private float firstx;
    private Direction direct;

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
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (drag)
        {
            base.OnBeginDrag(eventData);
            firstx = rt.anchoredPosition.x;
        }

    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (drag)
            base.OnDrag(eventData);
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

        int k = (int)(-firstx / width);
        float move = Mathf.Abs(x - firstx);
        float dst = 0;
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
            if(move * 2 > width)
            {
                int count=base.content.transform.childCount;
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
