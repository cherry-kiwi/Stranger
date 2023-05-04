// All Rights Reserved, Muhammad A.Moniem (mamoniem) 2015  http://www.mamoniem.com 
// v1.4
using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent (typeof (Animation))]	

public class CinematicCameraMotor : MonoBehaviour {

	public AnimationCurve curvePosX ;
	public AnimationCurve curvePosY ;
	public AnimationCurve curvePosZ ;
	
	public AnimationCurve curveRotX ;
	public AnimationCurve curveRotY ;
	public AnimationCurve curveRotZ ;
	public AnimationCurve curveRotW ;

	public AnimationCurve fovCurve;
	
	AnimationClip clip ;
	
	
	void playAnimation (string curveType) {
		Quaternion q1 = Quaternion.Euler(CinematicCameraCalls.targetedRotation);
		if (curveType == "EaseInOut"){
			curvePosX = AnimationCurve.EaseInOut(0, this.gameObject.transform.localPosition.x, CinematicCameraCalls.bndTime, CinematicCameraCalls.targetedPosition.x);
			curvePosY = AnimationCurve.EaseInOut(0, this.gameObject.transform.localPosition.y, CinematicCameraCalls.bndTime, CinematicCameraCalls.targetedPosition.y);
			curvePosZ = AnimationCurve.EaseInOut(0, this.gameObject.transform.localPosition.z, CinematicCameraCalls.bndTime, CinematicCameraCalls.targetedPosition.z);

			curveRotX = AnimationCurve.EaseInOut(0, this.gameObject.transform.localRotation.x, CinematicCameraCalls.bndTime, CinematicCameraCalls.q1.x);
			curveRotY = AnimationCurve.EaseInOut(0, this.gameObject.transform.localRotation.y, CinematicCameraCalls.bndTime, CinematicCameraCalls.q1.y);
			curveRotZ = AnimationCurve.EaseInOut(0, this.gameObject.transform.localRotation.z, CinematicCameraCalls.bndTime, CinematicCameraCalls.q1.z);
			curveRotW = AnimationCurve.EaseInOut(0, this.gameObject.transform.localRotation.w, CinematicCameraCalls.bndTime, CinematicCameraCalls.q1.w);

			if ( this.gameObject.GetComponent <Camera>().orthographic){
				fovCurve = AnimationCurve.EaseInOut(0, this.gameObject.GetComponent <Camera>().orthographicSize, CinematicCameraCalls.bndTime, CinematicCameraCalls.FOVValue);
			}else{
				fovCurve = AnimationCurve.EaseInOut(0, this.gameObject.GetComponent <Camera>().fieldOfView, CinematicCameraCalls.bndTime, CinematicCameraCalls.FOVValue);
			}
		}else{
			curvePosX = AnimationCurve.Linear(0, this.gameObject.transform.localPosition.x, CinematicCameraCalls.bndTime, CinematicCameraCalls.targetedPosition.x);
			curvePosY = AnimationCurve.Linear(0, this.gameObject.transform.localPosition.y, CinematicCameraCalls.bndTime, CinematicCameraCalls.targetedPosition.y);
			curvePosZ = AnimationCurve.Linear(0, this.gameObject.transform.localPosition.z, CinematicCameraCalls.bndTime, CinematicCameraCalls.targetedPosition.z);
			
			curveRotX = AnimationCurve.Linear(0, this.gameObject.transform.localRotation.x, CinematicCameraCalls.bndTime, CinematicCameraCalls.q1.x);
			curveRotY = AnimationCurve.Linear(0, this.gameObject.transform.localRotation.y, CinematicCameraCalls.bndTime, CinematicCameraCalls.q1.y);
			curveRotZ = AnimationCurve.Linear(0, this.gameObject.transform.localRotation.z, CinematicCameraCalls.bndTime, CinematicCameraCalls.q1.z);
			curveRotW = AnimationCurve.Linear(0, this.gameObject.transform.localRotation.w, CinematicCameraCalls.bndTime, CinematicCameraCalls.q1.w);

			if ( this.gameObject.GetComponent <Camera>().orthographic){
				fovCurve = AnimationCurve.Linear(0, this.gameObject.GetComponent <Camera>().orthographicSize, CinematicCameraCalls.bndTime, CinematicCameraCalls.FOVValue);
			}else{
				fovCurve = AnimationCurve.Linear(0, this.gameObject.GetComponent <Camera>().fieldOfView, CinematicCameraCalls.bndTime, CinematicCameraCalls.FOVValue);
			}
		}
		
		clip = new AnimationClip();
		clip.legacy = true;
		
		clip.ClearCurves();
		clip.SetCurve("", typeof(Transform), "localPosition.x", curvePosX);
		clip.SetCurve("", typeof(Transform), "localPosition.y", curvePosY);
		clip.SetCurve("", typeof(Transform), "localPosition.z", curvePosZ);
		
		clip.SetCurve("", typeof(Transform), "localRotation.x", curveRotX);
		clip.SetCurve("", typeof(Transform), "localRotation.y", curveRotY);
		clip.SetCurve("", typeof(Transform), "localRotation.z", curveRotZ);
		clip.SetCurve("", typeof(Transform), "localRotation.w", curveRotW);

		if ( this.gameObject.GetComponent <Camera>().orthographic){
			clip.SetCurve("", typeof(Camera), "orthographic size", fovCurve);
		}else{
			clip.SetCurve("", typeof(Camera), "field of view", fovCurve);
		}

		GetComponent<Animation>().AddClip(clip, "cinematicMovement");
		GetComponent<Animation>().CrossFade("cinematicMovement");

		Debug.Log("animation reseted corectlly");
	}
	
	

	
	
