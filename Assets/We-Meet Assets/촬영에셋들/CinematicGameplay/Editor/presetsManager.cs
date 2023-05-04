// All Rights Reserved, Muhammad A.Moniem (mamoniem) 2015  http://www.mamoniem.com 
// v1.4
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;

public class presetsManager : EditorWindow {
	
#region Vars
	static presetsManager presetsManagerWindow;
	static int windowMinSizeX = 500;
	static int windowMinSizeY = 600;
	static int windowMaxSizeX = 500;
	static int windowMaxSizeY = 600;
	
	static int windowSizeX = 500;
	static int windowSizeY = 600;

	Color triggerStatus;
	string triggerStatusText;
	string triggerName;
	GameObject sourceTrigger;

	Color cameraStatus;
	string cameraStatusText;
	string cameraName;
	GameObject sourceCamera;

	string presetName;
	string presetDescription;
	bool useOnce;
	bool canExitToNormal;
	bool exitAfterDuration;
	float exitDuration;

	enum curveTypes {EaseInOut, Linear};
	curveTypes playCurve;
	curveTypes resetCurve;

	Vector3 targetPosition;
	Vector3 targetRotation;
	Vector4 targetRotationNormal;

	float blendingTime;
	float targetCameraFOV;


	bool AddPresetOptions = false;
	string addPresetButtonName;
	Color presetButtonColor;

	Vector2 scrollListPosition;
#endregion

	[MenuItem ("Cinematic Gameplay/Presets Manager")]
	
	#region Init, Update, Start & Awake
	static void Init () {
		presetsManagerWindow = (presetsManager)EditorWindow.GetWindow (typeof(presetsManager));
		presetsManagerWindow.autoRepaintOnSceneChange = true;
		presetsManagerWindow.minSize = new Vector2(windowMinSizeX, windowMinSizeY);
		presetsManagerWindow.maxSize = new Vector2(windowMaxSizeX, windowMaxSizeY);
		presetsManagerWindow.position = new Rect(Screen.width/2,100.0f, windowSizeX,windowSizeY);
		presetsManagerWindow.title = "CGP-PresetsMGR";
		presetsManagerWindow.Show();
	}
	
	void Awake (){

	} 
	
	void Start (){

	}
	
	void Update(){
		if (sourceTrigger != null){
			triggerStatus = Color.green;
			triggerStatusText = "The selected Trigger is: ";
			triggerName = sourceTrigger.gameObject.name;
		}else{
			triggerStatus = Color.red;
			triggerStatusText = "No Trigger Selected, Please select one";
			triggerName = "";
		}

		if (sourceCamera != null){
			cameraStatus = Color.green;
			cameraStatusText = "The selected Camera is: ";
			cameraName = sourceCamera.gameObject.name;
		}else{
			cameraStatus = Color.red;
			cameraStatusText = "No Camera Selected, Please select one";
			cameraName = "";
		}

		if (AddPresetOptions){
			addPresetButtonName = "Cancel Preset Addition";
			presetButtonColor = Color.red;
		}else{
			addPresetButtonName = "Add Preset";
			presetButtonColor = Color.green;
		}
	}
	
	void OnInspectorUpdate (){
		this.Repaint ();
	}
	#endregion
	
