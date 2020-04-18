using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( Pickuper ), typeof( PlayerMovement ))]
public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public PlayerMovement Controller { get; private set; }
    public Pickuper Pickup { get; private set; }


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
                this.Pickup.EndHold();

                if( this.Pickup.holdingObject is PickupableRigidbody )
                {
                    Rigidbody body = (this.Pickup.holdingObject as PickupableRigidbody).body;
                    body.AddForce( Controller.orientation.rotation.eulerAngles.normalized * 400f );//VectorExtras.Direction( Controller.
                }
            }
            else
            {
                RaycastHit data;
                Ray ray = MoveCamera.Instance.camera.ScreenPointToRay( Input.mousePosition );
                if( Physics.Raycast( ray, out data ) )
                {
                    Pickupable obj = data.collider.GetComponent<Pickupable>();
                    if( obj != null )
                    {
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
