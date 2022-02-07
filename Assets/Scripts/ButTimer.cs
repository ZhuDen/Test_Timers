using UnityEngine;
using UnityEngine.UI;

public class ButTimer : MonoBehaviour
{
    public Text TextButton;
    public Button ButtonTimer;

    public void SetTextInButton (string text)
    {
        TextButton.text = text;
    }

    public void SetColorButton (Color color)
    {
        ButtonTimer.GetComponent<Image>().color = color;
    }
}
