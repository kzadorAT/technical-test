using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextboxController : MonoBehaviour
{
    public TMP_Text text;
    public int group;
    public int index;

    public List<TextboxController> contacts = new();


    public void Initialize(int index)
    {
        this.index = index;
        text.text = group == 1 ? GameController.Instance.group1[index] : GameController.Instance.group2[index];        
    }

    public void OnCollisionEnter(Collision collision)
    {        
        if (collision.gameObject.CompareTag("Group2"))
        {
            Debug.Log("Collision");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Group2") && !gameObject.CompareTag("Group2"))
        {
            contacts.Add(other.gameObject.GetComponent<TextboxController>());
            if(contacts.Count == 1)
            {
                contacts[0].transform.localScale = Vector3.one * 1.25f;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Group2") && !gameObject.CompareTag("Group2"))
        {
            other.transform.localScale = Vector3.one;
            contacts.Remove(other.gameObject.GetComponent<TextboxController>());
            if (contacts.Count > 0)
            {
                contacts[0].transform.localScale = Vector3.one * 1.25f;
            }
        }
    }
}
