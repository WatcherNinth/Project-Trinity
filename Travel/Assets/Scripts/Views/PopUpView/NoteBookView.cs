using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;
using System;

public class NoteBookView : BaseSceneEaseInOut {

    public Transform content;

    protected override void InitUI()
    {
        base.InitUI();
        InitData();
        Enter();
        OnReturnClick = delegate ()
        {
            TimeManager.instance.StartTimeManager();
        };
    }

    private bool CreateNewNote(OnePageNoteBook data)
    {
        GameObject prefab = PrefabManager.Instance.GetPrefabs(Prefabs.OneDayShow);
        GameObject panelObj = GameObject.Instantiate<GameObject>(prefab);
        panelObj.transform.SetParent(content);
        LuckyUtils.MakeIndentity(panelObj.transform);
        panelObj.GetComponent<OneDayView>().contentMessage = data;
        panelObj.SetActive(true);
        LuckyUtils.MakeIndentity(panelObj.transform);
        return false;
    }

    private void InitData()
    {
        if(NoteBookModel.Instance.noteBookList.Count==0)
        {

        }
        else
        {
            DateTime dt = NoteBookModel.Instance.noteBookList[0].time;
            foreach (OnePageNoteBook data in NoteBookModel.Instance.noteBookList)
            {
                CreateNewNote(data);
            }
        }
    }

    public void SetText(int index)
    {
        DateTime dt = NoteBookModel.Instance.noteBookList[index].time;
    }
}
