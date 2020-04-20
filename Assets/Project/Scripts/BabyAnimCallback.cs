using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyAnimCallback : MonoBehaviour
{
    private Baby owner;
    new private SpriteRenderer renderer;
    private void Awake()
    {
        owner = GetComponentInParent<Baby>();
        renderer = GetComponent<SpriteRenderer>();
    }

    public void YeetObject()
    {
        owner.Yeet();
       // Debug.Break();
    }

    public void LateUpdate()
    {
        float dot = Vector3.Dot( owner.transform.right, Player.Instance.Controller.orientation.forward );

        renderer.flipX = Mathf.Sign( dot ) < 0f;
    }
}
