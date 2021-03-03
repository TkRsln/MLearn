using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car { 
    public class DObject : MonoBehaviour {

        private static List<IObject> listeners = new List<IObject>();
        public List<GameObject> In = new List<GameObject>();
        public string[] ignore = new string[0];
        public float[] angle = new float[4];
        public float size = 1;
        public float ang = 335;
        public Vector3 toMove = new Vector3(0,0,0);

        [Space]
        public bool debug = false;

        

        void OnDrawGizmosSelected()
         {
            if (!debug) return;

            //Gizmos.color = Color.green;
            Vector3 direction = Vector3.right;
            Gizmos.color = Color.cyan;
            Vector3 pos = transform.position + toMove;
            Gizmos.DrawLine(pos, pos + (Rotate(direction, ang + transform.rotation.eulerAngles.z) * size));

            /*
            float z = transform.rotation.eulerAngles.z;
            for (int i =0;i < 4;i++)
            {
                if (i==1||i==2)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(transform.position, (transform.position + (Rotate(direction, angle[i]+z) * size/4*3)));

                    Gizmos.color = Color.magenta;
                    Gizmos.DrawLine(transform.position + (Rotate(direction, angle[i]+z) * size / 4 * 3), (transform.position + (Rotate(direction,z+angle[i]) * size)));
                }
                else
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(transform.position, (transform.position + (Rotate(direction, angle[i]+z) * size)));
                }
            }//*/
        }
        private bool printDebug = true;
        public void checkFront()
        {
            //Vector3 fwd = transform.TransformDirection(Vector3.right);
            Vector3 direction = Vector3.right;
            Vector3 pos = transform.position + toMove;
            Vector3 v = (pos + (Rotate(direction, ang + transform.rotation.eulerAngles.z) * size ));

            RaycastHit2D[] rh = Physics2D.RaycastAll(pos, v.normalized, (v - transform.position).magnitude);
            // for (int i = 0; i < rh.Length; i++) if (rh[i].collider.tag!=("Player")) Debug.Log(">>" + rh[i].collider.tag+" | "+ (rh[i].collider.tag != ("Player"))+" Player!="+rh[i].collider.tag+" | size:"+rh.Length);         
            /*printDebug = true;
            foreach (RaycastHit2D ty in rh)
            {
                if (!ty.collider.tag.Equals("Player"))
                {
                    printDebug = false;
                    Debug.Log("Cukur"+ty.collider.tag);
                }
                else
                {
                    Debug.Log("Cukur" + ty.collider.tag);
                }
            }
            if (printDebug)
            {
                //Debug.Log("Cukur");
                Motor.active.Jump();

            }//*/
            
        }

        public static Vector3 Rotate(Vector3 v, float degrees)
        {
            return Quaternion.Euler(0, 0, degrees) * v;
        }


        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!isContains(col.gameObject.tag))
            {
                In.Add(col.gameObject);
                sendEvent(true, col.gameObject.tag,col.gameObject);
                //Debug.Log("TRI_ENT:" + col.gameObject.tag);
            }
        }
        private void OnTriggerExit2D(Collider2D col)
        {
            if (!isContains(col.gameObject.tag))
            {
                In.Remove(col.gameObject);
                sendEvent(false, col.gameObject.tag,col.gameObject);
            }
        }
        public static void addListener(IObject listener) { listeners.Add(listener); }
        public static void removeListener(IObject listener) { listeners.Remove(listener); }

        public bool isContains(string tag)
        {
            for(int i=0;i<ignore.Length;i++)
            {
                string ig = ignore[i];
                if (ig.Equals(tag)) return true;
                if (tag.Equals(ig)) return true;
                if (ig == tag) return true;
                if (ig.Contains(tag)) return true;
               // Debug.Log("if1>" + (ig.Contains(tag)) + " if2>" + (ig == tag) + " if3>" + (ig.Equals(tag))+"Tag: Untagged=="+tag+("Untagged"==tag));
            }
            return false;
        }
        public void sendEvent(bool Entered,string tag,GameObject obj)
        {
            // if (debug) Debug.Log("Detect Object( Entered:"+Entered+" | Tag:"+tag+" )");
            if (tag.Equals("Player")) return;
            if (Entered) foreach (IObject li in listeners) li.OnEnter(tag,obj);
            else foreach (IObject li in listeners) li.OnExit(tag,obj);
        }
    }
}