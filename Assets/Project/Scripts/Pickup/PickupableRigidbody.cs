using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PickupableRigidbody : Pickupable
{
    [HideInInspector]
    public Rigidbody body;
    [HideInInspector]
    new public Collider collider;

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        Register();
    }

    protected virtual void Update()
    {
        if( BeingHeld )
            OnHoldingUpdate( HeldBy );
    }

    public override string ObjectType { get { return "Rigidbody"; } }
    public override void OnStartBeingHeld( Pickuper holder )
    {
        Physics.IgnoreCollision( collider, holder.collider, true );
        body.isKinematic = true;
        base.OnStartBeingHeld( holder );
    }

    protected override void OnHoldingUpdate( Pickuper holder )
    {
        return;
    }

    public override void OnDrop( Pickuper holder )
    {
        Physics.IgnoreCollision( collider, holder.collider, false );
        body.isKinematic = false;
        transform.parent = null;
        base.OnDrop( holder );
    }

    protected virtual void OnDisable()
    {
        UnRegister();
    }
}
