using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    public string[] arrayNames;
    public List<int> listNames;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(arrayNames[3]);
        Debug.Log(listNames[1]);
    }
}
