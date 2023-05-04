// All Rights Reserved, Muhammad A.Moniem (mamoniem) 2015  http://www.mamoniem.com 
// v1.4
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class cinematicPresets : MonoBehaviour {

	[Serializable]
	public class singlePreset {
		public string presetName;
		public string presetDescription;
		public bool useOnce;
		public bool canExitToNormal;
		public bool exitAfterDuration;
		public float waitingDuration;
		public enum curveTypes {EaseInOut, Linear};
		public curveTypes playAnimationCurve;
		public curveTypes resetAnimationCurve;
		public Vector3 targetPosition;
		public Vector3 targetRotation;
		public Vector4 targetRotationNormal;
		public float blendingTime;
		public float targetCameraFOV;
	}

	
	public List <singlePreset> presetsList ;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
