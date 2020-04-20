using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class PickupableRigidbody : Pickupable
{
    public const float RESPAWN_FLOOR = -10f;
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
        if( transform.position.y < RESPAWN_FLOOR )
        {
            Debug.LogWarning("Object fell out of the world!", this.gameObject);
            transform.position = Vector3.zero;
            body.velocity = Vector3.zero;
            body.AddForce( Random.insideUnitSphere.normalized * Random.Range( 1f, 10f ), ForceMode.VelocityChange );
        }

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
