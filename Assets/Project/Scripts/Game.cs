using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game : MonoBehaviour
{
    public const string Date = "4-17-2020";


    public float babysitTime = 500f;


    private void Awake()
    {
        Pickupables.Initalize();
    }

    public float TimeLeft { get; private set; }
    IEnumerator GameTimerRoutine()
    {
        TimeLeft = babysitTime;
        while( true )
        {
            TimeLeft -= Time.deltaTime;

            if( TimeLeft <= 0f )
            {
                EndBabysit();
                yield break;
            }
        }
    }

    void EndBabysit()
    {

    }
}
