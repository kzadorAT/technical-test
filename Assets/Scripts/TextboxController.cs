using TMPro;
using UnityEngine;

public class TextboxController : MonoBehaviour
{
    public TMP_Text text;
    public int group;
    public int index;

    public void Initialize(int group, int index)
    {
        this.group = group;
        this.index = index;
        text.text = group == 1 ? GameController.Instance.group1[index] : GameController.Instance.group2[index];
    }
}
