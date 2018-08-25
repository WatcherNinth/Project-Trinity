using UnityEngine;
using System.Collections;

public class MainModel : BaseInstance<MainModel> {

    private float cellsize;
    public float CellSize
    {
        get { return cellsize; }
        set { cellsize = value; }
    }

    private float canvasccaler;
    public float CanvaSccaler
    {
        get { return canvasccaler; }
        set { canvasccaler = value; }
    }

    public float TrueSize
    {
        get { return cellsize; }
    }

    private int col;
    public int Column
    {
        get { return col; }
        set { col = value; }
    }

    private int row;
    public int Row
    {
        get { return row; }
        set { row = value; }
    }

    private Vector2 start;
    public Vector2 Start
    {
        get { return start; }
        set { start = value; }
    }

    private float baseLine;
    public float BaseLine
    {
        get { return baseLine; }
        set { baseLine = value; }
    }

    private Vector2 stop;
    public Vector2 Stop
    {
        get { return stop; }
        set { stop = value; }
    }
}
