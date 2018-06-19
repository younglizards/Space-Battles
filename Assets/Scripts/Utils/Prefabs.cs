using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Prefabs
{
    static Dictionary<string, GameObject> prefabsByName = new Dictionary<string, GameObject>();

    public static GameObject Get(string prefabName)
    {
        GameObject prefab;
        if (!prefabsByName.TryGetValue(prefabName, out prefab))
        {
            prefab = Resources.Load<GameObject>(prefabName);
            if (prefab)
            {
                prefabsByName[prefabName] = prefab;
            }
            else
            {
                Debug.LogError("Undefined prefab with name " + prefabName);
            }
        }
        return prefab;
    }
}
