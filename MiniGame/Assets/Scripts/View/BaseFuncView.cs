using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseFuncView : MonoBehaviour,IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerUpHandler
{

    public Transform canvas;
    public Transform UnderCanvas;
    public GameObject leftobject;
    public GameObject rightobject;
    public GameObject bottomobject;
    public GameObject topobject;
    public Text text;

    public GridView RedSquare;
    public GuideView guideView;

    protected bool firstDrag = true;
    private bool isDrag = false;
    private bool isShow = false;
   
    protected BaseFuncController controller;
    protected BaseFuncModel model;
    protected GridView gridView = null;
    protected Vector3 last;

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

    public void Start()
    {
        float offset = MainModel.Instance.CellSize/2;
        float width = MainModel.Instance.CellSize / 3;

        Vector2 sizedelta = new Vector2(width, width);

        RectTransform trt = left.GetComponent<RectTransform>();
        trt.anchoredPosition = new Vector3(offset, 0,0);
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;

        if (firstDrag)
        {
            //第一次拖动
            //复制一个原来的GameObject在原来的地方
            GameObject itself = Instantiate(gameObject);
            RectTransform itrt = itself.GetComponent<RectTransform>();
            RectTransform rt = GetComponent<RectTransform>();

            itself.transform.SetParent(transform.parent, false);
            itself.transform.SetAsFirstSibling();
            itrt.transform.position = rt.transform.position;
            itrt.transform.rotation = rt.transform.rotation;
            itrt.transform.localScale = Vector3.one;
            itself.SetActive(true);

            //初始化自身设置
            transform.SetParent(canvas, false);
            transform.SetSiblingIndex(canvas.childCount - 1);
            rt.anchorMax = new Vector2(0, 0);
            rt.anchorMin = new Vector2(0, 0);

            rt.sizeDelta = new Vector2(MainModel.Instance.CellSize, MainModel.Instance.CellSize);

            firstDrag = false;

        }
        else
        {
            //第二次拖动
            gridView.model.Occupancy = Type.None;
        }

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            int row = 0;
            int col = 0;
            float size = GridsModel.Instance.size;
            float truesize = MainModel.Instance.TrueSize;
            rt.position = globalMousePos - new Vector3(truesize / 2, truesize / 2, 0);
            controller.GetGrid(globalMousePos, ref col, ref row);
            GridView gv = GridsModel.Instance.GetGridView(row, col);
            if (gv != null)
            {
                //获取到对应的方格
                if(gv.model.Occupancy==Type.None)
                {
                    //没有占用，记录这次的位置
                    RedSquare.SetColor(Color.green);
                    last = gv.model.Position;
                    gridView = gv;
                }
                else
                {
                    //被占用
                    RedSquare.SetColor(Color.red);
                }
                RedSquare.transform.position = gv.model.Position;
                RedSquare.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
                if (!RedSquare.gameObject.activeSelf)
                    RedSquare.gameObject.SetActive(true);

            }
            else
            {
                //获取不到对应的方格
                if (RedSquare.gameObject.activeSelf)
                    RedSquare.gameObject.SetActive(false);
            }

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            int row = 0;
            int col = 0;
            controller.GetGrid(globalMousePos,ref col,ref row);
            GridView gv = GridsModel.Instance.GetGridView(row, col);

            transform.SetParent(UnderCanvas);
            transform.SetSiblingIndex(UnderCanvas.childCount - 1);

            if (gv != null)
            {
                if (gv.model.Occupancy==Type.None)
                {
                    //方格为空
                    gridView = gv;
                    transform.position = gv.model.Position;
                    gridView.model.Occupancy = model.OccupancyType;
                }
                else
                {
                    //方格不为空
                    transform.position = last;
                }
            }
            else
            {
                //使用上一次遇到的方格
                if (gridView != null)
                {
                    gridView.model.Occupancy = model.OccupancyType;
                    transform.position = last;
                } 
                else
                    Destroy(gameObject);
            }

            if (RedSquare.gameObject.activeSelf)
                RedSquare.gameObject.SetActive(false);
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDrag)
        {
            if (!isShow)
            {
                Vector3 globalMousePos;
                if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rt, eventData.position, eventData.pressEventCamera, out globalMousePos))
                {
                    ShowButton(globalMousePos);
                }
            }
            else
                HideButton();

        }
        else
            isDrag = false;

    }

    public bool isShowUp(int row, int col,Direction direct)
    {
        GridModel gm = null;
        switch(direct)
        {
            case Direction.Top:
                gm = GridsModel.Instance.GetGridView(row - 1, col).model;
                break;
            case Direction.Bottom:
                gm = GridsModel.Instance.GetGridView(row + 1, col).model;
                break;
            case Direction.Left:
                gm = GridsModel.Instance.GetGridView(row, col -1).model;
                break;
            case Direction.Right:
                gm = GridsModel.Instance.GetGridView(row, col + 1).model;
                break;
        }
        if (gm.Occupancy == Type.None)
            return true;
        else if (gm.Occupancy == Type.Path && gm.Directions[(int)direct])
            return true;
        else
            return false;
    }

    public void SetController(BaseFuncController con)
    {
        controller = con;
    }

    public void SetModel(BaseFuncModel mod)
    {
        model = mod;
    }

    public void SetText(string s)
    {
        text.text = s;
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

    public void HideButton()
    {
        if (leftobject.activeSelf)
            leftobject.SetActive(false);
        if (rightobject.activeSelf)
            rightobject.SetActive(false);
        if (topobject.activeSelf)
            topobject.SetActive(false);
        if (bottomobject.activeSelf)
            bottomobject.SetActive(false);
    }

    public void ShowButton(Vector3 globalMousePos)
    {
        int row = 0;
        int col = 0;
        controller.GetGrid(globalMousePos, ref col, ref row);
        if (isShowUp(row, col, Direction.Left))
            leftobject.SetActive(true);
        if (isShowUp(row, col, Direction.Right))
            rightobject.SetActive(true);
        if (isShowUp(row, col, Direction.Bottom))
            bottomobject.SetActive(true);
        if (isShowUp(row, col, Direction.Top))
            topobject.SetActive(true);
    }
}
