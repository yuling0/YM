using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;

namespace XNode.Examples.MathNodes {
    [System.Serializable]
    [NodeWidth(500)]
    public class MathNode : XNode.Node {
        // Adding [Input] or [Output] is all you need to do to register a field as a valid port on your node 
        [Input] public float a;
        [Input] public float b;
        // The value of an output node field is not used for anything, but could be used for caching output results
        [Output] public float result;

        [Output(dynamicPortList = true)]
        [ListDrawerSettings(ShowPaging = false,Expanded = false/*,CustomAddFunction ="Add", CustomRemoveElementFunction = "Remove"*/)]
        [OnCollectionChanged("OnDynamicPortListChange")]
        public List<OptionData> fs = new List<OptionData>();
        // Will be displayed as an editable field - just like the normal inspector
        public MathType mathType = MathType.Add;
        public enum MathType { Add, Subtract, Multiply, Divide }

        // GetValue should be overridden to return a value for any specified output port
        public override object GetValue(XNode.NodePort port) {

            // Get new a and b values from input connections. Fallback to field values if input is not connected
            float a = GetInputValue<float>("a", this.a);
            float b = GetInputValue<float>("b", this.b);

            // After you've gotten your input values, you can perform your calculations and return a value
            result = 0f;

            if (port.fieldName.StartsWith("fs"))
            {
                string[] s = port.fieldName.Split(' ');
                int idx = int.Parse(s[1]);
                return fs[idx].condition;
            }
            if (port.fieldName == "result")
                switch (mathType) {
                    case MathType.Add: default: result = a+b; break;
                    case MathType.Subtract: result = a - b; break;
                    case MathType.Multiply: result = a * b; break;
                    case MathType.Divide: result = a / b; break;
                }
            return result;
        }

        //private OptionData Add()
        //{
        //    int i = fs.Count;
        //    this.AddDynamicOutput(typeof(OptionData), ConnectionType.Multiple, TypeConstraint.None, "fs" + " " + i.ToString());
        //    return new OptionData();
        //}

        //private void Remove()
        //{
        //    int i = fs.Count-1;
        //    this.RemoveDynamicPort("fs" + " " + i.ToString());
        //    fs.RemoveAt(i);
        //}
    }
    [Serializable]
    public class OptionData
    {
        [LabelText("选项")]
        public string option = "";
        [LabelText("条件判断")]
        public bool condition;
        [LabelText("表达式")]
        public string context = "";
    }
}