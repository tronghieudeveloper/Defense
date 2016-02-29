using UnityEngine;
using System.Collections;
using System.IO;
//[ExecuteInEditMode]
public class EnemyMoveController : MonoBehaviour {

	[SerializeField] private int mapIndex;
	[SerializeField] private int turnIndex;
	[Range(1, 100)] public float timeTurnDelay=2f; //public chi de hien thi ra inspector
	[Range(0.1f,8f)] public float timeDelayToCallOneEnemy=0.2f; //public chi de hien thi ra inspector
	[Range(0f,8f)] public float timeDelayToCallEachSortEnemy=0f; //public chi de hien thi ra inspector
	[SerializeField] private bool isTurnAutomatic;
	[SerializeField] private int numberTurnInMap;
	[SerializeField] private Transform enemyObjects;
	[SerializeField] Transform[] tranformFlag;
	[SerializeField] GameObject[] gameObjectMove;


	[SerializeField] private bool isRotate = true, isSmoothRotate = true;

	private int NUMBER_SORT_ENEMY=15;  // số loại enemy hiện có là 15
	private int[] numberEnemyEachSort=new int[15]; //mảng lưu các số lượng enemy mỗi loại trên file txt
	void Awake () {
		UpdateParameterNumberEnemyInTurn (turnIndex);
		StartCoroutine(CallEnemyEachTurn ());	
	}

	void UpdateParameterNumberEnemyInTurn(int tIndex){
		char[] delemiter = {':'};
		string like = "";
		int numberSortEnemyIndex = 0;
		string saveMapTurn = "map" + mapIndex + "_turn" + tIndex;
		int numberConvert;
#if UNITY_EDITOR
		//truyen vao turn nao thi cap nhat thong so o file txt cua turn day va luu chung lai o saveMapTurn
		string mapTurn = "map" + mapIndex + "_turn" + tIndex +".txt";
		System.IO.StreamReader file = new System.IO.StreamReader ("Assets/Save_File_Txt/Number_Enemy_Each_Turn/"+mapTurn);
		string saveNumber;
		PlayerPrefs.SetString (""+saveMapTurn, "");
		while ((like=file.ReadLine())!=null) {
			string[] fields=like.Split(delemiter);
			saveNumber=fields[1];
			PlayerPrefs.SetString(""+saveMapTurn,""+PlayerPrefs.GetString(""+saveMapTurn)+""+saveNumber+":");
			int.TryParse(fields[1],out numberConvert);
			numberEnemyEachSort[numberSortEnemyIndex]=numberConvert;
			numberSortEnemyIndex++;
		}
//		like=PlayerPrefs.GetString(""+saveMapTurn);
//		string[] fields1=like.Split(delemiter);
//		for (int i=0; i<NUMBER_SORT_ENEMY; i++) {
//			//int.TryParse(fields1[i],out numberConvert);
//			print(fields1[i]);
//		}

#else
		like=PlayerPrefs.GetString(""+saveMapTurn);
		string[] fields1=like.Split(delemiter);
		for (int i=0; i<NUMBER_SORT_ENEMY; i++) {
			int.TryParse(fields1[i],out numberConvert);
			numberEnemyEachSort[numberSortEnemyIndex]=numberConvert;
			numberSortEnemyIndex++;
		}
#endif
	}

	//goi enemy theo tung turn
	IEnumerator CallEnemyEachTurn(){
		int sortEnemyIndex=0;
		for (int i=0; i<NUMBER_SORT_ENEMY; i++) {
			if(numberEnemyEachSort[sortEnemyIndex]>0){
				for(int j=0;j<numberEnemyEachSort[sortEnemyIndex];j++){
					GameObject enemyObject=Instantiate(gameObjectMove[sortEnemyIndex],tranformFlag[0].position,Quaternion.identity)as GameObject;
					enemyObject.transform.SetParent(enemyObjects.transform);
					StartCoroutine(Movement(enemyObject));
					yield return new WaitForSeconds(timeDelayToCallOneEnemy);
				}
			}
			yield return new WaitForSeconds(timeDelayToCallEachSortEnemy);
			sortEnemyIndex++;
		}
		if (isTurnAutomatic) {
			if(turnIndex<numberTurnInMap){
				yield return new WaitForSeconds(timeTurnDelay);
				InvokeRepeating("CallNewTurnEnemy",0f,1.8f);
			}
			else{
				CancelInvoke("CallNewTurnEnemy");
				print("Map "+mapIndex+" co "+numberTurnInMap+" turn. Da choi het so turn." );
			}
		} else {
			print("Ban dang chon che do test tung turn");
		}
	}

	//them ham nay de check khi nao tren ban do khong con con enemy nao thi moi goi turn tiep theo
	void CallNewTurnEnemy(){
		if(enemyObjects.childCount==0){
			turnIndex++;
			UpdateParameterNumberEnemyInTurn(turnIndex);
			StartCoroutine (CallEnemyEachTurn());
		}
	}
	
	// Ham di chuyen theo map
	IEnumerator Movement(GameObject gameObjectMoveMent)
	{
		for(int i = 0; i < tranformFlag.Length; i++)
		{
			yield return StartCoroutine(MoveGameObject (gameObjectMoveMent, tranformFlag[i]));
		}
		//chạy hết vòng for = gameObjectMovement đã đi đến điểm cuối cùng
		Destroy(gameObjectMoveMent);
	}
	
	// Di chuyen 1 doi tuong giua 2 vi tri
	IEnumerator MoveGameObject(GameObject objectMove, Transform end)
	{
		while(objectMove.transform.position != end.position)
		{
			float speedMove=objectMove.GetComponent<Enemy>().speedMove;
			float speedRotation=objectMove.GetComponent<Enemy>().speedRotation;
			Vector3 positionMediate = Vector3.MoveTowards(objectMove.transform.position, end.position, speedMove/100f);
			objectMove.transform.position = positionMediate;
			if(isRotate)
			{
				float angleRotate = Mathf.Atan2(objectMove.transform.position.x - end.position.x, objectMove.transform.position.y - end.position.y);
				if(objectMove.transform.position.x != end.position.x)
				{
					if(!isSmoothRotate)
						objectMove.transform.rotation = Quaternion.Euler(0f, 0f, 180f - angleRotate * 180f/Mathf.PI);
					else
						SmoothRotate(objectMove.transform,new Vector3(0f, 0f, 180f - angleRotate * 180f/Mathf.PI),speedRotation);
				}
			}
			yield return null;
		}
	}
	
	void SmoothRotate(Transform start, Vector3 end,float speedRotation)
	{
		if (Mathf.Abs (Vector3.RotateTowards (start.eulerAngles, end, 0.1f, speedRotation).z)>float.Epsilon) {
			Vector3 angleMediate = Vector3.one;
			if (start.eulerAngles.z > 180f && end.z < 180f && start.eulerAngles.z - end.z > 180f) {
				angleMediate = Vector3.RotateTowards (start.eulerAngles, new Vector3 (0f, 0f, 362f), 0.1f, speedRotation);
			} else if (start.eulerAngles.z < 180f && end.z > 180f && end.z - start.eulerAngles.z > 180f) {
				angleMediate = Vector3.RotateTowards (start.eulerAngles, new Vector3 (0f, 0f, 0f), 0.1f, speedRotation);
				if (angleMediate.z == 0f)
					angleMediate.z = 358f;
			} else
				angleMediate = Vector3.RotateTowards (start.eulerAngles, end, 0.1f, speedRotation);
			start.rotation = Quaternion.Euler (0f, 0f, angleMediate.z);
		}
	}
}
