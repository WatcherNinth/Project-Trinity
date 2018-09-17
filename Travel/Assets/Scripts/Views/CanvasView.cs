using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using Lucky;
using System.Collections.Generic;

public class CanvasView : MonoBehaviour {

    public Button NoteBtn;
    public Image RedPoints;

    private Vector3 dst = new Vector3(395, 644, 0);
    private Vector3 src = new Vector3(546, 369, 0);

    private bool isOn = false;
    private NoteBookView nbv = null;

    void Start()
    {
        RegisterMsg(true);
        AudioManager.Instance.PlayMusic(Audios.BG, true);
        NoteBtn.onClick.AddListener(delegate() 
        {
            Show();
            /*
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
            */
            //RedPoints.gameObject.SetActive(false);

        });
    }

    private void OnDestroy()
    {
        RegisterMsg(false);
    }

    private void Show()
    {
        GameObject go = PopUpManager.Instance.AddUiLayerPopUp(Prefabs.NoteBook);
        nbv = go.GetComponent<NoteBookView>();
        PopUpManager.Instance.SetPopupPanelAutoClose(go);
        isOn = true;
    }

    private void RegisterMsg(bool isOn)
    {
        if (isOn)
        {
            MessageBus.Register<OnePageNoteBook>(AddNote);
        }
        else
        {
            MessageBus.UnRegister<OnePageNoteBook>(AddNote);
        }
    }

    private bool AddNote(OnePageNoteBook data)
    {
        Debug.Log("add note");
        //RedPoints.gameObject.SetActive(true);
        NoteBookModel.Instance.noteBookList.Add(data);
        Show();
        TimeManager.instance.SetNormalSpeed();
        return false;
    }

    
}
