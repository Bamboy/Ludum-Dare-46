using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Pickupable : MonoBehaviour
{
    protected void Register() { Pickupables.Register( this ); }
    protected void UnRegister(){ Pickupables.UnRegister( this ); }

    public Pickuper Holder { get; private set; }
    public bool BeingHeld { get { return Holder != null; } }

    public virtual bool CanBePickedUpBy( Pickuper holder ) { return true; }

    public virtual void OnStartBeingHeld( Pickuper holder )
    {
        Holder = holder;
    }
    protected abstract void OnHoldingUpdate( Pickuper holder );
    public virtual void OnDrop( Pickuper holder )
    {
        Holder = null;
    }
}
