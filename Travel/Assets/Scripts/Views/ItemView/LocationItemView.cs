using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class LocationItemView : MonoBehaviour, IPointerClickHandler
{

    private MainContent mc;

    private void Awake()
    {
        mc = GameObject.FindGameObjectWithTag("MainContent").GetComponent<MainContent>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        mc.ShowView(ViewID.BuyTickets);
    }
}
