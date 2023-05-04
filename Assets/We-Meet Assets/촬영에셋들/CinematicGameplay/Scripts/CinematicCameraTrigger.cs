// All Rights Reserved, Muhammad A.Moniem (mamoniem) 2015  http://www.mamoniem.com 
// v1.4
using UnityEngine;
using System.Collections;


public class CinematicCameraTrigger : MonoBehaviour {
	
	private GameObject renderingCamera ;
	
	
	public bool UseOnce ;
	
	
	public bool CanExitToNormal ;

	public bool exitAfterDuration;
	public float waitingDuration;
	
	public enum curveTypes {EaseInOut, Linear};
	public curveTypes playAnimationCurve;
	public curveTypes resetAnimationCurve;

	public float FOVValue ;
	
	public Vector3 TargetedPosition ;
	public Vector3 TargetedRotation ;
	public Vector4 TargetedRotationNormal ;
	
	public Vector3 CameraDefaultPosition ;
	public Vector4 CameraDefaultRotation ;
	
	public float BlendingTimeOnSeconds;
	
	enum CinematicPresetList {None, TPS, FPS, Top, HighAngel, DownQuest, EagleEye};
	CinematicPresetList CinematicPreset ;

	
	public GameObject[] ActivationEntities;

	public bool is2D ;

	#region Expermintal
	bool motionEffect;
	float motionEffectTimes;
	float motionEffectDuration;
	float motionEffectAmount;
	float motionEffectSplitBy;
	bool TimeFreez ;
	float FreezingPeriod ;
	#endregion

	
	
	void Awake () {
		this.gameObject.GetComponent<Renderer>().enabled = false;
	}
	void Start () {
		renderingCamera = Camera.main.gameObject;
		renderingCamera.gameObject.SendMessage ("cinematic1", 1.0f);
		renderingCamera.gameObject.SendMessage ("cinematic2", 50.0f);
		renderingCamera.gameObject.SendMessage ("cinematic3", CameraDefaultPosition);
		renderingCamera.gameObject.SendMessage ("cinematic4", CameraDefaultRotation);

		//Debug.Log(playAnimationCurve.ToString());
		//Debug.Log(resetAnimationCurve.ToString());
	}
	
	void Update () {
		
	}
	
