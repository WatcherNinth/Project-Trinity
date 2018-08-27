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
        float scaler = MainModel.Instance.CanvaSccaler;
        cellsize = cellsize * scaler;
        float eventx = position.x;
        float eventy = position.y;

        row = (int)(eventx / cellsize);
        col = (int)(eventy / cellsize);
        col = 8 - col;
    }


}
