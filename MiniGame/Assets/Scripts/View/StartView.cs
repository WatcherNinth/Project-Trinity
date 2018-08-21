using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class StartView : BaseFuncView{

    public StartView()
    {
        firstDrag = false;
        SetController(new BaseFuncController());
    }

}
