using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZV1 { 
    public class Action : MonoBehaviour {

        public static Action active = null;

        private void Start()
        {
            active = this;
        }

        public void apply(Kind act)
        {

            if (act == Kind.Zıpla)
            {
                Car.Motor.active.Jump();
            }
            if (act == Kind.Ilerle)
            {
                Car.Motor.active.wheel_active[0] = true;
                Car.Motor.active.wheel_active[1] = true;
                Car.Motor.active.speed = 150f;
            }
            if (act == Kind.Ateş)
            {
                Car.Gun.active.canShot = true;
                Car.Gun.active.target = ZV1.Karar.active.last_go;
            }

        }
        public  enum Kind : int
        {
            Ilerle = 0, Zıpla = 1, Ateş = 2, Hızlan = 3,

        }

    }
}
