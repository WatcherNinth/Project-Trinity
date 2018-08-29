using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lucky;

public class MapsView : BaseUI {

    private MainContent mainContent;

    public Button Btn;

    // Use this for initialization
    void Start () {
        mainContent = transform.parent.gameObject.GetComponent<MainContent>();

        Btn.onClick.AddListener(OnClick);
	}
	
    public void OnClick()
    {
        mainContent.ShowView(ViewID.BuyTickets);
    }


}
