using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkBottle : PickupableRigidbody
{
    public override string ObjectType { get { return "Bottle"; } }

    private MeshRenderer[] renderers;

    protected override void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        base.Awake();
    }

    public void Show( bool state )
    {
        Debug.Log( state );
        foreach( MeshRenderer item in renderers )
            item.enabled = state;
    }

    public override bool CanBePickedUpBy( Pickuper holder )
    {
        if( holder.GetComponent<Baby>() != null )
            return Baby.Instance.hunger <= 0.25f;
        else
            return true;
    }

    public override void OnStartBeingHeld( Pickuper holder )
    {
        if( Baby.Instance.PickupGrabber == holder )
            Show( false );
        else
            Show( true );

        base.OnStartBeingHeld( holder );
    }

    protected override void OnHoldingUpdate( Pickuper holder )
    {
        
        if( Baby.Instance.PickupGrabber == holder )
        {
            if( Baby.Instance.hunger >= 100f )
            {
                Baby.Instance.hunger = 100f;

                Baby.Instance.AnimationActionState = 3; //Start throw
                Show( true );
            }
            else
                Baby.Instance.AnimationActionState = 4; // drink
        }

    }

    public override void OnDrop( Pickuper holder )
    {
        //if( Baby.Instance.PickupGrabber == holder )
            Show( true );

        base.OnDrop( holder );
    }
}
