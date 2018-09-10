﻿using UnityEngine;
using System.Collections;
using Lucky;
using System;

public class GameModel : BaseInstance<GameModel> {

    private DateTime start;
    public DateTime Start
    {
        get { return start; }
        set { start = value; }
    }

    public GameModel()
    {
        start = new DateTime(DateTime.Now.Year, 2, 4, 9, 0, 0);
    }

}
