using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV1 { 
    [System.Serializable]
    public class Memory  {
        public Memory(string tag,ERespawn reason,Action.Kind solve,float score)
        {
            last_tag = tag;
            this.reason = reason;
            this.solve = solve;
            this.score = score;
        }

        [SerializeField]
        public string last_tag = null;
        [SerializeField]
        public ERespawn reason = ERespawn.OnDamage;
        [SerializeField]
        public Action.Kind solve = Action.Kind.Ilerle;
        [SerializeField]
        public float score = 1f;


    }
}
