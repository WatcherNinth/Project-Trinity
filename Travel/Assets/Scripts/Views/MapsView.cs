using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lucky;
using HedgehogTeam.EasyTouch;

public class MapsView : BaseUI {

    private MainContent mainContent;

    public Button Btn;

    private void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    void Start () {
        mainContent = transform.parent.gameObject.GetComponent<MainContent>();
        Btn.onClick.AddListener(OnClick);
	}

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void OnClick()
    {
        mainContent.ShowView(ViewID.BuyTickets);
    }



}
