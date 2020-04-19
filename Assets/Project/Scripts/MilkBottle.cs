using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkBottle : PickupableRigidbody
{
    public override string ObjectType { get { return "Bottle"; } }


    public override bool CanBePickedUpBy( Pickuper holder )
    {
        if( holder.GetComponent<Baby>() != null )
            return Baby.Instance.hunger <= 0.25f;
        else
            return true;
    }

    protected override void OnHoldingUpdate( Pickuper holder )
    {
        
        if( Baby.Instance.PickupGrabber == holder )
        {
            if( Baby.Instance.hunger >= 100f )
            {
                Baby.Instance.hunger = 100f;
                holder.EndHold();
            }
        }

    } 

}
