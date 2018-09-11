using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lucky;

public class MapsView : BaseUI {

    private MainContent mainContent;

    public Button BuyBtn;
    public Button GoBtn;

    private bool isPlay = false;

    private void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    void Start () {
        mainContent = transform.parent.gameObject.GetComponent<MainContent>();
        BuyBtn.onClick.AddListener(OnClick);
        GoBtn.onClick.AddListener(OnGoClick);
	}

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void OnClick()
    {
        mainContent.ShowView(ViewID.BuyTickets);
    }

    private void OnGoClick()
    {
        if(!isPlay)
        {
            TimeManager.instance.GoToNextStartTime();
            TimeManager.instance.TimeSpeed = 1;
            isPlay = true; 
        }
        else
        {
            TimeManager.instance.TimeSpeed = 0;
            isPlay = false;
        }
    }



}
