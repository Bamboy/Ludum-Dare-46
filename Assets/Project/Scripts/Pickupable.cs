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

    public abstract void OnStartBeingHeld( Pickuper holder );
    protected abstract void OnHoldingUpdate( Pickuper holder );
    public abstract void OnDrop( Pickuper holder );
}
