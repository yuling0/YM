using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName ="TestScriptableObject",menuName = "YM/TestScriptableObject")]
public class ScriptableObjectTest : ScriptableObject
{
    [SerializeField]
    private GameObject obj;
}
