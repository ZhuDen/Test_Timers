using UnityEngine;

public class ButtonAnimationsMove : MonoBehaviour
{
    public Animation ButtonAnimation;
    public AnimationClip AnimationShowButton, AnimationHideButton;

    public void ShowButton ()
    {
        ButtonAnimation.clip = AnimationShowButton;
        ButtonAnimation.Play();
    }

    public void HideButton()
    {
        ButtonAnimation.clip = AnimationHideButton;
        ButtonAnimation.Play();
    }
}
