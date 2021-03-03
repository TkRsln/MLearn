using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZV1;

public interface IKarar  {

    void OnScore(Memory m);
    void OnNewMemory(Memory m);
    void OnSolution(Memory m);
}
