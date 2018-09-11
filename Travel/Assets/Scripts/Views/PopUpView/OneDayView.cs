using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lucky;

public class ContentMessage
{
    public Sprite image;
    public string FirstText;
    public string FirstChosen;
    public string SecondChosen;
    public string ThirdChosen = null;
    public string SecondText_1;
    public string SecondText_2;
    public string SecondText_3 = null;
}

public class OneDayView : BaseUI {

    public Image DayImage;
    public Text FirstText;
    public Button FirstButton;
    public Text FirstChosen;
    public Button SecondButton;
    public Text SecondChosen;
    public Button ThirdButton;
    public Text ThirdChosen;
    public Text SecondText;

    private ContentMessage m_contentMessage = null;
    public ContentMessage contentMessage
    {
        set { m_contentMessage = value; }
    }

    protected override void Start()
    {
        base.Start();
        SecondText.text = "";
    }

    protected override void UpdateView()
    {
        base.UpdateView();
        if (m_contentMessage != null)
            SetData(m_contentMessage);
    }

    private void SetData(ContentMessage data)
    {
        DayImage.sprite = data.image;
        FirstText.text = data.FirstText;
        FirstChosen.text = data.FirstChosen;
        SecondChosen.text = data.SecondChosen;

        InitEvent(data);

        InvalidView();
    }

    private void InitEvent(ContentMessage data)
    {
        FirstButton.onClick.AddListener(delegate ()
        {
            SecondText.text = data.SecondText_1;
            SecondButton.gameObject.SetActive(false);
            ThirdButton.gameObject.SetActive(false);
            FirstButton.onClick.RemoveAllListeners();
        });

        SecondButton.onClick.AddListener(delegate ()
        {
            SecondText.text = data.SecondText_2;
            FirstButton.gameObject.SetActive(false);
            ThirdButton.gameObject.SetActive(false);
            SecondButton.onClick.RemoveAllListeners();
        });


        if (string.IsNullOrEmpty(data.ThirdChosen))
        {
            ThirdButton.gameObject.SetActive(false);
        }
        else
        {
            ThirdButton.gameObject.SetActive(true);
            ThirdChosen.text = data.ThirdChosen;
            ThirdButton.onClick.AddListener(delegate ()
            {
                SecondText.text = data.SecondText_3;
                FirstButton.gameObject.SetActive(false);
                SecondButton.gameObject.SetActive(false);
                ThirdButton.onClick.RemoveAllListeners();
            });
        }
    }
}
