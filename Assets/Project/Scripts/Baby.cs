using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Pickuper))]
public class Baby : Pickupable
{
    public static Baby Instance { get; private set; }

    private NavMeshAgent navigator;
    new private Collider collider;

    public Pickuper Pickup { get; private set; }

    public float pickupDistance = 5f;

    [ShowInInspector][ReadOnly]
    private Pickupable targetPickup;

    private void Awake()
    {
        if( Instance == null )
            Instance = this;

        collider = GetComponent<Collider>();
        navigator = GetComponent<NavMeshAgent>();
        Pickup = GetComponent<Pickuper>();
        Register();
    }
    private void Start()
    {
        navigator.Warp( transform.position );

        targetPickup = FindPickup();
    }

    void Update()
    {
        if( BeingHeld )
        {
            this.OnHoldingUpdate( Holder );
        }
        else
        {
            if( this.Pickup.IsHoldingSomething == false )
            {
                if( targetPickup == null || targetPickup.BeingHeld )
                {
                    //Find new target
                    targetPickup = FindPickup();
                }

                if( targetPickup != null )
                {
                    if( Vector3.Distance( transform.position, targetPickup.transform.position ) < pickupDistance )
                    {
                        if( targetPickup.CanBePickedUpBy( this.Pickup ) )
                            Pickup.StartHold( targetPickup );
                        else
                            targetPickup = FindPickup();

                        return;
                    }

                    //Move to object
                    
                    navigator.SetDestination( targetPickup.transform.position );
                }
            }
        }
    }

    Pickupable FindPickup()
    {
        List<Pickupable> piks = new List<Pickupable>( 
            from p in Pickupables.Objects
                where (p != this as Pickupable) && p.BeingHeld == false && p.CanBePickedUpBy( this.Pickup )
                    orderby Vector3.Distance(transform.position, p.transform.position) ascending
                        select p );
        if( piks.Count > 0 )
            return piks[0];
        else
            return null;
    }

    public Pickupable TakeItem()
    {
        Pickupable p = this.Pickup.holdingObject;
        this.Pickup.EndHold();
        return p;
    }

    #region Pickupable

    public override bool CanBePickedUpBy( Pickuper holder )
    {
        return true;
    }

    public override void OnStartBeingHeld( Pickuper holder )
    {
        navigator.enabled = false;
        targetPickup = null;
        base.OnStartBeingHeld( holder );
    }

    protected override void OnHoldingUpdate( Pickuper holder )
    {
        return;
    }

    public override void OnDrop( Pickuper holder )
    {
        transform.parent = null;
        transform.position = holder.dropObjPosition.position;
        
        navigator.enabled = true;
        navigator.Warp( holder.dropObjPosition.position );
        targetPickup = FindPickup();
        base.OnDrop(holder);
    }

    #endregion

    void OnDestroy()
    {
        UnRegister();
    }
}
