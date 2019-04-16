using System.Collections;
using UnityEngine;
using UnityEditor;
 
[ExecuteInEditMode]
public class StaticCoroutine : MonoBehaviour {
    private static StaticCoroutine m_instance;
   /* private void OnDestroy()
    { m_instance.StopAllCoroutines(); }*/
/*    private void OnApplicationQuit()
    { m_instance.StopAllCoroutines(); }*/
    private static StaticCoroutine Build() {
        if (m_instance != null)
        { return m_instance; }
 
        m_instance = (StaticCoroutine)FindObjectOfType(typeof(StaticCoroutine));
 
        if (m_instance != null)
        { return m_instance; }
 
        GameObject instanceObject = new GameObject("StaticCoroutine");
        instanceObject.AddComponent<StaticCoroutine>();
        m_instance = instanceObject.GetComponent<StaticCoroutine>();
 
        if (m_instance != null)
        { return m_instance; }
 
        Debug.LogError("Build did not generate a replacement instance. Method Failed!");
        return null;
    }
    public static void Start(string methodName)
    { Build().StartCoroutine(methodName); }
    public static void Start(string methodName, object value)
    { Build().StartCoroutine(methodName, value); }
    public static void Start(IEnumerator routine)
    { Build().StartCoroutine(routine); }
}
 