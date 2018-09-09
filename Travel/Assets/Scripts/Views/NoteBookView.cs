using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;

public class NoteBookView : BaseSceneEaseInOut {

    public Text DateShow;

    protected override void InitUI()
    {
        base.InitUI();

        Enter();
    }
}
