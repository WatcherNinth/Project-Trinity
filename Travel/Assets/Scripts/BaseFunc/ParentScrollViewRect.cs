using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ParentScrollViewRect : ScrollRect
{
    public Text text;

    private float time = 0.3f;
    private int width = 940;
    private RectTransform rt;

    private bool drag = true;

    protected override void Awake()
    {
        base.Awake();
        rt = content.GetComponent<RectTransform>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if(drag)
            base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if(drag)
            base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        int x = (int)base.content.GetComponent<RectTransform>().anchoredPosition.x;

        drag = false;

        //x = Mathf.Abs(x);
        int i = x / width;
        int temp = x % width;
        Debug.Log("temp " + temp);
        float tx = x;
        if(Mathf.Abs(temp) < width/2)
        {
            float dst = tx - temp;
            Debug.Log("last start " + x + " dst " + dst);
            Tween tween = DOTween.To(
               () => tx,
               t => tx = t,
               dst,
               time
           );

            tween.OnUpdate
            (
                 () => onMoveUpdate(tx)
            );

            tween.OnComplete
            (
                () => Complete()
            );

        }
        else
        {
            float dst = x - (width + temp);
            Debug.Log("next start " + x + " dst " + dst);
            Tween tween = DOTween.To(
               () => tx,
               t => tx = t,
               dst,
               time
           );

            tween.OnUpdate
            (
                 () => onMoveUpdate(tx)
            );

            tween.OnComplete
            (
                () => Complete()
            );
        } 
        

    }

    private void onMoveUpdate(float num)
    {
        Debug.Log("num " + num);
        rt.anchoredPosition = new Vector2(num, rt.anchoredPosition.y);
    }

    private void Complete()
    {
        drag = true;
    }

    

}
