using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaOption
{
    private string gachaOptionName = "¿œπ› ∞°√≠";
    private bool gachaLimited;

    private Color gachaBackGround = new(0, 0, 0);
    private Color gachaGradient = new(0, 0, 0);

    private Color gachaTextColor = new(0, 0, 0);

    private Sprite gachaIcon = null;

    public GachaOption(string gachaOptionName, bool gachaLimited, Color gachaBackGround, Color gachaGradient, Color gachaTextColor, Sprite gachaIcon)
    {
        this.gachaOptionName = gachaOptionName;
        this.gachaLimited = gachaLimited;
        this.gachaBackGround = gachaBackGround;
        this.gachaGradient = gachaGradient;
        this.gachaTextColor = gachaTextColor;
        this.gachaIcon = gachaIcon;
    }

    public string GetGachaOption()
    {
        return gachaOptionName;
    }

    public bool GetGachaLimited()
    {
        return gachaLimited;
    }

    public Color GetGachaBackGround()
    {
        return gachaBackGround;
    }

    public Color GetGachaGradient() {
        return gachaGradient;
    }

    public Color GetGachaTextColor()
    {
        return gachaTextColor;
    }

    public Sprite GetGachaIcon()
    {
        return gachaIcon;
    }
}