using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Cache 
{
    private static Dictionary<Collider2D, Bee> bees = new Dictionary<Collider2D, Bee>();
    private static Dictionary<GameObject, Bee> beeGO = new Dictionary<GameObject, Bee>();

    public static Bee GetBee(Collider2D collider)
    {
        if (!bees.ContainsKey(collider))
        {
            bees.Add(collider, collider.GetComponent<Bee>());
        }

        return bees[collider];
    }
    public static Bee GetBeeGO(GameObject go)
    {
        if (!beeGO.ContainsKey(go))
        {
            beeGO.Add(go, go.GetComponent<Bee>());
        }

        return beeGO[go];
    }
}
