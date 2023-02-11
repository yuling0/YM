using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NPBehave;
public class BehaveTest : MonoBehaviour
{
    Blackboard blackboard;
    Root tree;
    // Start is called before the first frame update
    void Start()
    {
        
        tree = 
            new Root
            (
                new Selector
                (
                    new BlackboardCondition
                    ("Test", Operator.IS_EQUAL ,true , Stops.IMMEDIATE_RESTART,
                        new Action(() => { print("Test¥•∑¢£°£°£°");blackboard.Set("Test", false); })
                    )
                    ,
                    new Action(() => { print("Action÷¥––");blackboard.Set("Test",true); })
                )
            );
        blackboard = tree.Blackboard;

#if UNITY_EDITOR
        Debugger debugger = (Debugger)this.gameObject.AddComponent(typeof(Debugger));
        debugger.BehaviorTree = tree;
#endif
        //tree.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
