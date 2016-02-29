using UnityEngine;
using System.Collections;

public class TestShadow : MonoBehaviour {

    private Transform gun;
    private Transform gunShadow;

	void Start () {
        gun = transform.FindChild("gun");

        gunShadow = gun.FindChild("gun_shadow");// GetChild(0);
	}
	
	void Update () {
  //      gunShadow.rotation = gun.rotation;
        gunShadow.position = gun.position + new Vector3(0.1f, -0.1f, 0);
        //gun.position = Vector3.one;
	}

}
