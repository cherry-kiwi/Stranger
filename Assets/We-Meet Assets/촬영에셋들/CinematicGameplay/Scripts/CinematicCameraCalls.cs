// All Rights Reserved, Muhammad A.Moniem (mamoniem) 2015  http://www.mamoniem.com 
// v1.4
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class CinematicCameraCalls : MonoBehaviour {

	public static Vector3 targetedPosition ;
	public static Vector4 targetedRotation ;

	public float blendingAnimationTimeFactor ;
	
	public float time ;
	public float i ;
	public float rate ;
	
	public float waitingTime ;
	
	public int complete = 1;

	public float motionTimesWrap ;
	public float motionAmountWrap ;
	public float motionDurationWrap ;
	public float motionSplitWrap ;

	public static Vector3 currentPosition ;
	public static Vector3 currentRotation ;

	public static Vector3 defaultPos ;
	public static Vector4 defaultRot ;
	
	public static float bndTime ;
	public static Quaternion q1 ;

	public static float defaultFOV;
	public static float FOVValue;

	private string playCurve;

	void Start (){
		complete = 1 ;
		if ( this.gameObject.GetComponent <Camera>().orthographic){
			defaultFOV = this.GetComponent<Camera>().orthographicSize;
		}else{
			defaultFOV = this.GetComponent<Camera>().fieldOfView;
		}
	}
	
	void adjustPosition (Vector3 pos){
		targetedPosition = pos ;
	}
	
	
	void adjustRotation (Vector4 rot){
		targetedRotation = rot ;
		q1 = Quaternion.Euler(targetedRotation);
	}

	void adjustFOV (float fov){
		FOVValue = fov;
	}

	void cinematicPlayCurve (string curveName){
		playCurve = curveName;
	}

	void cinematicChange (float blendingTime){
		bndTime = blendingTime ;
		Camera.main.gameObject.SendMessage ("playAnimation", playCurve);
		Debug.Log(playCurve);
	}
	void resetCamera (string curveName){
		targetedPosition = Vector3.zero ;
		targetedRotation = Vector4.zero ;
		Camera.main.gameObject.SendMessage ("resetAnimation", curveName);
		Debug.Log(curveName);
	}
	
	public void cinematic1 (float blend){
		blendingAnimationTimeFactor = blend ;
	}
	
	public void cinematic2 (float fovValue){
		if (fovValue <=0.0f){
			fovValue = 50.0f ;	
		}
		FOVValue = fovValue;
	}

	public void cinematic3 (Vector3 pos){
		defaultPos = pos ;
	}

	public void cinematic4 (Vector4 rot){
		defaultRot = rot ;
	}
	
	void addFreez (float tm){

	}
}
