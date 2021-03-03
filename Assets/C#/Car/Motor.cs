using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car { 
    public class Motor : MonoBehaviour, IGround
    {
        public static Motor active;

        [Space]
        [Header("Main")]
        [Range(-250f,250f)]
        public float speed = 10f;
        public float jump_force = 100f;
        public bool jump_btn = false;
        //public bool canJump = false;

        [Space]
        [Header("Wheel")]
        public WheelJoint2D[] wheels;
        public bool[] wheel_active;
        private JointMotor2D mtr;

        [Space]
        public ParticleSystem[] particle;


        [Space]
        [Header("Grounds")]
        public bool ground_on = true;
        public bool ground_arka = true;

        public void Jump()
        {
            if (!IsOnGround()) return;
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jump_force * 1000f));
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            foreach(ParticleSystem o in particle)
            {
                o.Play();
            }
        }

        private void Start()
        {
            active = this;
            DGround.listener.Add(this);
            wheel_active = new bool[wheels.Length];
            //if (wheels.Length >= 1) wheel_active[0] = true;
        }

        // Update is called once per frame
        void Update () {
           // if (IsOnGround()) canJump = true;
            mtr.motorSpeed = speed*8;
            mtr.maxMotorTorque = 3000f*5f;
            for (int i = 0; i < wheel_active.Length; i++) {
                if (wheels.Length > i)
                {
                    wheels[i].useMotor = wheel_active[i];
                    if (wheel_active[i]) wheels[i].motor = createMotor(speed*8, 3000f * 5f);
                    else wheels[i].motor = createMotor(0, 0);
                }
            }
            if (jump_btn)
            {
                jump_btn = false;
                Jump();
            }
	    }

        public void startAll(float speed)
        {
            this.speed = speed;
            //for(int i=0;i<wheel_active.Length;i++) wheel_active[i] = true;
            wheel_active[1] = true;
        }
        public void stopAll()
        {
            speed = 0;
            //for (int i = 0; i < wheel_active.Length; i++) wheel_active[i] = false;
        }

        private JointMotor2D createMotor(float speed,float torq)
        {
            JointMotor2D jm = new JointMotor2D();
            jm.maxMotorTorque = torq;
            jm.motorSpeed = speed;
            return jm;
        }

        public bool IsOnGround()
        {
            return ground_on || ground_arka;
        }

        public void OnGround(int i, bool isIn) // GROUND
        {
            if (isIn) GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            if (i == 1) ground_on = isIn;
            else if (i == 2) ground_arka = isIn;
        }
    }
}
