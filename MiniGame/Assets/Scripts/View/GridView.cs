using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GridView : MonoBehaviour {

    public Image left;
    public Image right;
    public Image top;
    public Image bottom;

    public float width;
    public Color color;
    public GridModel model;

    private void Awake()
    {
        model = new GridModel();
    }

    private void Start()
    {
        GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public void SetWidth(float twidth,Color tcolor)
    {
        width = twidth;
        color = tcolor;
        SetWidthOrHeight(left);
        SetWidthOrHeight(right);
        SetWidthOrHeight(top, false);
        SetWidthOrHeight(bottom, false);
    }

    private void SetWidthOrHeight(Image image, bool w = true)
    {
        RectTransform rt = image.GetComponent<RectTransform>();
        if(w)
            rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
        else
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, width);
        image.color = color;
    }

    public void SetColor(Color color)
    {
        left.color = color;
        right.color = color;
        top.color = color;
        bottom.color = color;
    }

    public void SetPosition()
    {
        model.Position = transform.position;
    }

    public void OnClick()
    {
        Debug.Log(transform.position);
    }

}
