using UnityEngine;
using System.Collections;
using Lucky;

public enum ViewID
{
    Maps = 0,
    BuyTickets = 1,
    WeChat = 2,
    None
}

public class MainContent : MonoBehaviour {

    [SerializeField]
    private string[] mChildViews;

    private BaseUI[] baseUI;
    private ViewID nowUI;

    private void Awake()
    {
        baseUI = new BaseUI[mChildViews.Length];
        nowUI = ViewID.None;
    }

    void Start()
    {
        //打开第一个分
        ShowView(ViewID.Maps);
    }

    public void ShowView(ViewID index)
    {
        var view = GetView(index);
        if (view)
        {
            view.Show();
            if(nowUI != ViewID.None)
            {
                HideView(nowUI);
            }
            nowUI = index;

        }
    }

    public void HideView(ViewID index)
    {
        var view = GetView(index);
        if (view)
        {
            view.Hide();
        }
    }

    private BaseUI GetView(ViewID index, bool autoCreate = true)
    {
        int i = (int)index;
        BaseUI view = null;
        if(baseUI[i] == null)
        {
            view = LuckyUtils.CreatePanelFromResource<BaseUI>("Prefabs/" + mChildViews[i], transform);

            if (view)
            {
                LuckyUtils.MakeFullStretch(view.transform);
                baseUI[i] = view;
            }
        }

        view = baseUI[i];

        return view;
    }


}
