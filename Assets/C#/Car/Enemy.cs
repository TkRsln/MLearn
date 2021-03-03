using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car { 
    [RequireComponent(typeof(Car.Gun))]
    public class Enemy : MonoBehaviour {

        public GameObject missle;
        public GameObject toSpawn;
        public float range = 30;
        public float waitFor = 1;
        [HideInInspector]
        public Gun enemyGun;
        public GameObject car;

        private float time_until = 0;

        void Start()
        {
            //car = GameObject.FindGameObjectWithTag("Player");
            enemyGun = GetComponent<Gun>();
            time_until = Time.time + 0.1f;
        }

        void Update()
        {
            if (Vector2.Distance(car.transform.position, transform.position) < range)
            {
                enemyGun.target = car.gameObject ;
                if (isNoCD())
                {
                    launchMissile();

                    setCD(waitFor);
                }

            }
        }
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, range);
        }


        public void launchMissile()
        {
            GameObject go = Instantiate(missle);
            Destroy(go, 5);
            go.transform.position = toSpawn.transform.position;
            //Quaternion qua = go.transform.rotation;
            //qua.eulerAngles = new Vector3(0, 0, 90+enemyGun.getRotation());
            //go.transform.rotation = Quaternion.EulerAngles(0,0,enemyGun.getRotation());
            go.GetComponent<Missile>().launch(enemyGun.getRotation());

        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
        
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
        
        }

        public bool isNoCD()
        {
            return Time.time > time_until;
        }
        public void setCD(float time)
        {
            time_until = time + Time.time;
        }

    }
}