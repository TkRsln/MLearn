using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZV1;

public class Output:Neuron  {

    public Output(GameObject obj,Action.Kind kind): base(null, false,obj)
    {
        this.kind = kind;
        setText(kind.ToString());
    }
    public Action.Kind kind; 


    
}