	void OnTriggerEnter (Collider other){
		if (!is2D){
			for (int c=0; c<ActivationEntities.Length; c++){
				if (other.gameObject.name == ActivationEntities[c].name){
					/*if (UseOnce == true){
						this.gameObject.collider.enabled = false ;
					}*/
					if (CinematicPreset == CinematicPresetList.None){
						//				TargetedPosition = Vector3.zero ;
						//				TargetedRotation = Vector4.zero ;
						messaging();
						renderingCamera.gameObject.SendMessage ("cinematic1", BlendingTimeOnSeconds);
						renderingCamera.gameObject.SendMessage ("cinematic2", FOVValue);
						renderingCamera.gameObject.SendMessage ("cinematic3", CameraDefaultPosition);
						renderingCamera.gameObject.SendMessage ("cinematic4", CameraDefaultRotation);
					}else if (CinematicPreset == CinematicPresetList.TPS){
						TargetedPosition = new Vector3(-5.83288f, 1.76965f, 7.2388f) ;
						TargetedRotation = new Vector4(1.819901f, 61.40001f, -32.4f, 0.0f) ;
						messaging();
					}else if (CinematicPreset == CinematicPresetList.FPS){
						TargetedPosition = new Vector3(-0.8667077f, 1.589924f, 9.946441f) ;
						TargetedRotation = new Vector4(-10.5f, 64.01257f, -32.5f, 0.0f) ;
						messaging();
					}else if (CinematicPreset == CinematicPresetList.Top){
						TargetedPosition = new Vector3(0.4923877f, 7.013456f, 1.613622f) ;
						TargetedRotation = new Vector4(36.7f, -6.6f, 15.95f, 0.0f) ;
						messaging();
					}else if (CinematicPreset == CinematicPresetList.HighAngel){
						TargetedPosition = new Vector3(-9.690356f, 3.669313f, 3.877897f) ;
						TargetedRotation = new Vector4(20.37004f, 48.93536f, -35.9f, 0.0f) ;
						messaging();
					}else if (CinematicPreset == CinematicPresetList.DownQuest){
						TargetedPosition = new Vector3(-4.27f, 0.506f, 7.8f) ;
						TargetedRotation = new Vector4(1.82f, 61.4f, 3.899f, 0.0f) ;
						messaging();
					}else if (CinematicPreset == CinematicPresetList.EagleEye){
						TargetedPosition = new Vector3(0.0f, 5.0f, -7.0f);
						TargetedRotation = new Vector4(20.0f, 0.0f, 20.0f, 0.0f);
						messaging();
					}

					if (exitAfterDuration){
						this.gameObject.GetComponent<Collider>().enabled = false ;
						UseOnce = true;
						CanExitToNormal = true;
						StartCoroutine(exitAfterDurationOfTime());

					}
				}
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other){
		if (is2D){
			for (int c=0; c<ActivationEntities.Length; c++){
				if (other.gameObject.name == ActivationEntities[c].name){
					/*if (UseOnce == true){
						this.gameObject.collider.enabled = false ;
					}*/
					if (CinematicPreset == CinematicPresetList.None){
						//				TargetedPosition = Vector3.zero ;
						//				TargetedRotation = Vector4.zero ;
						messaging();
						renderingCamera.gameObject.SendMessage ("cinematic1", BlendingTimeOnSeconds);
						renderingCamera.gameObject.SendMessage ("cinematic2", FOVValue);
						renderingCamera.gameObject.SendMessage ("cinematic3", CameraDefaultPosition);
						renderingCamera.gameObject.SendMessage ("cinematic4", CameraDefaultRotation);
					}else if (CinematicPreset == CinematicPresetList.TPS){
						TargetedPosition = new Vector3(-5.83288f, 1.76965f, 7.2388f) ;
						TargetedRotation = new Vector4(1.819901f, 61.40001f, -32.4f, 0.0f) ;
						messaging();
					}else if (CinematicPreset == CinematicPresetList.FPS){
						TargetedPosition = new Vector3(-0.8667077f, 1.589924f, 9.946441f) ;
						TargetedRotation = new Vector4(-10.5f, 64.01257f, -32.5f, 0.0f) ;
						messaging();
					}else if (CinematicPreset == CinematicPresetList.Top){
						TargetedPosition = new Vector3(0.4923877f, 7.013456f, 1.613622f) ;
						TargetedRotation = new Vector4(36.7f, -6.6f, 15.95f, 0.0f) ;
						messaging();
					}else if (CinematicPreset == CinematicPresetList.HighAngel){
						TargetedPosition = new Vector3(-9.690356f, 3.669313f, 3.877897f) ;
						TargetedRotation = new Vector4(20.37004f, 48.93536f, -35.9f, 0.0f) ;
						messaging();
					}else if (CinematicPreset == CinematicPresetList.DownQuest){
						TargetedPosition = new Vector3(-4.27f, 0.506f, 7.8f) ;
						TargetedRotation = new Vector4(1.82f, 61.4f, 3.899f, 0.0f) ;
						messaging();
					}else if (CinematicPreset == CinematicPresetList.EagleEye){
						TargetedPosition = new Vector3(0.0f, 5.0f, -7.0f);
						TargetedRotation = new Vector4(20.0f, 0.0f, 20.0f, 0.0f);
						messaging();
					}
					
					if (exitAfterDuration){
						this.gameObject.GetComponent<Collider>().enabled = false ;
						UseOnce = true;
						CanExitToNormal = true;
						StartCoroutine(exitAfterDurationOfTime());
						
					}
				}
			}
		}
	}

	
	
	void OnTriggerExit (Collider other){
		if (!is2D){
			for (int c=0; c<ActivationEntities.Length; c++){
				if (other.gameObject.name == ActivationEntities[c].name){
					if (CanExitToNormal == true){
						if (UseOnce == true){
							this.gameObject.GetComponent<Collider>().enabled = false ;
						}
						if (CinematicPreset == CinematicPresetList.None){
							renderingCamera.gameObject.SendMessage ("cinematic1", BlendingTimeOnSeconds);
							renderingCamera.gameObject.SendMessage ("cinematic2", FOVValue); //50 is the normal FOV
							renderingCamera.gameObject.SendMessage ("cinematic3", CameraDefaultPosition);
							renderingCamera.gameObject.SendMessage ("cinematic4", CameraDefaultRotation);
							renderingCamera.gameObject.SendMessage ("resetCamera", resetAnimationCurve.ToString());
						}else{
							renderingCamera.gameObject.SendMessage ("resetCamera", resetAnimationCurve.ToString());
						}
					}
				}
			}
		}
	}

	void OnTriggerExit2D (Collider2D other){
		if (is2D){
			for (int c=0; c<ActivationEntities.Length; c++){
				if (other.gameObject.name == ActivationEntities[c].name){
					if (CanExitToNormal == true){
						if (UseOnce == true){
							this.gameObject.GetComponent<Collider>().enabled = false ;
						}
						if (CinematicPreset == CinematicPresetList.None){
							renderingCamera.gameObject.SendMessage ("cinematic1", BlendingTimeOnSeconds);
							renderingCamera.gameObject.SendMessage ("cinematic2", FOVValue); //50 is the normal FOV
							renderingCamera.gameObject.SendMessage ("cinematic3", CameraDefaultPosition);
							renderingCamera.gameObject.SendMessage ("cinematic4", CameraDefaultRotation);
							renderingCamera.gameObject.SendMessage ("resetCamera", resetAnimationCurve.ToString());
						}else{
							renderingCamera.gameObject.SendMessage ("resetCamera", resetAnimationCurve.ToString());
						}
					}
				}
			}
		}
	}

	IEnumerator exitAfterDurationOfTime(){
		yield return new WaitForSeconds (waitingDuration);
		if (CanExitToNormal == true){
			if (UseOnce == true){
				this.gameObject.GetComponent<Collider>().enabled = false ;
			}
			if (CinematicPreset == CinematicPresetList.None){
				renderingCamera.gameObject.SendMessage ("cinematic1", BlendingTimeOnSeconds);
				renderingCamera.gameObject.SendMessage ("cinematic2", FOVValue); //50 is the normal FOV
				renderingCamera.gameObject.SendMessage ("cinematic3", CameraDefaultPosition);
				renderingCamera.gameObject.SendMessage ("cinematic4", CameraDefaultRotation);
				renderingCamera.gameObject.SendMessage ("resetCamera", resetAnimationCurve.ToString());
			}else{
				renderingCamera.gameObject.SendMessage ("resetCamera", resetAnimationCurve.ToString());
			}
		}
	}
	
	void messaging(){
		if (TimeFreez == true){
			renderingCamera.gameObject.SendMessage ("addFreez", FreezingPeriod);
		}
		if (FOVValue != 50){
			//renderingCamera.gameObject.SendMessage ("cinematic1", BlendingTimeOnSeconds);
			//renderingCamera.gameObject.SendMessage ("cinematic2", 50);
			//renderingCamera.gameObject.SendMessage ("cinematic3", CameraDefaultPosition);
			//renderingCamera.gameObject.SendMessage ("cinematic4", CameraDefaultRotation);
		}
		renderingCamera.gameObject.SendMessage ("adjustFOV", FOVValue);
		renderingCamera.gameObject.SendMessage ("adjustPosition", TargetedPosition);
		renderingCamera.gameObject.SendMessage ("adjustRotation", TargetedRotationNormal);
		renderingCamera.gameObject.SendMessage ("cinematicPlayCurve", playAnimationCurve.ToString());
		renderingCamera.gameObject.SendMessage ("cinematicChange", BlendingTimeOnSeconds);
	}
	
}