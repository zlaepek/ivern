using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaEvent
{
    #region VARIABLES
    private string gachaEventName = "¿œπ› ∞°√≠";

    private DateTime gachaEventEndTime;

    private Color gachaEventBackGroundColor = new(0, 0, 0);

    private Color gachaEventTextColor = new(0, 0, 0);

    private Sprite gachaEventIcon = null;
    #endregion

    public GachaEvent(string gachaEventName, DateTime gachaEventEndTime, Color gachaEventBackGroundColor, Color gachaEventTextColor, Sprite gachaEventIcon)
    {
        this.gachaEventName = gachaEventName;
        this.gachaEventEndTime = gachaEventEndTime;
        this.gachaEventBackGroundColor = gachaEventBackGroundColor;
        this.gachaEventTextColor = gachaEventTextColor;
        this.gachaEventIcon = gachaEventIcon;
    }

    #region GETTER METHODS
    public string GetGachaEventName()
    {
        return gachaEventName;
    }

    public DateTime GetGachaEventEndTime()
    {
        return gachaEventEndTime;
    }

    public Color GetGachaEventBackGroundColor()
    {
        return gachaEventBackGroundColor;
    }

    public Color GetGachaEventTextColor()
    {
        return gachaEventTextColor;
    }

    public Sprite GetEventIcon()
    {
        return gachaEventIcon;
    }
    #endregion
}
