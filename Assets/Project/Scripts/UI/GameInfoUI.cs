using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Sirenix.OdinInspector;

public class GameInfoUI : MonoBehaviour
{
    public static GameInfoUI Instance { get; private set; }

    public TextMeshProUGUI timer;
    public TextMeshProUGUI timerUnlit;
    public FillBar feedBar;

    public GameObject menu;
    public Image backgroundMenuShade;
    public GameObject credit;
    public GameObject gameplay;

    [Space]
    public Color blinkOn;
    public Color blinkOff;
    public float blinkNeedThreshhold = 0.33f;

    private void Awake()
    {
        if( Instance == null )
            Instance = this;
    }
    private void Start()
    {
        StartCoroutine( BlinkNeedBar( feedBar ) );
    }

    private bool _menuShown = true;
    public bool MenuShown
    {
        get { return _menuShown; }
        set
        {
            _menuShown = value;
            Time.timeScale = value ? 0f : 1f;

            backgroundMenuShade.enabled = value;
            menu.SetActive( value );
            credit.SetActive( value );
            gameplay.SetActive( !value );

            Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = value;
        }
    }
 
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
        timerUnlit.text = "00:00";
    }
    public void SetTimer( string text )
    {
        timer.text = text;
        timerUnlit.text = text;
    }

    public void SetFood( float amount )
    {
        feedBar.Set( amount );
        if( amount < blinkNeedThreshhold )
            StartCoroutine( BlinkNeedBar( feedBar ) );
    }

    IEnumerator BlinkNeedBar( FillBar bar )
    {
        while( true )
        {
            float speed = GetBlinkSpeed(bar);
            if( speed > 0 )
            {
                if( Mathf.FloorToInt( Time.unscaledTime * speed ) % 2 == 0 )
                    bar.color = blinkOff;
                else
                    bar.color = blinkOn;
            }
            else
                bar.color = blinkOff;

            yield return null;
        }
    }

    private float GetBlinkSpeed( FillBar bar )
    {
        if( Baby.Instance.PickupGrabber.IsHoldingSomething )
        {
            if( Baby.Instance.PickupGrabber.heldObject.ObjectType == "Milk" )
                return 0f;
            if( Baby.Instance.PickupGrabber.heldObject.ObjectType == "Bleach" )
                return Mathf.PI * 1.25f;
        }

        if( bar.fillAmount < blinkNeedThreshhold )
            return 1.25f;

        return 0f;
    }

    public void Btn_MouseSens( float value )
    {
        PlayerMovement.sensitivity = Mathf.Lerp(32f, 120f, value);
    }

    public void Btn_Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void Btn_Quit()
    {
        Application.Quit();
    }


    private void Update()
    {
        if( Input.GetKeyDown( KeyCode.Escape ) )
            MenuShown = !MenuShown;

    }
}
