using System;
using UnityEngine;

public class MentalLink : MonoBehaviour
{
    public Action OnLinkFeedback;
    public EventHandler OnLinkSevered;

    public void LinkSever()
    {
        OnLinkSevered?.Invoke(this, EventArgs.Empty);
    }

    public void LinkFeedback()
    {
        OnLinkFeedback?.Invoke();
    }
}
