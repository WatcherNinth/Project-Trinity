using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Lucky;

public class CanvasView : MonoBehaviour {

    public Button NoteBtn;

    private Vector3 dst = new Vector3(395, 644, 0);
    private Vector3 src = new Vector3(546, 369, 0);

    private bool isOn = false;
    private NoteBookView nbv = null;

    void Start()
    {
        NoteBtn.onClick.AddListener(delegate() 
        {
            if(!isOn)
            {
                Tweener tween = NoteBtn.transform.DOMove(dst, 0.3f);
                tween.OnComplete
                (
                    () => Show()
                );
            }
            else
            {
                if(nbv!=null)
                {
                    nbv.Dispose();
                    Tweener tween = NoteBtn.transform.DOMove(src, 1);
                }
            }
            
        });
    }

    private void Show()
    {
        GameObject go = PopUpManager.Instance.AddUiLayerPopUp(Prefabs.NoteBook);
        nbv = go.GetComponent<NoteBookView>();
        PopUpManager.Instance.SetPopupPanelAutoClose(go);
        isOn = true;
    }

    
}
