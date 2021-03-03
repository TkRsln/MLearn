using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObject  {
    void OnEnter(string tag,GameObject target);
    void OnExit(string tag,GameObject target);
}
