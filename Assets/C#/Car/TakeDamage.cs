using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Car {
    public class TakeDamage : MonoBehaviour
    {
        public float damage = 15f;
       
        private void OnCollisionEnter2D(Collision2D col)
        {
           
            if (col.gameObject.tag.ToLower().Equals("player"))
            {
                Respawn rs = col.gameObject.GetComponent<Respawn>();
                if (rs != null) rs.Damage(damage, ERespawn.OnDamage);
            }

        }
    }
}
