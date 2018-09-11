using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DayView : MonoBehaviour {

    public Text Day;
    private Image colori;
    private Button btn;

    public Button Btn
    {
        get { return btn; }
    }

    private void Awake()
    {
        colori = GetComponent<Image>();
        btn = GetComponent<Button>();
    }

    public void SetColor(Color color)
    {
        colori.color = color;
    }
}
