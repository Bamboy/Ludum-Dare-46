using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Pickupables
{
    public static HashSet<Pickupable> Objects;

    public static void Register( Pickupable obj ) { Objects.Add( obj ); }

    public static void UnRegister( Pickupable obj ) { Objects.Remove( obj ); }

    public static void Initalize()
    {
        Objects = new HashSet<Pickupable>();
    }
}
