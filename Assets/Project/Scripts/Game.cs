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

            TimeLeft -= Time.deltaTime;
            GameInfoUI.Instance.SetTimer( TimeLeft );

            if( Baby.Instance.PickupGrabber.IsHoldingSomething && Baby.Instance.PickupGrabber.heldObject.ObjectType == "Bottle" )
                Baby.Instance.hunger += Baby.Instance.hungerRestoreRate * Time.deltaTime;
            else
                Baby.Instance.hunger -= Baby.Instance.hungerRate * Time.deltaTime;

            Baby.Instance.hunger = Mathf.Clamp( Baby.Instance.hunger, 0f, 100f );
            GameInfoUI.Instance.SetFood( Baby.Instance.hunger / 100f );

            if( TimeLeft <= 0f )
            {
                EndBabysit();
                yield break;
            }
            yield return null;
        }
    }

    void EndBabysit()
    {

    }
}
