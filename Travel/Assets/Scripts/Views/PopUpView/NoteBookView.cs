using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;

public class NoteBookView : BaseSceneEaseInOut {

    public Text DateShow;
    public Transform content;

    protected override void InitUI()
    {
        base.InitUI();

        Enter();
        CreateNewNote(null);
        CreateNewNote(null);
    }

    public bool CreateNewNote(OnePageNoteBook data)
    {
        GameObject prefab = PrefabManager.Instance.GetPrefabs(Prefabs.OneDayShow);
        GameObject panelObj = GameObject.Instantiate<GameObject>(prefab);
        panelObj.transform.SetParent(content);
        LuckyUtils.MakeIndentity(panelObj.transform);
        //panelObj.GetComponent<OneDayView>().contentMessage = data;
        panelObj.SetActive(true);
        return false;
    }
}
