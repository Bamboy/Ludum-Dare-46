using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Gui;

public class FillBar : MonoBehaviour
{
    public RectTransform fill;
    public RectTransform fillBG;


    public float fillAmount = 0.8f;
    private float defaultWidth;

    private LeanBox graphic;

    public Color color
    {
        get { return graphic.color; }
        set { graphic.color = value; }
    }

    private void Awake()
    {
        graphic = fill.GetComponent<LeanBox>();
        defaultWidth = fill.rect.width;
    }

    public void Set( float percent )
    {
        fillAmount = percent;

        float mirror = VectorExtras.MirrorValue( fillAmount, 0f, 1f );
        fill.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, defaultWidth - (mirror * fillBG.rect.width) );
    }


}
