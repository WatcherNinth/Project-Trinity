using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class FuncView : BaseFuncView
{
    public FuncView()
    {
        SetController(new FuncViewController());
        SetModel(new FuncModel());
    }
}
