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
        public int number;
    }

    public int Col;
    public int Row;
    public GameObject prefab;
    public int linewidth;
    public Color color;

    public GameObject start;
    public Pair[] points;

    public GameObject train;

    public GameObject line;

    private int All;
    private GridLayoutGroup gridlayoutgroup;
    private RectTransform rt;

    void Awake()
    {
        gridlayoutgroup = GetComponent<GridLayoutGroup>();
        rt = GetComponent<RectTransform>();
        Col *= 5;
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
        
        float cellwidth = width / Col;
        MainModel.Instance.CellSize = cellwidth;

        float height = cellwidth * Row;
        MainModel.Instance.BaseLine = rt.sizeDelta.y - height;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, height);

        gridlayoutgroup.cellSize = new Vector2(cellwidth, cellwidth);

        for (int i = 0; i < All; i++)
        {
            GameObject temp = Instantiate(prefab);
            temp.transform.SetParent(transform);
            GridView gv = temp.GetComponent<GridView>();
            gv.SetWidth(linewidth / 2, color);
            GridsModel.Instance.Add(gv);
        }

        yield return null;

        foreach(GridView gv in GridsModel.Instance.grids)
        {
            gv.SetPosition();
        }

        float size = GridsModel.Instance.size;
        for (int i = 0; i < points.Length; i++)
        {
            GameObject tempStart = Instantiate(start);
            GridView gv = GridsModel.Instance.GetGridView(points[i].i, points[i].j);
            int num = points[i].number;

            tempStart.transform.SetParent(transform.parent);
            tempStart.transform.GetChild(0).GetComponent<Text>().text = num + "";
            tempStart.transform.position = gv.model.Position;
            tempStart.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
            tempStart.transform.localScale = Vector3.one;
            tempStart.SetActive(true);

        }
    }
	
}
