using UnityEngine;
using System.Collections;

public enum Type
{
    None,
    Func,
    Path,
    Num
}

public enum Direction
{
    Left = 0,
    Right = 1,
    Top = 2 ,
    Bottom = 3
}

public class GridModel {

    private Vector3 pos;
    public Vector3 Position
    {
        get { return pos; }
        set { pos = value; }
    }

    private Type occupancy;
    public Type Occupancy
    {
        get { return occupancy; }
        set { occupancy = value; }
    }

    private bool[] directions = new bool[4];

    public bool Top
    {
        get { return directions[(int)Direction.Top]; }
        set { directions[(int)Direction.Top] = value; }
    }

    public bool Bottom
    {
        get { return directions[(int)Direction.Bottom]; }
        set { directions[(int)Direction.Bottom] = value; }
    }

    public bool Left
    {
        get { return directions[(int)Direction.Left]; }
        set { directions[(int)Direction.Left] = value; }
    }

    public bool[] Directions
    {
        get { return directions; }
    }

    public bool Right
    {
        get { return directions[(int)Direction.Right]; }
        set { directions[(int)Direction.Right] = value; }
    }

    public GridModel()
    {
        for (int i = 0; i < 4; i++)
            directions[i] = false;
        occupancy = Type.None;
    }

}
