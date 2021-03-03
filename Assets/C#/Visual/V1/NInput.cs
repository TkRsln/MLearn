using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NInput : Neuron {

    public NInput(GameObject obj):base(null,true,obj)
    {
        setText("Inputs");
    }

}