	void resetAnimation (string curveType) {
		Quaternion q2 = Quaternion.Euler(CinematicCameraCalls.defaultRot);
		if (curveType == "EaseInOut"){
			curvePosX = AnimationCurve.EaseInOut(0, this.gameObject.transform.localPosition.x, CinematicCameraCalls.bndTime, CinematicCameraCalls.defaultPos.x);
			curvePosY = AnimationCurve.EaseInOut(0, this.gameObject.transform.localPosition.y, CinematicCameraCalls.bndTime, CinematicCameraCalls.defaultPos.y);
			curvePosZ = AnimationCurve.EaseInOut(0, this.gameObject.transform.localPosition.z, CinematicCameraCalls.bndTime, CinematicCameraCalls.defaultPos.z);
			
			curveRotX = AnimationCurve.EaseInOut(0, this.gameObject.transform.localRotation.x, CinematicCameraCalls.bndTime, q2.x);
			curveRotY = AnimationCurve.EaseInOut(0, this.gameObject.transform.localRotation.y, CinematicCameraCalls.bndTime, q2.y);
			curveRotZ = AnimationCurve.EaseInOut(0, this.gameObject.transform.localRotation.z, CinematicCameraCalls.bndTime, q2.z);
			curveRotW = AnimationCurve.EaseInOut(0, this.gameObject.transform.localRotation.w, CinematicCameraCalls.bndTime, q2.w);

			if ( this.gameObject.GetComponent <Camera>().orthographic){
				fovCurve = AnimationCurve.EaseInOut(0, this.gameObject.GetComponent <Camera>().orthographicSize, CinematicCameraCalls.bndTime, CinematicCameraCalls.defaultFOV);
			}else{
				fovCurve = AnimationCurve.EaseInOut(0, this.gameObject.GetComponent <Camera>().fieldOfView, CinematicCameraCalls.bndTime, CinematicCameraCalls.defaultFOV);
			}
		}else{
			curvePosX = AnimationCurve.Linear(0, this.gameObject.transform.localPosition.x, CinematicCameraCalls.bndTime, CinematicCameraCalls.defaultPos.x);
			curvePosY = AnimationCurve.Linear(0, this.gameObject.transform.localPosition.y, CinematicCameraCalls.bndTime, CinematicCameraCalls.defaultPos.y);
			curvePosZ = AnimationCurve.Linear(0, this.gameObject.transform.localPosition.z, CinematicCameraCalls.bndTime, CinematicCameraCalls.defaultPos.z);
			
			curveRotX = AnimationCurve.Linear(0, this.gameObject.transform.localRotation.x, CinematicCameraCalls.bndTime, q2.x);
			curveRotY = AnimationCurve.Linear(0, this.gameObject.transform.localRotation.y, CinematicCameraCalls.bndTime, q2.y);
			curveRotZ = AnimationCurve.Linear(0, this.gameObject.transform.localRotation.z, CinematicCameraCalls.bndTime, q2.z);
			curveRotW = AnimationCurve.Linear(0, this.gameObject.transform.localRotation.w, CinematicCameraCalls.bndTime, q2.w);

			if ( this.gameObject.GetComponent <Camera>().orthographic){
				fovCurve = AnimationCurve.Linear(0, this.gameObject.GetComponent <Camera>().orthographicSize, CinematicCameraCalls.bndTime, CinematicCameraCalls.defaultFOV);
			}else{
				fovCurve = AnimationCurve.Linear(0, this.gameObject.GetComponent <Camera>().fieldOfView, CinematicCameraCalls.bndTime, CinematicCameraCalls.defaultFOV);
			}
		}
		
		clip = new AnimationClip();
		clip.legacy = true;

		clip.ClearCurves();
		clip.SetCurve("", typeof(Transform), "localPosition.x", curvePosX);
		clip.SetCurve("", typeof(Transform), "localPosition.y", curvePosY);
		clip.SetCurve("", typeof(Transform), "localPosition.z", curvePosZ);
		
		clip.SetCurve("", typeof(Transform), "localRotation.x", curveRotX);
		clip.SetCurve("", typeof(Transform), "localRotation.y", curveRotY);
		clip.SetCurve("", typeof(Transform), "localRotation.z", curveRotZ);
		clip.SetCurve("", typeof(Transform), "localRotation.w", curveRotW);

		if ( this.gameObject.GetComponent <Camera>().orthographic){
			clip.SetCurve("", typeof(Camera), "orthographic size", fovCurve);
		}else{
			clip.SetCurve("", typeof(Camera), "field of view", fovCurve);
		}

		
		GetComponent<Animation>().AddClip(clip, "cinematicMovement");
		GetComponent<Animation>().CrossFade("cinematicMovement");
	}
}
