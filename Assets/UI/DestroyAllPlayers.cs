using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllPlayers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (var p in FindObjectsOfType<GenericPlayer>())
        {
            Destroy(p.gameObject);
        }
    }

}
