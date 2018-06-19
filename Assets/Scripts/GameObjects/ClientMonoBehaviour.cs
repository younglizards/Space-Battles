using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ClientMonoBehaviour : MonoBehaviour
{
    private FakeClient client = null;
    protected FakeClient Client
    {
        get
        {
            if (client == null) { client = FindObjectOfType<FakeClient>(); }
            return client;
        }
    }
}
