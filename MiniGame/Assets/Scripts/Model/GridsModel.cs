using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridsModel : BaseInstance<GridsModel>{

    public List<GridView> grids;

    public GridsModel()
    {
        grids = new List<GridView>();
    }

    public void Add(GridView view)
    {
        grids.Add(view);
    }

    public GridView GetGridView(int i,int j)
    {
        if(i>-1 && i<MainModel.Instance.Row && j>-1 && j<MainModel.Instance.Column)
        {
            int k = i * MainModel.Instance.Column + j;
            return grids[k];
        }
        return null;
    }

    public float size
    {
        get { return MainModel.Instance.CellSize; }
    }

}
