using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private GameObject handCards;
    private GameObject deck;
    private Vector3 initPos;

    void Start()
    {
        handCards = GameObject.Find("HandCards");
        deck = GameObject.Find("Deck");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("begin drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0) / 110;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        EventBus.Publish<CardDropEvent>(new CardDropEvent(gameObject, initPos));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("pointer down");
        initPos = transform.position;
    }
}

public class CardDropEvent
{
    public GameObject card;
    public Vector3 initPos;
    public CardDropEvent(GameObject _card, Vector3 _initPos) { card = _card; initPos = _initPos; }
}
