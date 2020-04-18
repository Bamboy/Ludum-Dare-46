using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( Pickuper ), typeof( PlayerMovement ))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public PlayerMovement Controller { get; private set; }
    public Pickuper Pickup { get; private set; }

    public float pickupDistance = 8f;

    private void Awake()
    {
        if( Instance == null )
            Instance = this;

        Controller = GetComponent<PlayerMovement>();
        Pickup = GetComponent<Pickuper>();
    }

    private void Update()
    {
        if( Input.GetButtonDown( "Fire1" ) )
        {
            if( this.Pickup.IsHoldingSomething )
            {
                if( this.Pickup.holdingObject is PickupableRigidbody )
                {
                    Rigidbody body = (this.Pickup.holdingObject as PickupableRigidbody).body;
                    body.isKinematic = false;
                    body.AddForce( Controller.orientation.forward * 16.5f, ForceMode.VelocityChange ); //Throw
                }
                this.Pickup.EndHold();
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
                                if( bab.Pickup.IsHoldingSomething )
                                {
                                    this.Pickup.StartHold( bab.TakeItem() );
                                    return;
                                }
                            }

                            if( obj.CanBePickedUpBy( this.Pickup ) )
                            {
                                this.Pickup.StartHold( obj );
                            }
                        }
                    }
                }
            }
        }
    }

}
