using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using log4net;

public class Test : MonoBehaviour
{
    private static readonly ILog Log = LogManager.GetLogger(type:typeof(Test));
    void Start()
    {
        Log.Debug("sdsdfsdfsdfsdfsdf");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
