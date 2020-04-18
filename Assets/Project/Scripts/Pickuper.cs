using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[RequireComponent( typeof( Collider ) )]
public class Pickuper : MonoBehaviour
{
    public Transform heldObjAnchor;
    public Transform heldObjPosition;
    public Transform dropObjPosition;
    [ReadOnly]
    public Pickupable holdingObject = null;



    public bool IsHoldingSomething { get { return holdingObject != null; } }

    public void StartHold( Pickupable obj )
    {
        if( IsHoldingSomething == false && obj.CanBePickedUpBy( this ) )
        {
            holdingObject = obj;
            obj.transform.position = heldObjPosition.position;
            obj.transform.parent = heldObjAnchor;
            holdingObject.OnStartBeingHeld( this );
        }
    }

    public void EndHold()
    {
        if( IsHoldingSomething == true )
        {
            holdingObject.OnDrop( this );
            holdingObject.transform.parent = null;
            holdingObject.transform.position = dropObjPosition.position;
            holdingObject = null;
        }
    }
}
