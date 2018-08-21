using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MainView : MonoBehaviour {

    [Serializable]
    public class Pair
    {
        public int i;
        public int j;
    }

    public Transform canvas;

    public int Col;
    public GameObject prefab;
    public int linewidth;
    public Color color;

    public GameObject start;
    public Pair[] points;
    public int[] number;

    public GameObject train;

    public GameObject line;

    private int Row;
    private int All;
    private GridLayoutGroup gridlayoutgroup;
    private RectTransform rt;

    void Awake()
    {
        gridlayoutgroup = GetComponent<GridLayoutGroup>();
        rt = GetComponent<RectTransform>();
        Row = 9 * Col / 16;
        All = Row * Col;
        MainModel.Instance.Column = Col;
        MainModel.Instance.Row = Row;
        MainController.Instance.SetView(this);
    }

	// Use this for initialization
	void Start () {

        StartCoroutine(Init());
        
	}

    public IEnumerator Init()
    {
        float width = rt.sizeDelta.x;
        float height = rt.sizeDelta.y;
        float cellwidth = width / Col;
        float cellheight = height / Row;
        MainModel.Instance.CellSize = cellheight;
        MainModel.Instance.CanvaSccaler = canvas.localScale.x;
        float truesize = MainModel.Instance.TrueSize;
        gridlayoutgroup.cellSize = new Vector2(cellwidth, cellheight);

        for (int i = 0; i < All; i++)
        {
            GameObject temp = Instantiate(prefab);
            temp.transform.SetParent(transform);
            GridView gv = temp.GetComponent<GridView>();
            gv.SetWidth(linewidth / 2, color);
            GridsModel.Instance.Add(gv);
        }

        yield return null;
        yield return null;

        foreach(GridView gv in GridsModel.Instance.grids)
        {
            gv.SetPosition();
        }

        float size = GridsModel.Instance.size;
        for (int i = 0; i < number.Length; i++)
        {
            GameObject tempStart = Instantiate(start);
            GridView gv = GridsModel.Instance.GetGridView(points[i].i, points[i].j);
            int num = number[i];

            tempStart.transform.SetParent(transform.parent);
            tempStart.transform.GetChild(0).GetComponent<Text>().text = num + "";
            tempStart.transform.position = gv.model.Position;
            tempStart.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
            tempStart.transform.localScale = Vector3.one;
            tempStart.SetActive(true);

        }
    }
	

    public void BuildLine()
    {
        Vector2 start = MainModel.Instance.Start;
        Vector2 stop = MainModel.Instance.Stop;

        GameObject temp = Instantiate(line);
        line.transform.SetParent(transform.parent);

        if (start.y==stop.y )
        {
            float offset = start.x - stop.x;
            RectTransform rt = line.GetComponent<RectTransform>();
            float width = Mathf.Abs(offset) - MainModel.Instance.TrueSize;
            rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
            if (offset<0)
                line.transform.position =new Vector3(start.x+ MainModel.Instance.TrueSize/2, start.y,0);
            else
                line.transform.position = new Vector3(start.x - MainModel.Instance.TrueSize / 2, start.y, 0);

        }
        else if(start.x == stop.x)
        {
            float offset = start.y - stop.y;
            RectTransform rt = line.GetComponent<RectTransform>();
            float height = Mathf.Abs(offset) - MainModel.Instance.TrueSize;
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);
            if(offset<0)
                line.transform.position = new Vector3(start.y + MainModel.Instance.TrueSize / 2, start.y, 0);
            else
                line.transform.position = new Vector3(start.y - MainModel.Instance.TrueSize / 2, start.y, 0);

        }
        else
        {
            Vector2 middle = new Vector2(stop.x, start.y);
        }
        
        
    }
}
