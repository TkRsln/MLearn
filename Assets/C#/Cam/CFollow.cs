using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cam { 
public class CFollow : MonoBehaviour {

    public Transform car;
	// Update is called once per frame
	void LateUpdate () {
            if (car == null) return;
            transform.position = new Vector3(car.position.x, car.position.y < 0 ? 0 : car.position.y, transform.position.z);
	}
}
}
