using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Lucky;
using System.Collections.Generic;

public class OneDayView : BaseUI {

    public Image DayImage;
    public Text FirstText;

    public List<Button> buttons;
    public List<Text> texts;
    public Text SecondText;

    private OnePageNoteBook m_contentMessage = null;
    public OnePageNoteBook contentMessage
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

    private void SetData(OnePageNoteBook data)
    {
        DayImage.sprite = SpriteManager.Instance.GetSprite(data.imagePath);
        FirstText.text = data.FirstText;

        InitEvent(data);

        InvalidView();
    }

    private void InitEvent(OnePageNoteBook data)
    {
        if(data.chosen==-1)
        {
            int count = data.buttontext.Count;
            for (int i = 0; i < buttons.Count; i++)
            {
                if (i < count)
                {
                    buttons[i].gameObject.SetActive(true);
                    texts[i].text = data.buttontext[i];
                    int temp = i;
                    buttons[i].onClick.AddListener(delegate ()
                    {
                        for (int j = 0; j < buttons.Count; j++)
                        {
                            if (j == temp)
                                continue;
                            else
                                buttons[j].gameObject.SetActive(false);
                        }
                        SecondText.text = data.finaltext[temp];
                        data.chosen = temp;
                    });
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            for (int i = 0; i < buttons.Count; i++)
            {
                if(i == data.chosen)
                {
                    buttons[i].gameObject.SetActive(true);
                    texts[i].text = data.buttontext[i];
                    SecondText.text = data.finaltext[i];
                }
                else
                {
                    buttons[i].gameObject.SetActive(false);
                }
            }
        }
    }
}
