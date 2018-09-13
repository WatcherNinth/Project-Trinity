using UnityEngine;
using System.Collections;

public class NoteBookScrollView : ParentScrollViewRect
{
    private NoteBookView nbv;

    protected override void Awake()
    {
        base.Awake();
        nbv = GetComponentInParent<NoteBookView>();
    }

    protected override void Complete()
    {
        base.Complete();
        
        if(nbv!=null)
        {
            nbv.SetText(index);
        }
        
    }
}
