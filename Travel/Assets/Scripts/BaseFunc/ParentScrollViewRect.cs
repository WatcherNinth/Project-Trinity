using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ParentScrollViewRect : ScrollRect
{
    public Text text;



    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        Debug.Log("end drag");
        //base.content.
    }

    

}
