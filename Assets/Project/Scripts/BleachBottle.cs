using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleachBottle : PickupableRigidbody
{
    public override string ObjectType { get { return "Bleach"; } }

    private MeshRenderer[] renderers;

    protected override void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>();
        base.Awake();
    }

    public void Show( bool state )
    {
        foreach( MeshRenderer item in renderers )
            item.enabled = state;
    }

    public override bool CanBePickedUpBy( Pickuper holder )
    {
        if( holder.GetComponent<Baby>() != null )
            return Baby.Instance.hunger >= 25f; //Dont drink bleach - Give player time to find milk
        else
            return true;
    }

    public override void OnStartBeingHeld( Pickuper holder )
    {
        base.OnStartBeingHeld( holder );
    }

    protected override void OnHoldingUpdate( Pickuper holder )
    {
        /*
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
        } */

    }

    public override void OnDrop( Pickuper holder )
    {
        base.OnDrop( holder );
    }
}
