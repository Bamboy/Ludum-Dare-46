using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    public bool lockUpFacing = true;

    void LateUpdate()
    {
        if( lockUpFacing )
        {
            Vector3 dir = -VectorExtras.Direction( transform.position, MoveCamera.Instance.transform.position );
            transform.rotation = Quaternion.LookRotation( new Vector3( dir.x, 0f, dir.z ) );
        }
        else
        {
            transform.rotation = Quaternion.LookRotation( -VectorExtras.Direction( transform.position, MoveCamera.Instance.transform.position ) );
        }
    }

}
