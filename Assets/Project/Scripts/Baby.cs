using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Pickuper))]
public class Baby : Pickupable
{
    public const string ANIM_STATE_NAME = "actionState";
    public const float ANIM_THROW_LENGTH = 0.5f;

    public static Baby Instance { get; private set; }

    private NavMeshAgent navigator;
    new private Collider collider;
    [Required]
    public Animator animator;


    [Space]
    public float pickupDistance = 6f;
    public float throwPow = 16.5f;

    [Space]
    public float findNewTargetDelay = 0.8f;
    public float getBoredDelay = 2f;

    [Space]
    [ReadOnly]
    public float hunger = 100f;
    public float hungerRate = 1.2f;
    public float hungerRestoreRate = 2.5f;


    private void Awake()
    {
        if( Instance == null )
            Instance = this;

        collider = GetComponent<Collider>();
        navigator = GetComponent<NavMeshAgent>();
        PickupGrabber = GetComponent<Pickuper>();
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
            this.OnHoldingUpdate( HeldBy );
        }
        else
        {
            if( this.PickupGrabber.IsHoldingSomething == false )
            {
                if( targetPickup == null || targetPickup.BeingHeld )
                {
                    //Find new target with no delay
                    targetPickup = FindPickup();
                    //return;
                } /*
                if( Vector3.Distance( navigator.destination, targetPickup.transform.position ) > pickupDistance )
                {
                    //Find new target within grab distance
                    targetPickup = FindPickup();
                    return;
                } */

                if( targetPickup != null && targetPickup != _lastPickup )
                {
                    if( Vector3.Distance( transform.position, targetPickup.transform.position ) < pickupDistance )
                    {
                        if( targetPickup.CanBePickedUpBy( this.PickupGrabber ) )
                        {
                            AnimationActionState = 0; //sit idle
                            PickupGrabber.StartHold( targetPickup );
                            StartCoroutine( GetBoredDelay() );
                        }
                        else
                        {
                            targetPickup = FindPickup();
                        }

                        return;
                    }

                    //Move to object
                    
                    AnimationActionState = 1; //walk
                    navigator.SetDestination( targetPickup.transform.position );
                }
            }
        }
    }

    public void StopMoving()
    {
        navigator.SetDestination( transform.position );
        AnimationActionState = 0; //sit idle
    }



    IEnumerator FindNewTargetDelay( float delay = 0.8f )
    {
        yield return null;

        AnimationActionState = 0; //sit
        float timer = delay * Random.Range( 0.3f, 1f );
        while( true )
        {
            timer -= Time.deltaTime;
            if( timer <= 0f )
            {
                targetPickup = FindPickup();
                if( targetPickup != null )
                {
                    AnimationActionState = 1; //walk
                    yield break;
                }
                else
                {
                    AnimationActionState = 0; //sit
                    timer = delay * Random.Range( 0.3f, 1f );
                }
            }
            yield return null;
        }
    }

    IEnumerator GetBoredDelay( float delay = 3f )
    {
        yield return null;

        AnimationActionState = 0; //sit
        float timer = delay * Random.Range( 0.3f, 1f );
        while( PickupGrabber.IsHoldingSomething )
        {
            timer -= Time.deltaTime;
            if( timer <= 0f )
            {
                StopAllCoroutines();
               // Debug.Break();
                AnimationActionState = 3; //trigger throw
                //Yeet();
                yield break;
            }
            yield return null;
        }
        StartCoroutine( FindNewTargetDelay() );
    }


    /* 
     * Animation Action states
     * 
     * 0 - bored, idle
     * 
     * 1 - moving (to object)
     * 2 - being held
     * 
     * 3 - throw
     * 4 - drinking
     */




    [ShowInInspector][ReadOnly]
    public int AnimationActionState
    {
        get { return animator.GetInteger( ANIM_STATE_NAME ); }
        set { animator.SetInteger( ANIM_STATE_NAME, value ); }
    }

    #region pickup logic

    [ShowInInspector][ReadOnly]
    private Pickupable targetPickup;
    private Pickupable _lastPickup;

    public Pickuper PickupGrabber { get; private set; }

    private Pickupable FindPickup()
    {
        List<Pickupable> piks = new List<Pickupable>( 
            from p in Pickupables.Objects
                where p != _lastPickup && IsValidPickupTarget( p ) 
                    orderby Vector3.Distance(transform.position, p.transform.position) ascending
                        select p );
        if( piks.Count > 0 )
            return piks[0];
        else
            return null;
    }

    private bool IsValidPickupTarget( Pickupable obj )
    {
        if( obj == this as Pickupable || obj.BeingHeld )
            return false;

        //if( Vector3.Distance( navigator.destination, obj.transform.position ) > pickupDistance )
        //    return false;

        return obj.CanBePickedUpBy( this.PickupGrabber );
    }
    public Pickupable TakeItem()
    {
        Pickupable p = this.PickupGrabber.heldObject;
        DropItem();
       // this.PickupGrabber.EndHold();
        return p;
    }

    public void DropItem()
    {
        if( this.PickupGrabber.IsHoldingSomething )
        {
            _lastPickup = this.PickupGrabber.heldObject;
            this.PickupGrabber.EndHold();
        }
    }

    public void Yeet()
    {
        if( this.PickupGrabber.heldObject is PickupableRigidbody )
        {
            Rigidbody body = (this.PickupGrabber.heldObject as PickupableRigidbody).body;
            body.isKinematic = false;

            //Vector3 dir = Random.insideUnitSphere.normalized;
            //dir = new Vector3( dir.x, this.PickupGrabber.dropObjPosition.forward.y, dir.z ).normalized;

            this.PickupGrabber.dropObjPosition.parent.localRotation = Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f);
            Vector3 force = VectorExtras.DirectionalCone( this.PickupGrabber.dropObjPosition.position, this.PickupGrabber.dropObjPosition.forward, 7.5f ) * throwPow * Random.Range(0.6f, 1f);
            body.AddForce( force, ForceMode.VelocityChange ); //Throw
            DropItem();
            
            this.PickupGrabber.dropObjPosition.parent.localRotation = Quaternion.identity;

            AnimationActionState = 0; //sit idle


            StopAllCoroutines();
            StartCoroutine( FindNewTargetDelay(ANIM_THROW_LENGTH) );
        }

        if( this.PickupGrabber.heldObject is MilkBottle )
        {
            (this.PickupGrabber.heldObject as MilkBottle).Show( true );
        }
    }

    #endregion

    #region Base Pickupable
    public override string ObjectType { get { return "Baby"; } }

    public override bool CanBePickedUpBy( Pickuper holder )
    {
        return true;
    }

    public override void OnStartBeingHeld( Pickuper holder )
    {
        StopAllCoroutines();
        navigator.enabled = false;
        targetPickup = null;

        AnimationActionState = 2; //being held sprite

        base.OnStartBeingHeld( holder );
    }

    protected override void OnHoldingUpdate( Pickuper holder )
    {
        transform.localScale = Vector3.one;
    }

    public override void OnDrop( Pickuper holder )
    {
        transform.parent = null;
        transform.position = holder.dropObjPosition.position;
        transform.localScale = Vector3.one;

        navigator.enabled = true;
        navigator.Warp( holder.dropObjPosition.position );
        _lastPickup = null;

        StopAllCoroutines();
        targetPickup = FindPickup();

        if( targetPickup == null )
            AnimationActionState = 0; // sit idle
        else
            AnimationActionState = 1; // walk

        base.OnDrop(holder);
    }

    #endregion

    void OnDestroy()
    {
        UnRegister();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere( transform.position, pickupDistance );
    }
}
