using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDebug : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        _ = InfluxTest.Main();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
