using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteInEditMode]
public class MapArea : MonoBehaviour
{
    [SerializeField]
    private Vector2 areaXRange;

#if UNITY_EDITOR
    private void Update()
    {
        DrawUtility.DrawRectangle(this.transform,areaXRange,Color.green);
    }
#endif
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
