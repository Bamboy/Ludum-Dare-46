using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[RequireComponent( typeof( Collider ) )]
public class Pickuper : MonoBehaviour
{
    [HideInInspector]
    new public Collider collider;
    protected void Awake()
    {
        collider = GetComponent<Collider>();
    }

    public Transform heldObjAnchor;
    public Transform heldObjPosition;
    public Transform dropObjPosition;

    [ReadOnly]
    public Pickupable heldObject = null;

    public bool IsHoldingSomething { get { return heldObject != null; } }

    public void StartHold( Pickupable obj )
    {
        if( IsHoldingSomething == false /*&& obj.CanBePickedUpBy( this )*/ )
        {
            heldObject = obj;
            obj.transform.position = heldObjPosition.position;
            obj.transform.parent = heldObjAnchor;
            heldObject.OnStartBeingHeld( this );
        }
    }

    public void EndHold()
    {
        if( IsHoldingSomething == true )
        {
            heldObject.OnDrop( this );
            heldObject.transform.parent = null;
 
            Vector3 direction = VectorExtras.Direction( heldObjAnchor.position, dropObjPosition.position );
            Vector3 backOffset = VectorExtras.OffsetPosInDirection( heldObjAnchor.position, -direction, 0.8f );
            RaycastHit[] datas;
            datas = Physics.BoxCastAll( backOffset, Vector3.one * 0.25f, direction,
                               heldObjAnchor.rotation, Vector3.Distance( backOffset, dropObjPosition.position ) );
            
            foreach( var data in datas )
            {
                if( data.collider == heldObject.GetComponent<Collider>() ||
                    data.collider.gameObject.layer == LayerMask.NameToLayer( "Player" ) || 
                    data.collider.gameObject.layer == LayerMask.NameToLayer( "Baby" ) )
                    continue;
                else
                {
                  //  Debug.Log( data.collider.gameObject.name );
                    heldObject.transform.rotation = heldObjAnchor.rotation;
                    heldObject.transform.position = VectorExtras.OffsetPosInDirection( data.point, -direction, 0.4f );
                   // Debug.Break();
                    heldObject = null;
                    Debug.DrawLine(backOffset, dropObjPosition.position, Color.red, 4f );
                    
                    return;
                }
            }

            heldObject.transform.position = dropObjPosition.position;
            heldObject = null;
        }
    }

}
