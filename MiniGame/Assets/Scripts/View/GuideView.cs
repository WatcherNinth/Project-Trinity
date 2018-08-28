using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GuideView : MonoBehaviour {

    public GameObject leftobject;
    public GameObject rightobject;
    public GameObject bottomobject;
    public GameObject topobject;

    private RectTransform rt;

    private Image left;
    private Image right;
    private Image top;
    private Image bottom;

    private Button leftb;
    private Button rightb;
    private Button topb;
    private Button bottomb;

    public void Awake()
    {
        rt = gameObject.GetComponent<RectTransform>();

        left = leftobject.GetComponent<Image>();
        right = rightobject.GetComponent<Image>();
        top = topobject.GetComponent<Image>();
        bottom = bottomobject.GetComponent<Image>();

        leftb = leftobject.GetComponent<Button>();
        rightb = rightobject.GetComponent<Button>();
        topb = bottomobject.GetComponent<Button>();
        bottomb = topobject.GetComponent<Button>();

        leftb.onClick.AddListener(onLeftClick);
        rightb.onClick.AddListener(onRightClick);
        topb.onClick.AddListener(onTopClick);
        bottomb.onClick.AddListener(onBottomClick);
    }

    // Use this for initialization
    void Start () {

        float size = MainModel.Instance.CellSize;
        rt.sizeDelta = new Vector2(size, size);

        float offset = MainModel.Instance.CellSize / 2;
        float width = MainModel.Instance.CellSize / 3;

        Vector2 sizedelta = new Vector2(width, width);

        RectTransform trt = left.GetComponent<RectTransform>();
        trt.anchoredPosition = new Vector3(offset, 0, 0);
        trt.sizeDelta = sizedelta;

        trt = right.GetComponent<RectTransform>();
        trt.anchoredPosition = new Vector3(-offset, 0, 0);
        trt.sizeDelta = sizedelta;

        trt = top.GetComponent<RectTransform>();
        trt.anchoredPosition = new Vector3(0, offset, 0);
        trt.sizeDelta = sizedelta;

        trt = bottom.GetComponent<RectTransform>();
        trt.anchoredPosition = new Vector3(0, -offset, 0);
        trt.sizeDelta = sizedelta;

    }

    public void onLeftClick()
    {

    }

    public void onRightClick()
    {

    }

    public void onTopClick()
    {

    }

    public void onBottomClick()
    {

    }

}
