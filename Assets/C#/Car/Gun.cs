using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car { 
    public class Gun : MonoBehaviour {
        public static Gun active;
        public  GameObject target;
        public GameObject gun = null;
        public ParticleSystem gun_partic = null;
        public bool canShot = true;
        public float distance = 10f;
        public float cool_down = 3.0f;
        public float hitForce = 1000f;


    
        private float wait_until = 0f;

        private void Start()
        {
            active = this;
        }

        private void Update()
        {
            if (gun == null || target == null ) return;
            if (Vector2.Distance(this.transform.position, target.transform.position) < distance)
            {
                gun.transform.LookAt(target.transform);
                if (!canShot) return;
                if (wait_until < Time.time)
                {
                    if (gun_partic != null) gun_partic.Play();
                    wait_until = Time.time + cool_down;
                    OnHit(target);
                }

            }
            else
            {
                target = null;
                //gun.transform.rotation = rotate(new Vector3(0, -90, 0), gun.transform.rotation);
            }
        
        
        }
        public float getRotation()
        {
            return gun.transform.rotation.eulerAngles.x;
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, distance);
        }
            private void OnHit(GameObject target)
        {
            Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
            if (rb != null) rb.AddForce((target.transform.position - gun.transform.position).normalized * hitForce);
        }

        private Quaternion rotate(Vector3 to,Quaternion qua)
        {
            qua.eulerAngles = to;
            return qua;
        }
    }
}
