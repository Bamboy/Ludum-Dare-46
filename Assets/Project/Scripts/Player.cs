using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( Pickuper ), typeof( PlayerMovement ))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public PlayerMovement Controller { get; private set; }
    public Pickuper PickupGrabber { get; private set; }

    public float pickupDistance = 8f;
    public float throwPow = 16.5f;

    private void Awake()
    {
        if( Instance == null )
            Instance = this;

        Controller = GetComponent<PlayerMovement>();
        PickupGrabber = GetComponent<Pickuper>();
    }

    private void Update()
    {
        if( Input.GetButtonDown( "Fire1" ) )
        {
            if( this.PickupGrabber.IsHoldingSomething )
            {
                if( this.PickupGrabber.heldObject is MilkBottle || this.PickupGrabber.heldObject is BleachBottle )
                {
                    if( Baby.Instance.PickupGrabber.IsHoldingSomething == false )
                    {
                        if( Vector3.Distance( transform.position, Baby.Instance.transform.position ) < pickupDistance )
                        {
                            Baby.Instance.PickupGrabber.StartHold( this.TakeItem() );
                            Baby.Instance.StopMoving();
                            return;
                        }
                    }
                }

                if( this.PickupGrabber.heldObject is PickupableRigidbody )
                {
                    Rigidbody body = (this.PickupGrabber.heldObject as PickupableRigidbody).body;
                    body.isKinematic = false;
                    body.AddForce( Controller.orientation.forward * throwPow, ForceMode.VelocityChange ); //Throw
                }
                this.PickupGrabber.EndHold();
            }
            else
            {
                RaycastHit data;
                Ray ray = MoveCamera.Instance.camera.ScreenPointToRay( Input.mousePosition );
                if( Physics.Raycast( ray, out data ) )
                {
                    if( data.distance < pickupDistance )
                    {
                        Pickupable obj = data.collider.GetComponent<Pickupable>();
                        if( obj != null )
                        {
                            if( obj is Baby )
                            {
                                Baby bab = obj as Baby;
                                if( bab.PickupGrabber.IsHoldingSomething && bab.PickupGrabber.heldObject is MilkBottle == false )
                                {
                                    this.PickupGrabber.StartHold( bab.TakeItem() );
                                }
                                return;
                            }
                            else if( obj is MilkBottle )
                            {
                                if( obj.BeingHeld && obj.HeldBy.GetComponent<Baby>() != null )
                                    return; //Prevent taking bottle from baby
                            }
                            else
                            {
                                if( obj.BeingHeld && obj.HeldBy.GetComponent<Baby>() != null )
                                {
                                    this.PickupGrabber.StartHold( Baby.Instance.TakeItem() );
                                    return;
                                }
                            }

                            if( obj.CanBePickedUpBy( this.PickupGrabber ) )
                            {
                                this.PickupGrabber.StartHold( obj );
                            }
                        }
                    }
                }
            }
        }
    }

    public Pickupable TakeItem()
    {
        Pickupable p = this.PickupGrabber.heldObject;
        this.PickupGrabber.EndHold();
        return p;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere( transform.position, pickupDistance );
    }
}
