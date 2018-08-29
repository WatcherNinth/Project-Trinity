using UnityEngine;
using System.Collections;
using Lucky;
using UnityEngine.UI;

public class BuyTickets : BaseUI {

    public Button back;

    public void Awake()
    {
        BuyTicketsModel.Instance.start = "abc";   
    }

}
