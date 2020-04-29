using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Game : MonoBehaviour
{
    public const string Date = "4-17-2020";

    public static Game Instance { get; private set; }


    public float babysitTime = 500f;



    private void Awake()
    {
        if( Instance == null )
            Instance = this;
        Pickupables.Initalize();
    }

    private void Start()
    {
        GameInfoUI.Instance.MenuShown = true;
        StartCoroutine( GameTimerRoutine() );
    }


    public float TimeLeft { get; private set; }
    IEnumerator GameTimerRoutine()
    {
        TimeLeft = babysitTime;
        Baby.Instance.hunger = 100f;

        while( true )
        {
            if( Time.timeScale <= 0f )
                yield return null;

            
            



            //Hunger
            float hungerChange = Baby.Instance.hungerRate;
            if( Baby.Instance.PickupGrabber.IsHoldingSomething )
            {
                if( Baby.Instance.PickupGrabber.heldObject.ObjectType == "Milk" )
                    hungerChange = Baby.Instance.hungerRestoreRate;
                else if( Baby.Instance.PickupGrabber.heldObject.ObjectType == "Bleach" )
                    hungerChange = Baby.Instance.hungerPoisonRate;
            }

            Baby.Instance.hunger = Mathf.Clamp( Baby.Instance.hunger + (hungerChange * Time.deltaTime), 0f, 100f );
            GameInfoUI.Instance.SetFood( Baby.Instance.hunger / 100f );

            TimeLeft -= Time.deltaTime;
            if( TimeLeft <= 0f )
            {
                EndBabysit( true );
                yield break;
            }
            else
            {
                if( Baby.Instance.hunger <= 0f )
                {
                    EndBabysit( false );
                    yield break;
                }
                else
                    GameInfoUI.Instance.SetTimer( TimeLeft );
            }
            yield return null;
        }
    }

    void EndBabysit( bool victory )
    {
        GameInfoUI.Instance.SetTimer( victory ? "W.I.N" : "R.I.P" );
        GameInfoUI.Instance.timer.color = victory ? Color.blue : Color.red;
    }

    /*
    public enum GameState
    {
        Win,
        Lose,
        Playing
    } */
}
