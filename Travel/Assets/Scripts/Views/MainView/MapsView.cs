using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lucky;

public class MapsView : BaseUI {

    private MainContent mainContent;

    public Button BuyBtn;
    public Button GoBtn;

    public Text GoBtnText;

    private bool isPlay = false;

    private void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        mainContent = transform.parent.gameObject.GetComponent<MainContent>();
        BuyBtn.onClick.AddListener(OnClick);
        GoBtn.onClick.AddListener(OnGoClick);
        TimeManager.instance.SetMapsView(this);
        SetFirstPopUp();

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
            isPlay = true;
            TimeManager.instance.GoToNextStartTime();
            MapTrafficView.instance.SetAnimatorSpeed();
            GoBtnText.text = "休息";
        }
        else
        {
            isPlay = false;
            GoBtnText.text = "出发";
            TimeManager.instance.TimeSpeed = 1.0f;
        }
    }

    public void ChangeGoButton()
    {
        isPlay = false;
        GoBtnText.text = "出发";
        MapTrafficView.instance.SetAnimatorSpeed();
    }

    private void SetFirstPopUp()
    {
        if(UserTicketsModel.Instance.firstEnter==0)
        {
            UserTicketsModel.Instance.firstEnter = 1;
            InfoView.Show(new InfoMessage("要回家，回沈阳","任务！"));
#if UNITY_ANDROID
            PlayerPrefs.SetInt("firstenter", 1);
#endif
        }
    }



}
