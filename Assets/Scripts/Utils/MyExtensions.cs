using System;
using System.Collections.Generic;
using UnityEngine;

public static class MyExtensions
{
    private static System.Random rng = new System.Random();

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static T FindInParents<T>(this GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }

    public static int GetPositionInParent (this Transform tf)
    {
        Transform parent = tf.parent;
        for (int i = 0; i < parent.childCount; i ++)
        {
            if (tf.parent.GetChild(i) == tf)
            {
                return i;
            }
        }
        throw new Exception("Impossible situation");
    }

    public static void DestroyAllChildren(this Transform t)
    {
        for (int i = t.childCount - 1; i >= 0; i --)
        {
            UnityEngine.Object.Destroy(t.GetChild(i).gameObject);
        }
    }
}
