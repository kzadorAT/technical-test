using System.Collections;
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

        if(target.GetComponent<TextboxController>().contacts.Count > 0)
        {
            Debug.Log("Comparar elementos");
        }

        if (shouldReturn)
        {
            // target.transform.position = target.parent.position;
            // target.GetComponent<RectTransform>().anchoredPosition = startPosition;
            // target.localScale = Vector3.one;

            StartCoroutine(AnimateReturn(target, startPosition, Vector3.one, 0.5f));
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

    private IEnumerator AnimateReturn(Transform target, Vector3 returnPosition, Vector3 originalScale, float duration)
    {
        float elapsedTime = 0f;

        Vector3 initialPosition = target.GetComponent<RectTransform>().anchoredPosition;
        Vector3 initialScale = target.localScale;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            target.GetComponent<RectTransform>().anchoredPosition = Vector3.Lerp(initialPosition, returnPosition, t);
            target.localScale = Vector3.Lerp(initialScale, originalScale, t);

            yield return null;
        }

        target.GetComponent<RectTransform>().anchoredPosition = returnPosition;
        target.localScale = originalScale;
    }
}
