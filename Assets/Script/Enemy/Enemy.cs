using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	[Range(0.1f,10f)] public float speedMove;
	[Range(0.1f,5f)] public float speedRotation;
	[SerializeField] private float getMoney;
	private int heart;
	[SerializeField] private GameObject heartObject;

	void Awake(){
		transform.localScale = new Vector3 (0.8f,0.8f,0f);
	}

	void Update(){
		if(transform.localScale.x==0||transform.localScale.y==0)
			transform.localScale = new Vector3 (0.8f,0.8f,0f);
	}

	public void LossHeartEnemy(int lossHeart){
	}

}
