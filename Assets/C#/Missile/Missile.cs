using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Missile : MonoBehaviour {

    public bool Lock=false;
	// Use this for initialization
	public void launch(float angle)
    {
        if(Lock)transform.LookAt(Car.Respawn.active.transform);

        Quaternion qua = transform.rotation;
        qua.eulerAngles = new Vector3(0, 0, 90+angle);
        //go.transform.rotation = Quaternion.EulerAngles(0,0,enemyGun.getRotation());
        transform.rotation = qua;

        GetComponent<Rigidbody2D>().velocity = transform.up * 10;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag.Equals("Player"))
        {
            Car.DObject obj = Car.Respawn.active.GetComponent<Car.DObject>();
            obj.In.Add(col.gameObject);
            ZV1.Karar.active.checkOut(gameObject.tag, this.gameObject);
            //obj.sendEvent(true, col.gameObject.tag, col.gameObject);
            Car.Respawn.active.Damage(20, ERespawn.OnDamage);
            Destroy(gameObject);
        }
    }


}
