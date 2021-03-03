using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZV1;

public class NHidden : Neuron {
    public string tag = "";
    public NHidden(GameObject go,string tag,Action.Kind first) : base(null, true, go)
    {
        this.tag = tag;
        Output ou =Visual.active.findOutByKind(first);
        addAkson(ou.getGOPosition());
        setText(tag);
    }
    public bool isEqualTag(string tag)
    {
        return this.tag.Equals(tag)||this.tag==tag||this.tag.ToLower().Equals(tag.ToLower());
    }

}
