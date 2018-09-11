using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ParentScrollViewRect : ScrollRect
{
    public Text text;

    private float time = 0.3f;
    private float width=940.0f;
    private RectTransform rt;

    protected override void Awake()
    {
        base.Awake();
        rt = content.GetComponent<RectTransform>();
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        float x = base.content.GetComponent<RectTransform>().anchoredPosition.x;
        x = Mathf.Abs(x);
        int i = (int)(x / width);
        float temp = x % width;
        Debug.Log("temp " + temp);
        if(temp < width/2)
        {
            float dst = x - temp;
            Tween tween = DOTween.To(
               () => x,
               t => x = t,
               dst,
               time
           );

            tween.OnUpdate
            (
                 () => onMoveUpdate(x)
            );

        }
        else
        {
            float dst = x + width - temp;
            Tween tween = DOTween.To(
               () => x,
               t => x = t,
               dst,
               time
           );

            tween.OnUpdate
            (
                 () => onMoveUpdate(x)
            );
        } 

    }

    private void onMoveUpdate(float num)
    {
        Debug.Log("num " + num);
        rt.anchoredPosition = new Vector2(-num, rt.anchoredPosition.y);
    }

    

}