	void OnGUI () {
		GameObject thePresetsPrefab;
		int thePresetsAmount;
		thePresetsPrefab = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/CinematicGameplay/Presets/Presets.prefab", typeof(GameObject));
		thePresetsAmount = thePresetsPrefab.gameObject.GetComponent<cinematicPresets>().presetsList.Count;

		GUILayout.BeginHorizontal();
		GUI.contentColor = Color.yellow;
		GUILayout.Label ("Total Saved Presets: [" + thePresetsAmount + "]", EditorStyles.boldLabel);
		GUI.contentColor = Color.white;
		if(GUILayout.Button ("Select The presets data prefab")){
			EditorGUIUtility.PingObject((GameObject) AssetDatabase.LoadAssetAtPath("Assets/CinematicGameplay/Presets/Presets.prefab", typeof(GameObject)));
		}
		GUILayout.EndHorizontal();

		GUI.contentColor = triggerStatus;
		GUILayout.Label(triggerStatusText+" " + triggerName, EditorStyles.boldLabel);
		sourceTrigger = (GameObject)EditorGUILayout.ObjectField("Selected Cinematic Trigger :", sourceTrigger, typeof (GameObject), true);

		GUI.contentColor = cameraStatus;
		GUILayout.Label(cameraStatusText+" " + cameraName, EditorStyles.boldLabel);
		sourceCamera = (GameObject)EditorGUILayout.ObjectField("Selected Camera View :", sourceCamera, typeof (GameObject), true);

		GUI.contentColor = Color.white;
		GUILayout.Space(20);

		GUI.contentColor = presetButtonColor;
		if (GUILayout.Button (addPresetButtonName)){
			AddPresetOptions = !AddPresetOptions;
		}
		GUI.contentColor = Color.white;

		if (AddPresetOptions){
			GUILayout.BeginVertical();
			presetName = EditorGUILayout.TextField ("Preset Name: ", presetName);
			presetDescription = EditorGUILayout.TextField ("Preset Description: ", presetDescription);
			useOnce = EditorGUILayout.ToggleLeft ("Use Once: ", useOnce);
			canExitToNormal = EditorGUILayout.ToggleLeft ("Can exit to normal: ", canExitToNormal);
			exitAfterDuration = EditorGUILayout.ToggleLeft ("Exit after duration: ", exitAfterDuration);
			exitDuration = EditorGUILayout.FloatField ("Waiting Duration: ", exitDuration);
			playCurve = (curveTypes)EditorGUILayout.EnumPopup ("Play Animation Curve: ", playCurve);
			resetCurve = (curveTypes)EditorGUILayout.EnumPopup ("Reset Animation Curve: ", resetCurve);
			GUILayout.EndVertical();


			GUI.contentColor = Color.yellow;
			GUILayout.BeginHorizontal();
			if (GUILayout.Button ("Pick Coords")){
				if (sourceCamera){
					EditorGUIUtility.PingObject(sourceCamera);
					EditorPrefs.SetFloat("cinematicCameraPosX", sourceCamera.gameObject.transform.localPosition.x);
					EditorPrefs.SetFloat("cinematicCameraPosY", sourceCamera.gameObject.transform.localPosition.y);
					EditorPrefs.SetFloat("cinematicCameraPosZ", sourceCamera.gameObject.transform.localPosition.z);
					
					EditorPrefs.SetFloat("cinematicCameraRotX", sourceCamera.gameObject.transform.localEulerAngles.x);
					EditorPrefs.SetFloat("cinematicCameraRotY", sourceCamera.gameObject.transform.localEulerAngles.y);
					EditorPrefs.SetFloat("cinematicCameraRotZ", sourceCamera.gameObject.transform.localEulerAngles.z);
					
					targetPosition.x = EditorPrefs.GetFloat("cinematicCameraPosX");
					targetPosition.y = EditorPrefs.GetFloat("cinematicCameraPosY");
					targetPosition.z = EditorPrefs.GetFloat("cinematicCameraPosZ");
					
					targetRotationNormal.x = EditorPrefs.GetFloat("cinematicCameraRotX");
					targetRotationNormal.y = EditorPrefs.GetFloat("cinematicCameraRotY");
					targetRotationNormal.z = EditorPrefs.GetFloat("cinematicCameraRotZ");
				}
			}
			if (GUILayout.Button ("Reset Coords")){
				targetPosition = Vector3.zero;
				targetRotation = Vector3.zero;
				targetRotationNormal = Vector4.zero;
			}
			GUILayout.EndHorizontal();

			GUI.contentColor = Color.white;
			targetPosition = EditorGUILayout.Vector3Field ("Target Position: ", targetPosition);
			targetRotation = EditorGUILayout.Vector3Field ("Target Rotation: ", targetRotation);
			targetRotationNormal = EditorGUILayout.Vector4Field ("Target Rotation Normal: ", targetRotationNormal);

			blendingTime = EditorGUILayout.FloatField ("Blending Time: ", blendingTime);
			targetCameraFOV = EditorGUILayout.FloatField ("Target FOV: ", targetCameraFOV);

			GUI.contentColor = Color.green;
			if (GUILayout.Button ("Save Preset")){
				thePresetsPrefab.gameObject.GetComponent<cinematicPresets>().presetsList.Add (new cinematicPresets.singlePreset());
				//save the attributes
				cinematicPresets.singlePreset theNewlyAddedPreset;
				theNewlyAddedPreset = thePresetsPrefab.gameObject.GetComponent<cinematicPresets>().presetsList[thePresetsAmount];
				theNewlyAddedPreset.presetName = presetName;
				theNewlyAddedPreset.presetDescription = presetDescription;
				theNewlyAddedPreset.useOnce = useOnce;
				theNewlyAddedPreset.canExitToNormal = canExitToNormal;
				theNewlyAddedPreset.exitAfterDuration = exitAfterDuration;
				theNewlyAddedPreset.waitingDuration = exitDuration;
				if (playCurve == curveTypes.EaseInOut){
					theNewlyAddedPreset.playAnimationCurve = cinematicPresets.singlePreset.curveTypes.EaseInOut;
				}else{
					theNewlyAddedPreset.playAnimationCurve = cinematicPresets.singlePreset.curveTypes.Linear;
				}
				if (resetCurve == curveTypes.EaseInOut){
					theNewlyAddedPreset.resetAnimationCurve = cinematicPresets.singlePreset.curveTypes.EaseInOut;
				}else{
					theNewlyAddedPreset.resetAnimationCurve = cinematicPresets.singlePreset.curveTypes.Linear;
				}
				theNewlyAddedPreset.targetPosition = targetPosition;
				theNewlyAddedPreset.targetRotation = targetRotation;
				theNewlyAddedPreset.targetRotationNormal = targetRotationNormal;
				theNewlyAddedPreset.blendingTime = blendingTime;
				theNewlyAddedPreset.targetCameraFOV = targetCameraFOV;
				AddPresetOptions = !AddPresetOptions;
			}
			GUI.contentColor = Color.red;
			if (GUILayout.Button ("Cancel Preset Addition")){
				AddPresetOptions = !AddPresetOptions;
			}
		}else{
			GUI.contentColor = Color.white;
			GUILayout.Space(30);
			if (thePresetsAmount >0){
				scrollListPosition = EditorGUILayout.BeginScrollView (scrollListPosition, GUILayout.Width (windowMaxSizeX-50), GUILayout.Height (400));
				for(int c=0; c<thePresetsAmount; c++){
					GUILayout.BeginHorizontal();
					GUI.contentColor = Color.yellow;
					GUILayout.Label("Preset" + "["  + c.ToString() + "]: "
					                           + thePresetsPrefab.gameObject.GetComponent<cinematicPresets>().presetsList[c].presetName,
					                           GUILayout.Width(230));
					//GUILayout.Label ();
					GUI.contentColor = Color.green;
					if (GUILayout.Button("Apply", GUILayout.Width(90))){
						if (sourceTrigger){
							CinematicCameraTrigger activeTRigger;
							activeTRigger = sourceTrigger.GetComponent<CinematicCameraTrigger>();
							activeTRigger.UseOnce = thePresetsPrefab.gameObject.GetComponent<cinematicPresets>().presetsList[c].useOnce;
							activeTRigger.CanExitToNormal = thePresetsPrefab.gameObject.GetComponent<cinematicPresets>().presetsList[c].canExitToNormal;
							activeTRigger.waitingDuration = thePresetsPrefab.gameObject.GetComponent<cinematicPresets>().presetsList[c].waitingDuration;
							if (thePresetsPrefab.gameObject.GetComponent<cinematicPresets>().presetsList[c].playAnimationCurve == cinematicPresets.singlePreset.curveTypes.EaseInOut){
								activeTRigger.playAnimationCurve = CinematicCameraTrigger.curveTypes.EaseInOut;
							}else{
								activeTRigger.playAnimationCurve = CinematicCameraTrigger.curveTypes.Linear;
							}
							if (thePresetsPrefab.gameObject.GetComponent<cinematicPresets>().presetsList[c].resetAnimationCurve == cinematicPresets.singlePreset.curveTypes.EaseInOut){
								activeTRigger.resetAnimationCurve = CinematicCameraTrigger.curveTypes.EaseInOut;
							}else{
								activeTRigger.resetAnimationCurve = CinematicCameraTrigger.curveTypes.Linear;
							}
							activeTRigger.TargetedPosition = thePresetsPrefab.gameObject.GetComponent<cinematicPresets>().presetsList[c].targetPosition;
							activeTRigger.TargetedRotation = thePresetsPrefab.gameObject.GetComponent<cinematicPresets>().presetsList[c].targetRotation;
							activeTRigger.TargetedRotationNormal = thePresetsPrefab.gameObject.GetComponent<cinematicPresets>().presetsList[c].targetRotationNormal;
							activeTRigger.BlendingTimeOnSeconds = thePresetsPrefab.gameObject.GetComponent<cinematicPresets>().presetsList[c].blendingTime;
							activeTRigger.FOVValue = thePresetsPrefab.gameObject.GetComponent<cinematicPresets>().presetsList[c].targetCameraFOV;
						}
					}
					GUI.contentColor = Color.red;
					if (GUILayout.Button("Delete", GUILayout.Width(90))){
						thePresetsPrefab.gameObject.GetComponent<cinematicPresets>().presetsList.RemoveAt(c);
					}
					GUI.contentColor = Color.white;
					GUILayout.EndHorizontal();
				}
				EditorGUILayout.EndScrollView();
			}
		}


	}
		

}
