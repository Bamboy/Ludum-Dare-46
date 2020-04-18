using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PickupableRigidbody : Pickupable
{
    public Rigidbody body;
    new public Collider collider;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        Register();
    }

    private void Update()
    {
        if( BeingHeld )
            OnHoldingUpdate( Holder );
    }

    public override void OnStartBeingHeld( Pickuper holder )
    {
        collider.enabled = false;
        body.detectCollisions = false;
        body.isKinematic = true;
        base.OnStartBeingHeld( holder );
    }

    protected override void OnHoldingUpdate( Pickuper holder )
    {
        return;
    }

    public override void OnDrop( Pickuper holder )
    {
        collider.enabled = true;
        body.detectCollisions = true;
        body.isKinematic = false;
        transform.parent = null;
        base.OnDrop( holder );
    }

    private void OnDisable()
    {
        UnRegister();
    }
}
