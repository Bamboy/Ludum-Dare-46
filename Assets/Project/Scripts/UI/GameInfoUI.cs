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
    public FillBar feedBar;

    public GameObject menu;
    public Image backgroundMenuShade;
    public GameObject credit;
    public GameObject gameplay;

    

    [Space]
    public Color blinkOn;
    public Color blinkOff;
    public float blinkNeedThreshhold = 0.33f;


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

    private void Awake()
    {
        if( Instance == null )
            Instance = this;
    }

    private void Update()
    {
        if( Input.GetKeyDown( KeyCode.Escape ) )
            MenuShown = !MenuShown;

    }
}
