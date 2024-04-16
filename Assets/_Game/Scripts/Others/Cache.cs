using System.Collections.Generic;
using UnityEngine;

public static class Cache 
{
    private static Dictionary<Collider2D, Bee> beeColider = new Dictionary<Collider2D, Bee>();
    private static Dictionary<GameObject, Bee> beeObject = new Dictionary<GameObject, Bee>();

    public static Bee GetBee(Collider2D collider)
    {
        if (!beeColider.ContainsKey(collider))
        {
            beeColider.Add(collider, collider.GetComponent<Bee>());
        }

        return beeColider[collider];
    }
    
    public static Bee GetBeeGameObject(GameObject gameObject)
    {
        if (!beeObject.ContainsKey(gameObject))
        {
            beeObject.Add(gameObject, gameObject.GetComponent<Bee>());
        }

        return beeObject[gameObject];
    }
}
