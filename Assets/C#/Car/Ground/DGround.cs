using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Car { 
    public class DGround : MonoBehaviour {

        public static List<IGround> listener=new List<IGround>();
        public int wheel_no = 0;
        public bool onAll = false;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag.ToLower().Equals("ground")|| collision.gameObject.tag.ToLower().Equals("untagged") || (onAll && !collision.gameObject.tag.ToLower().Equals("player"))) { 
                foreach (IGround list in listener) list.OnGround(wheel_no,true);
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.tag.ToLower().Equals("ground")|| collision.gameObject.tag.ToLower().Equals("untagged")||(onAll&& !collision.gameObject.tag.ToLower().Equals("player")))
            {
                foreach (IGround list in listener) list.OnGround(wheel_no, false);
            }
        }
    }
}
