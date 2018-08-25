using UnityEngine;
using System.Collections;

public class BaseFuncController
{
    public BaseFuncController()
    {
        
    }

    public void GetGrid(Vector3 position, ref int col, ref int row)
    {
        float x = 0;
        float y = 0;
        float cellsize = MainModel.Instance.CellSize;

        float eventx = position.x;
        float eventy = position.y - MainModel.Instance.BaseLine;

        col = (int)(eventx / cellsize);
        row = (int)(eventy / cellsize);

        row = MainModel.Instance.Row - 1 - row;
    }


}
