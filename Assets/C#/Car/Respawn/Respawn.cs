using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car { 
    public class Respawn : MonoBehaviour, IGround
    {
        [Range(0,10)]
        public float health = 10f;
        private static List<IRespawn> listener = new List<IRespawn>();
        public Transform point = null;
        public static Respawn active;

        [Space]
        [Header("Grounds")]
        public bool ground_on = true;
        public bool ground_arka = true;

        [Space]
        [Header("  Settings")]
        [Header("Stuck")]
        public float time_move = 0f;
        public float cd_move = 5f;
        public bool canMove = true;
        private Vector3 pos_move = Vector3.zero;
        [Header("Move")]
        public float time_fall = 0f;
        public float cd_fall = 5f;
        public bool canCount = false;

        private void Start()
        {
            DGround.listener.Add(this);
            active = this;
        }

        private void Update()
        {

            if (this.transform.position.y < -10) Damage(2,ERespawn.OnFall);
#region OnFall
            if (!IsOnGround())
            {
                if (!canCount) { time_fall = Time.time + cd_fall; canCount = true; }
                else if (canCount && (Time.time > time_fall))
                {
                    Damage(20,ERespawn.OnMove);
                }
            }
            else canCount = false;
#endregion
#region OnStuck
            if (time_move < Time.time)
            {
                time_move = Time.time + cd_move;
                if (Vector3.Distance(transform.position, pos_move) <1f&&Mathf.Abs(Motor.active.speed)>12)
                {
                    if (canMove) canMove = false;
                    else Damage(20, ERespawn.OnStuck);
                }
                else canMove = true;
                pos_move = transform.position;
            }
#endregion


        }
        public void Damage(float damage,ERespawn reason)
        {
            health -= damage;
            if (health <= 0)
            {
                health = 10;
                foreach (IRespawn res in listener) res.OnRespawn(reason);
                respawn();
                //Debug.Log(reason);
            }
        }
        private void respawn()
        {
            if (point == null) return;
            transform.position = point.position;
            Quaternion qua = transform.rotation;
            qua.eulerAngles = Vector3.zero;
            transform.rotation = qua;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GetComponent<Rigidbody2D>().rotation =0;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            canCount = false;
            canMove = true;
        }


        public bool IsOnGround()
        {
            return ground_on || ground_arka;
        }

        public void OnGround(int i, bool isIn) // GROUND
        {
            if (i == 1) ground_on = isIn;
            else if (i == 2) ground_arka = isIn;
        }
        public static void addListener(IRespawn list)
        {
            listener.Add(list);
        }
        public static void removeListener(IRespawn list)
        {
            listener.Remove(list);
        }
    }
}