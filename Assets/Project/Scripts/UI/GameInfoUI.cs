using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;

public class GameInfoUI : MonoBehaviour
{
    public static GameInfoUI Instance { get; private set; }


    public TextMeshProUGUI timer;
    public FillBar feedBar;

    [Space]
    public Color blinkOn;
    public Color blinkOff;
    public float blinkNeedThreshhold = 0.33f;


    [Button]
    public void SetTimer( float seconds )
    {
        int secs = Mathf.CeilToInt( seconds % 60f );
        int mins = Mathf.FloorToInt( seconds / 60f );
        if( secs == 60 )
        {
            secs = 0;
            mins++;
        }
        mins = Mathf.Clamp( mins, 0, 99 );
        secs = Mathf.Clamp( secs, 0, 60 );

        timer.text = string.Format( "{0:00}:{1:00}", mins, secs );
    }

    public void SetFood( float amount )
    {
        feedBar.Set( amount );
        if( amount < blinkNeedThreshhold )
            StartCoroutine( BlinkNeedBar( feedBar ) );
    }

    IEnumerator BlinkNeedBar( FillBar bar )
    {
        while( bar.fillAmount < blinkNeedThreshhold )
        {
            if( Mathf.FloorToInt( Time.time ) % 2 == 0 )
                bar.color = blinkOff;
            else
                bar.color = blinkOn;

            yield return null;
        }
        bar.color = blinkOff;
    }

    private void Awake()
    {
        if( Instance == null )
            Instance = this;
    }
}
