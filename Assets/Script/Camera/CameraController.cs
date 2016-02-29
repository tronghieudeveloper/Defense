using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	[Range(1.1f,5f)] [SerializeField] private float speedZoom;
	[SerializeField] private float speedMoveZoom;
	[SerializeField] private float maxZoomSize;
	[SerializeField] private GameObject mapObject;
	[SerializeField] private GameObject cameraObject;
	[SerializeField] private float zoomSize=1.2f;

	private float zoomSizeMin=1f;
	private Vector3 positionMouseDown;
	private Vector3 distanceWithMouseDown;

	void Update () 
	{
		ZoomMap ();
//		MoveMap ();
//		if (zoomSize <= zoomSizeMin){ 
//			mapObject.transform.position=new Vector3(0,mapObject.transform.position.y,0f);
//		}
	}
	float curDist,lastDist=0f;
	void ZoomMap(){
#if UNITY_EDITOR
		//Zoom Camera
		if (Input.GetAxis ("Mouse ScrollWheel") > 0) {
			if(zoomSize>zoomSizeMin*speedZoom){
				zoomSize /= speedZoom;
			}
			else
				zoomSize=zoomSizeMin;
		}
		if (Input.GetAxis ("Mouse ScrollWheel") < 0) {
			if(zoomSize<maxZoomSize/speedZoom)
				zoomSize *= speedZoom;
			else
				zoomSize=maxZoomSize;
		}
		cameraObject.GetComponent<Camera>().orthographicSize =zoomSize;
		//mapObject.transform.localScale=new Vector3(zoomSize,zoomSize,0f);
#else
		if (Input.touchCount > 1 && (Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetTouch(1).phase == TouchPhase.Moved)) 
		{
			//Two finger touch does pinch to zoom
			var touch1 = Input.GetTouch(0);
			var touch2 = Input.GetTouch(1);
			curDist = Vector2.Distance(touch1.position, touch2.position);
			if(curDist > lastDist)
			{
				if(zoomSize<maxZoomSize)
					zoomSize += speedZoom;
			}else{
				if(zoomSize>zoomSizeMin)
					zoomSize -= speedZoom;
			}
			lastDist = curDist;
			mapObject.transform.localScale=new Vector3(zoomSize,zoomSize,0f);
		}
#endif
	}

	void MoveMap(){
		//move Map
#if UNITY_EDITOR
		if (Input.GetMouseButtonDown (0)) {
			positionMouseDown=Camera.main.ScreenToWorldPoint(Input.mousePosition);
			distanceWithMouseDown=mapObject.transform.position-positionMouseDown;
		}		
		if (Input.GetMouseButton (0)) {
			mapObject.transform.position=Camera.main.ScreenToWorldPoint(Input.mousePosition)+distanceWithMouseDown;
		}
#else
		if (Input.touchCount <= 1) {
			if(Input.GetTouch(0).phase == TouchPhase.Began){
				positionMouseDown=Camera.main.ScreenToWorldPoint(Input.mousePosition);
				distanceWithMouseDown=mapObject.transform.position-positionMouseDown;
			}
			if(Input.GetTouch(0).phase == TouchPhase.Moved)
				mapObject.transform.position=Camera.main.ScreenToWorldPoint(Input.mousePosition)+distanceWithMouseDown;
		}
#endif
		float directionX = 5.0908f * (zoomSize - 1f);
		float directionY=5.0908f*zoomSize-2.86f;
		if(mapObject.transform.position.x<=-(directionX)){
			mapObject.transform.position=new Vector3(-directionX+0.1f,mapObject.transform.position.y,mapObject.transform.position.z);
		}
		if(mapObject.transform.position.x>=(directionX)){
			mapObject.transform.position=new Vector3(directionX-0.1f,mapObject.transform.position.y,mapObject.transform.position.z);
		}
		if(mapObject.transform.position.y>=(directionY)){
			mapObject.transform.position=new Vector3(mapObject.transform.position.x,directionY-0.1f,mapObject.transform.position.z);
		}
		if(mapObject.transform.position.y<=-(directionY)){
			mapObject.transform.position=new Vector3(mapObject.transform.position.x,-directionY+0.1f,mapObject.transform.position.z);
		}
	}
}
