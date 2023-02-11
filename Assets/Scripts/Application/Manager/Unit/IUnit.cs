using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    int ID { get; }
    string UnitName { get; }
    object UnitInstance { get; }

    void OnInit(int id,string unitName,object unitInstance,object userData = null);
    void OnShow(object userData = null);

    void OnHide(object userData = null);

    void OnRecycle(object userData = null);

    void OnUpdate();

    void OnFixedUpdate();
}
