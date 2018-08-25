using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseFuncView : MonoBehaviour,IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerUpHandler
{

    public Transform canvas;
    public Transform UnderCanvas;
    public Image left;
    public Image right;
    public Image bottom;
    public Image top;
    public Text text;
    public GameObject RedSquare;

    protected bool firstDrag = true;
    private bool isDrag = false;
    
    private RectTransform rt;
    protected BaseFuncController controller;


    public void Awake()
    {
        
        rt = gameObject.GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetDraggedPosition(eventData);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDrag = true;
        if (firstDrag)
        {
            GameObject itself = Instantiate(gameObject);
            RectTransform itrt = itself.GetComponent<RectTransform>();
            RectTransform rt = GetComponent<RectTransform>();

            itself.transform.SetParent(transform.parent, false);
            itself.transform.SetAsFirstSibling();
            itrt.transform.position = rt.transform.position;
            itrt.transform.rotation = rt.transform.rotation;
            itrt.transform.localScale = Vector3.one;
            itself.SetActive(true);

            transform.SetParent(canvas, false);
            transform.SetSiblingIndex(canvas.childCount - 1);
            rt.anchorMax = new Vector2(0, 0);
            rt.anchorMin = new Vector2(0, 0);

            rt.sizeDelta = new Vector2(MainModel.Instance.CellSize, MainModel.Instance.CellSize);

            firstDrag = false;
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
            if (gv != null)
            {
                transform.SetParent(UnderCanvas);
                transform.SetSiblingIndex(UnderCanvas.childCount - 1);
                transform.position = gv.model.Position;
            }
            if (RedSquare.activeSelf)
                RedSquare.SetActive(false);
        }

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDrag)
        {
            left.gameObject.SetActive(!left.gameObject.activeSelf);
            right.gameObject.SetActive(!right.gameObject.activeSelf);
            top.gameObject.SetActive(!top.gameObject.activeSelf);
            bottom.gameObject.SetActive(!bottom.gameObject.activeSelf);

        }
        else
            isDrag = false;

    }

    private void SetDraggedPosition(PointerEventData eventData)
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
                RedSquare.transform.position = gv.model.Position;
                RedSquare.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
                if (!RedSquare.activeSelf)
                    RedSquare.SetActive(true);
            }
            else
            {
                if(RedSquare.activeSelf)
                    RedSquare.SetActive(false);
            }
          
        }
    }

    public void SetController(BaseFuncController con)
    {
        controller = con;
    }

    public void SetText(string s)
    {
        text.text = s;
    }
}
