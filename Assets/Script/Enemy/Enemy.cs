using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	[Range(0.1f,10f)] public float speedMove;
	[Range(0.1f,5f)] public float speedRotation;
	[SerializeField] private float getMoney;
	[SerializeField] private GameObject heartObject;

	private int heart;

	//gun, gunshadow để xử lý cái bóng của enemy
	private Transform gun;
	private Transform gunShadow;

	void Awake(){
		gun = transform.FindChild("gun");
		gunShadow = gun.FindChild("gun_shadow");// GetChild(0);
		if (!gun || !gunShadow) {
			print("ban can dien lai ten gun va ten gunShadow");
		}
		transform.localScale = new Vector3 (0.8f,0.8f,0f);
	}

	void Update(){
		if(gun&&gunShadow)
			gunShadow.position = gun.position + new Vector3(0.1f, -0.1f, 0);

//		if(transform.localScale.x==0||transform.localScale.y==0)
//			transform.localScale = new Vector3 (0.8f,0.8f,0f);
	}

	public void LossHeartEnemy(int lossHeart){
	}

}
