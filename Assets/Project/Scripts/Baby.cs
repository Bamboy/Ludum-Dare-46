using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Pickuper))]
public class Baby : Pickupable
{
    public static Baby Instance { get; private set; }

    private NavMeshAgent navigator;
    new private Collider collider;

    public Pickuper Pickup { get; private set; }

    private void Awake()
    {
        if( Instance == null )
            Instance = this;

        collider = GetComponent<Collider>();
        navigator = GetComponent<NavMeshAgent>();
        Register();
    }


    void Update()
    {
        if( BeingHeld )
        {
            this.OnHoldingUpdate( Holder );
        }
    }

    #region Pickupable

    public override bool CanBePickedUpBy( Pickuper holder )
    {
        return true;
    }

    public override void OnStartBeingHeld( Pickuper holder )
    {
        return;
    }

    protected override void OnHoldingUpdate( Pickuper holder )
    {
        return;
    }

    public override void OnDrop( Pickuper holder )
    {
        return;
    }

    #endregion

    void OnDestroy()
    {
        UnRegister();
    }
}
