using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public Transform target;
    public Vector3 startPosition;

    public bool shouldMove;
    public bool shouldReturn;

    // private void Awake()
    // {
    //     target.gameObject.SetActive(false);
    // }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (shouldMove)
        {            
            startPosition = target.GetComponent<RectTransform>().anchoredPosition;
            target.transform.position = transform.position;
            target.localScale = Vector3.one * 0.75f;
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (shouldMove)
        {
            target.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        if (shouldReturn)
        {
            // target.transform.position = target.parent.position;
            target.GetComponent<RectTransform>().anchoredPosition = startPosition;
            target.localScale = Vector3.one;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Debug.Log("OnDrop");
        // if (shouldReturn)
        // {
        //     target.transform.position = target.parent.position;
        //     target.gameObject.SetActive(true);
        // }
    }


}
