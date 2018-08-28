using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class StartView : BaseFuncView{

    public StartModel sModel;

    public StartView()
    {
        firstDrag = false;
        sModel = new StartModel();
        SetController(new BaseFuncController());
        SetModel(sModel);
    }

    public void Init(Vector3 position, float size, GridView gv)
    {
        transform.SetParent(UnderCanvas);
        transform.position = position;
        GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
        transform.localScale = Vector3.one;
        gameObject.SetActive(true);

        gridView = gv;
        last = position;
    }

    public void SetNum(int i)
    {
        SetText(Convert.ToString(i));
        sModel.Num = i;
    }

}
