using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DropShadow : MonoBehaviour
{
    public Transform shadow;
    public float size = 1f;

    private Quaternion defaultRot;
    private void Awake()
    {
        defaultRot = Quaternion.Euler(90f, 0f, 0f);
    }
    void LateUpdate()
    {
        if( shadow == null )
            return;

        shadow.transform.localScale = new Vector3( size, size, 1f );

        RaycastHit data;
        if( Physics.Raycast( transform.position, Vector3.down, out data, 4f, LayerMask.GetMask( new string[2]{ "Ground", "Default" } ) ) )//LayerMask.NameToLayer( "Everything" ) | LayerMask.NameToLayer( "Ground" ) ) )
        {
            shadow.transform.position = data.point + (Vector3.up * 0.01f);
        }
        else
            shadow.transform.localPosition = new Vector3( 0f, 3000f, 0f );

        shadow.rotation = defaultRot;
        /*
        Vector3 direction = VectorExtras.Direction( transform.position, dropObjPosition.position );
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
        }*/
    }

}
