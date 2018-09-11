using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;

public class WarningView : BaseUI {

    public Text WarningText;

    private BaseAccident accidentMessage = null;
    public BaseAccident AccidentMessage
    {
        set
        {
            accidentMessage = value;
            InvalidView();
        }
    }

    protected override void UpdateView()
    {
        base.UpdateView();
        if(accidentMessage!=null)
        {
            SetDate(accidentMessage);
        }
    }

    private void SetDate(BaseAccident data)
    {
        if(data.GetType()== typeof(Accident))
        {
            Accident accident = data as Accident;
            WarningText.text = accident.duration+"";
        }
        else if(data.GetType() == typeof(AccidentWarning))
        {
            AccidentWarning warning = data as AccidentWarning;
            WarningText.text = warning.Accidentstarttime+" "+warning.min+" "+warning.max;
        }
    }
}
