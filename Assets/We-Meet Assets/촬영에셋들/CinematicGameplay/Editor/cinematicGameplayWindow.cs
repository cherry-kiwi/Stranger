// All Rights Reserved, Muhammad A.Moniem (mamoniem) 2015  http://www.mamoniem.com 
// v1.4
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.IO;

public class cinematicGameplayWindow : EditorWindow {
	
	#region Vars
	static cinematicGameplayWindow cinematicGameplayEditor;
	static int windowMinSizeX = 500;
	static int windowMinSizeY = 950;
	static int windowMaxSizeX = 500;
	static int windowMaxSizeY = 950;
	
	static int windowSizeX = 500;
	static int windowSizeY = 950;
	
	string coordsOptionsText;
	
	GameObject sourceTrigger = null;
	bool thereIsTrigger = false;
	
	Color firstColor = Color.red;
	Color secondColor = Color.red;
	
	string cameraStatus = "No Camera Object" ;
	string cameraName = " " ;
	
	string triggerStatus = "No Trigger Object" ;
	string triggerName = " " ;
	
	public RenderTexture renderTexture ;
	Camera camera = null ;
	
	bool thereIsCamera = false;

	bool alwaysMainCamera = false;
	bool cameraDefaultIsZero = true;

	bool playTesting = false;
	bool showHelp = true;
	bool streamingCamera = true;

	string helpNotification = "Select your preview settings and apply it to see a more clear preview !";

	Texture2D tex;

	double previewStartTime;
	AnimationCurve curvePosX ;
	AnimationCurve curvePosY ;
	AnimationCurve curvePosZ ;
	AnimationCurve curveRotX ;
	AnimationCurve curveRotY ;
	AnimationCurve curveRotZ ;
	AnimationCurve curveRotW ;
	AnimationCurve curveFOV ;
	AnimationClip clip ;
	Quaternion q1 ;
	float blendTime;

	Vector3 tempDefaultPos;
	Vector3 tempDefaultRot;
	Quaternion EditorDefaultRot;

	enum previewAALevels {
		m_0 = 0,
		m_1 = 1,
		m_2 = 2,
		m_4 = 3,
		m_8 = 4
	}
	previewAALevels AALevel = previewAALevels.m_0;
	int aaValueHolder = 8;

	enum previewAspectRatios {
		m_5to4 = 0,
		m_4to3 = 1,
		m_3to2 = 2,
		m_16to10 = 3,
		m_16to9 = 4
	}
	previewAspectRatios previewAspect = previewAspectRatios.m_5to4;
	int ratioValueHolder;
	
	#endregion
	
	[MenuItem ("Cinematic Gameplay/CGP Window")]
	
	#region Init, Update, Start & Awake
	static void Init () {
		cinematicGameplayEditor = (cinematicGameplayWindow)EditorWindow.GetWindow (typeof(cinematicGameplayWindow));
		cinematicGameplayEditor.autoRepaintOnSceneChange = true;
		cinematicGameplayEditor.minSize = new Vector2(windowMinSizeX, windowMinSizeY);
		cinematicGameplayEditor.maxSize = new Vector2(windowMaxSizeX, windowMaxSizeY);
		cinematicGameplayEditor.position = new Rect(Screen.width/2,100.0f, windowSizeX,windowSizeY);
		cinematicGameplayEditor.title = "CGP";
		cinematicGameplayEditor.Show();
	}
	
	void Awake (){
		setTheRenderTexture();
		camera = null ;
	} 

	void Start (){
		showHelp = true;
		EditorDefaultRot = camera.gameObject.transform.localRotation;
		tempDefaultPos = camera.gameObject.transform.localPosition;
	}
	
	void Update(){
		camera = Camera.main ;
		if(camera != null){
			thereIsCamera = true ;
			camera.targetTexture = renderTexture;
			camera.Render();
			camera.targetTexture = null;
			firstColor = Color.green ;
			cameraStatus = "Rendering Through : [" + camera.gameObject.name + "]" ;
		}else{
			thereIsCamera = false ;
			camera = Camera.main ;
			renderTexture = new RenderTexture((int)position.width,  (int)position.height, 24, RenderTextureFormat.ARGB32 ); // Resources.Load ("rt", typeof (RenderTexture));
			firstColor = Color.red ;
			cameraStatus = "No Camera Object";
		}
		if(renderTexture != null && (renderTexture.width != position.width || renderTexture.height != position.height) && thereIsCamera == true){
		
		}
		
		if (sourceTrigger != null){
			thereIsTrigger = true;
			secondColor = Color.green ;
			triggerStatus = "Set value into the trigger : [" + sourceTrigger.gameObject.name + "]" ;
			coordsOptionsText = "Coords Options";
		}else{
			thereIsTrigger = false;
			secondColor = Color.red ;
			triggerStatus = "No Trigger Object";
			coordsOptionsText = "[Select Camera & Trigger to Enable]";
		}
		
		if (EditorApplication.isPlayingOrWillChangePlaymode){
			//streamingCamera = false;	
		}
		checkAALevel();
		checkAspectRatio();
	}
	
	void OnInspectorUpdate (){
		Repaint ();
	}
	#endregion
	
	#region checkAA
	void checkAALevel(){
		switch (AALevel){
			case previewAALevels.m_0:
				aaValueHolder = 1 ;
				break;
		case previewAALevels.m_1:
				aaValueHolder = 1 ;
				break;
			case previewAALevels.m_2:
				aaValueHolder = 2 ;
				break;
			case previewAALevels.m_4:
				aaValueHolder = 4 ;
				break;
			case previewAALevels.m_8:
				aaValueHolder = 8 ;
				break;
			default:
				aaValueHolder = 8 ;
				break;
		}
	}
	#endregion
	
	#region checkAspectRation
	void checkAspectRatio(){
		switch (previewAspect){
		case previewAspectRatios.m_5to4:
			ratioValueHolder = 1 ;
			break;
		case previewAspectRatios.m_4to3:
			ratioValueHolder = 1 ;
			break;
		case previewAspectRatios.m_3to2:
			ratioValueHolder = 2 ;
			break;
		case previewAspectRatios.m_16to10:
			ratioValueHolder = 4 ;
			break;
		case previewAspectRatios.m_16to9:
			ratioValueHolder = 8 ;
			break;
		default:
			ratioValueHolder = 8 ;
			break;
		}
	}
	#endregion
	
	#region set, reset & do the preview
	void setTheRenderTexture(){
		thereIsCamera = false ;
		renderTexture = new RenderTexture((int)position.width,  (int)position.height, 16, RenderTextureFormat.R8 );// Resources.Load ("rt", typeof (RenderTexture));
		renderTexture.antiAliasing = aaValueHolder ;
		renderTexture.useMipMap = false;
		renderTexture.autoGenerateMips = false;
	}
	void reSetTheRenderTexture(){
		thereIsCamera = false ;
		renderTexture = new RenderTexture((int)camera.pixelWidth,  (int)camera.pixelHeight, 24, RenderTextureFormat.ARGB32 );// Resources.Load ("rt", typeof (RenderTexture));
		renderTexture.antiAliasing = aaValueHolder ;
		renderTexture.useMipMap = false;
		renderTexture.autoGenerateMips = false;
	}
	void DoPreview()
	{
		double timeElapsed = EditorApplication.timeSinceStartup - previewStartTime;
		clip.SampleAnimation(camera.gameObject, (float)timeElapsed);
	}
	#endregion

	void OnGUI () {

		EditorStyles.textField.wordWrap = true;
		EditorStyles.label.wordWrap = true;

		GUI.contentColor = Color.white;
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		alwaysMainCamera = EditorGUILayout.Toggle("Always Main Camera", alwaysMainCamera);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.Space();
		if(GUILayout.Button("Insert Cube Cinematic Trigger")){
			insertCube();
		}
		if(GUILayout.Button("Insert Sphere Cinematic Trigger")){
			insertSphere();
		}
		if(GUILayout.Button("Mark Selected as a Cinematic Trigger")){
			markSelected();
		}
		if(GUILayout.Button("Unmark Selected as a Cinematic Trigger")){
			unMarkSelected();
		}
		if(GUILayout.Button("Delete all Cinematic Triggers in Scene")){
			deleteAll();
		}
		if(GUILayout.Button("Enable all Cinematic Triggers in Scene")){
			enableAll();
		}
		if(GUILayout.Button("Disable all Cinematic Triggers in Scene")){
			disableAll();
		}
		if(GUILayout.Button("Set selected as the main Entity to all cinematic triggers in the scene")){
			setMainEntity();
		}


		EditorGUILayout.Space();
		EditorGUILayout.Space();

		GUILayout.Label("Camera Object Status :", EditorStyles.boldLabel);

		GUI.contentColor = firstColor;
		GUILayout.Label(cameraStatus+cameraName, EditorStyles.boldLabel);
		
		camera = (Camera)EditorGUILayout.ObjectField( "Camera View is :", camera, typeof (Camera), true);
		GUI.contentColor = Color.white;
		if(GUILayout.Button("Prepare Camera")){
			if (camera != null){
				if (camera.gameObject.GetComponent ("CinematicCameraCalls")){
					
				}else{
					camera.gameObject.AddComponent<CinematicCameraCalls>();
				}
				if (camera.gameObject.GetComponent ("CinematicCameraMotor")){
					
				}else{
					camera.gameObject.AddComponent<CinematicCameraMotor>();
				}
			}
		}

		GUI.contentColor = secondColor;
		GUILayout.Label(triggerStatus+triggerName, EditorStyles.boldLabel);
		sourceTrigger = (GameObject)EditorGUILayout.ObjectField("Selected Cinematic Trigger :", sourceTrigger, typeof (GameObject), true);

		EditorGUILayout.Space();
		EditorGUILayout.Space();

		GUI.contentColor = Color.cyan ;
		EditorStyles.label.wordWrap = true;

		if (thereIsCamera == true){
			//GUI.DrawTexture(new Rect( 15, 300, position.width/2, position.height/2), renderTexture, ScaleMode.ScaleToFit, false, 0.0F ); //100, 170 -> 
			//GUI.DrawTexture(new Rect( 100, 300, camera.pixelWidth/2, camera.pixelHeight/2), renderTexture, ScaleMode.ScaleToFit, false, camera.aspect ); //100, 170 -> 
		}


		GUI.contentColor = Color.black;
		thereIsTrigger = EditorGUILayout.BeginToggleGroup(coordsOptionsText, thereIsTrigger);
		GUI.contentColor = Color.yellow;
		if(GUILayout.Button(">>Set as Camera's default coords<<")){
			tempDefaultPos = camera.gameObject.transform.localPosition;
			tempDefaultRot = new Vector3(camera.gameObject.transform.localRotation.eulerAngles.x,
			                             camera.gameObject.transform.localRotation.eulerAngles.y,
			                             camera.gameObject.transform.localRotation.eulerAngles.z);

			Debug.Log("Camera Default have been set to " + tempDefaultPos + "  ::  " + tempDefaultRot);
		}
		
		if(GUILayout.Button(">>Reset Camera to default<<")){
			resetCameraCoords();		

		}
		if(GUILayout.Button(">>Pick Camera Coords<<")){
			if (camera){
				EditorGUIUtility.PingObject(camera);
				EditorPrefs.SetFloat("cinematicCameraPosX", camera.gameObject.transform.localPosition.x);
				EditorPrefs.SetFloat("cinematicCameraPosY", camera.gameObject.transform.localPosition.y);
				EditorPrefs.SetFloat("cinematicCameraPosZ", camera.gameObject.transform.localPosition.z);

				EditorPrefs.SetFloat("cinematicCameraFOV", camera.fieldOfView);

				EditorPrefs.SetFloat("cinematicCameraRotX", camera.gameObject.transform.localEulerAngles.x);
				EditorPrefs.SetFloat("cinematicCameraRotY", camera.gameObject.transform.localEulerAngles.y);
				EditorPrefs.SetFloat("cinematicCameraRotZ", camera.gameObject.transform.localEulerAngles.z);

				Debug.Log ("Captured position is : " + EditorPrefs.GetFloat("cinematicCameraPosX") + "/" + EditorPrefs.GetFloat("cinematicCameraPosY") + "/" + EditorPrefs.GetFloat("cinematicCameraPosZ") + "  ::  Captured rotation is : " + EditorPrefs.GetFloat("cinematicCameraRotX") + "/" + EditorPrefs.GetFloat("cinematicCameraRotY") + "/" + EditorPrefs.GetFloat("cinematicCameraRotZ"));
			}
		}
		

		if(GUILayout.Button(">>Paste Coords To Trigger<<")){
			if (sourceTrigger){
				sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().CameraDefaultPosition.x = tempDefaultPos.x;
				sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().CameraDefaultPosition.y = tempDefaultPos.y;
				sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().CameraDefaultPosition.z = tempDefaultPos.z;

				sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().CameraDefaultRotation.x = tempDefaultRot.x;
				sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().CameraDefaultRotation.y = tempDefaultRot.y;
				sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().CameraDefaultRotation.z = tempDefaultRot.z;

				sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().TargetedPosition.x = EditorPrefs.GetFloat("cinematicCameraPosX");
				sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().TargetedPosition.y = EditorPrefs.GetFloat("cinematicCameraPosY");
				sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().TargetedPosition.z = EditorPrefs.GetFloat("cinematicCameraPosZ");

				sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().TargetedRotationNormal.x = EditorPrefs.GetFloat("cinematicCameraRotX");
				sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().TargetedRotationNormal.y = EditorPrefs.GetFloat("cinematicCameraRotY");
				sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().TargetedRotationNormal.z = EditorPrefs.GetFloat("cinematicCameraRotZ");

				sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().FOVValue = EditorPrefs.GetFloat("cinematicCameraFOV");

				Debug.Log ("Added position is : " + EditorPrefs.GetFloat("cinematicCameraPosX") + "/" + EditorPrefs.GetFloat("cinematicCameraPosY") + "/" + EditorPrefs.GetFloat("cinematicCameraPosZ"));

				EditorGUIUtility.PingObject(sourceTrigger);
			}
		}
		EditorGUILayout.EndToggleGroup();
		
		
		GUI.contentColor = Color.red ;
		if(GUILayout.Button(">>DELETE MEMORY SAVED DATA<<")){
			EditorPrefs.DeleteAll();
			GUIContent content = new GUIContent("Memory Cleaned successfully !");
			cinematicGameplayEditor.ShowNotification(content);
		}

		EditorGUILayout.Space();

		GUI.contentColor = Color.white ;
		AALevel = (previewAALevels)EditorGUILayout.EnumPopup("Preview AA",AALevel);
		previewAspect = (previewAspectRatios)EditorGUILayout.EnumPopup("Preview AspectRatio",previewAspect);

		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Apply Preview")){
			reSetTheRenderTexture();
		}
		if(GUILayout.Button("Revert to Normal")){
			AALevel = previewAALevels.m_0;
			checkAALevel();
			reSetTheRenderTexture();
		}

		GUI.contentColor = Color.green ;
		if(GUILayout.Button("Stream") && !streamingCamera){
			streamingCamera = true;
			GUIContent content = new GUIContent("Camera Streaming now !!!!");
			cinematicGameplayEditor.ShowNotification(content);
		}

		GUI.contentColor = Color.red ;
		if(GUILayout.Button("Stop Streaming") && streamingCamera){
			streamingCamera = false;

			GUIContent content = new GUIContent("No Camera Stream !!!!");
			cinematicGameplayEditor.ShowNotification(content);
		}
		if(camera != null){
			if (streamingCamera){
				if (renderTexture != null){
					EditorGUI.DrawPreviewTexture (new Rect( 15, 555, 470, 350), renderTexture, null, ScaleMode.ScaleToFit, 0.0f);
				}
			}else{

			}
		}else{

		}

		if (!playTesting){
			GUI.contentColor = Color.green ;
			if (GUI.Button(new Rect(200, 910, 60, 25), "Play")){
				EditorDefaultRot = camera.gameObject.transform.localRotation;
				if (sourceTrigger){
					playTesting = true;
					previewStartTime = EditorApplication.timeSinceStartup;
					AnimationMode.StartAnimationMode();
					clip = new AnimationClip();
					if (sourceTrigger.gameObject){
						blendTime = sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().BlendingTimeOnSeconds ;
					}else{
						blendTime = 2;
					}
					clip.SampleAnimation(camera.gameObject, clip.length - Time.time);

					if (sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().playAnimationCurve == CinematicCameraTrigger.curveTypes.EaseInOut){
						curvePosX = AnimationCurve.EaseInOut(0, camera.gameObject.transform.localPosition.x, blendTime, sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().TargetedPosition.x); //curvePosX = AnimationCurve.EaseInOut(0, EditorPrefs.GetFloat("cinematicCameraDefaultPosX"), blendTime, EditorPrefs.GetFloat("cinematicCameraPosX"));
						curvePosY = AnimationCurve.EaseInOut(0, camera.gameObject.transform.localPosition.y, blendTime, sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().TargetedPosition.y);
						curvePosZ = AnimationCurve.EaseInOut(0, camera.gameObject.transform.localPosition.z, blendTime, sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().TargetedPosition.z);
					}else{
						curvePosX = AnimationCurve.Linear(0, camera.gameObject.transform.localPosition.x, blendTime, sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().TargetedPosition.x); //curvePosX = AnimationCurve.EaseInOut(0, EditorPrefs.GetFloat("cinematicCameraDefaultPosX"), blendTime, EditorPrefs.GetFloat("cinematicCameraPosX"));
						curvePosY = AnimationCurve.Linear(0, camera.gameObject.transform.localPosition.y, blendTime, sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().TargetedPosition.y);
						curvePosZ = AnimationCurve.Linear(0, camera.gameObject.transform.localPosition.z, blendTime, sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().TargetedPosition.z);
					}
					
					Vector3 targetedRotation = new Vector3(sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().TargetedRotationNormal.x,
					                                       sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().TargetedRotationNormal.y,
					                                       sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().TargetedRotationNormal.z) ;
					q1 = Quaternion.Euler(targetedRotation);

					if (sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().playAnimationCurve == CinematicCameraTrigger.curveTypes.EaseInOut){
						curveRotX = AnimationCurve.EaseInOut(0, camera.gameObject.transform.localRotation.x, blendTime, q1.x); //curveRotX = AnimationCurve.EaseInOut(0, EditorPrefs.GetFloat("cinematicCameraDefaultRotX"), blendTime, q1.x);
						curveRotY = AnimationCurve.EaseInOut(0, camera.gameObject.transform.localRotation.y, blendTime, q1.y);
						curveRotZ = AnimationCurve.EaseInOut(0, camera.gameObject.transform.localRotation.z, blendTime, q1.z);
						curveRotW = AnimationCurve.EaseInOut(0, camera.gameObject.transform.localRotation.w, blendTime, q1.w);
					}else{
						curveRotX = AnimationCurve.Linear(0, camera.gameObject.transform.localRotation.x, blendTime, q1.x); //curveRotX = AnimationCurve.EaseInOut(0, EditorPrefs.GetFloat("cinematicCameraDefaultRotX"), blendTime, q1.x);
						curveRotY = AnimationCurve.Linear(0, camera.gameObject.transform.localRotation.y, blendTime, q1.y);
						curveRotZ = AnimationCurve.Linear(0, camera.gameObject.transform.localRotation.z, blendTime, q1.z);
						curveRotW = AnimationCurve.Linear(0, camera.gameObject.transform.localRotation.w, blendTime, q1.w);
					}

					if (sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().playAnimationCurve == CinematicCameraTrigger.curveTypes.EaseInOut){
						if ( camera.gameObject.GetComponent <Camera>().orthographic){
							curveFOV = AnimationCurve.EaseInOut(0, camera.orthographicSize, blendTime, sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().FOVValue);
						}else{
							curveFOV = AnimationCurve.EaseInOut(0, camera.fieldOfView, blendTime, sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().FOVValue);
						}
					}else{
						if ( camera.gameObject.GetComponent <Camera>().orthographic){
							curveFOV = AnimationCurve.Linear(0, camera.orthographicSize, blendTime, sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().FOVValue);
						}else{
							curveFOV = AnimationCurve.Linear(0, camera.fieldOfView, blendTime, sourceTrigger.gameObject.GetComponent<CinematicCameraTrigger>().FOVValue);
						}
					}

					clip.ClearCurves();
					clip.SetCurve("", typeof(Transform), "localPosition.x", curvePosX);
					clip.SetCurve("", typeof(Transform), "localPosition.y", curvePosY);
					clip.SetCurve("", typeof(Transform), "localPosition.z", curvePosZ);
					
					clip.SetCurve("", typeof(Transform), "localRotation.x", curveRotX);
					clip.SetCurve("", typeof(Transform), "localRotation.y", curveRotY);
					clip.SetCurve("", typeof(Transform), "localRotation.z", curveRotZ);
					clip.SetCurve("", typeof(Transform), "localRotation.w", curveRotW);

					if ( camera.gameObject.GetComponent <Camera>().orthographic){
						clip.SetCurve("", typeof(Camera), "orthographic size", curveFOV);
					}else{
						clip.SetCurve("", typeof(Camera), "field of view", curveFOV);
					}


					camera.gameObject.GetComponent<Animation>().AddClip(clip, "cinematicGameplayPreview");
					EditorApplication.update += DoPreview;
				}else{
					GUIContent content = new GUIContent("Please Assign a [Cenimatic Gameplay Trigger] First");
					cinematicGameplayEditor.ShowNotification(content);
				}
			}
		}else{
			GUI.contentColor = Color.red ;
			if (GUI.Button(new Rect(200, 910, 60, 25), "Stop")){
				playTesting = false;
				AnimationMode.StopAnimationMode();
				EditorApplication.update -= DoPreview;
				resetCameraCoords();
				if (camera.gameObject.GetComponent<Animation>().GetClip("cinematicGameplayPreview") != null){
					camera.gameObject.GetComponent<Animation>().RemoveClip("cinematicGameplayPreview");
				}


			}
		}

		if (showHelp){
			if (cinematicGameplayEditor){
				cinematicGameplayEditor.minSize = new Vector2(windowMinSizeX, windowMinSizeY);
				GUIContent content = new GUIContent(helpNotification);
				cinematicGameplayEditor.ShowNotification(content);
				showHelp = false;
			}
		}else{
			
		}
	}
	
	#region insert cube or sphere trigger
	void insertCube(){
		GameObject cubeTrigger = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/CinematicGameplay/Prefabs/cinematicGameplayCubeTrigger.prefab", typeof (GameObject));
		GameObject instantiatedOne = (GameObject) PrefabUtility.InstantiatePrefab(cubeTrigger);
		sourceTrigger = instantiatedOne.gameObject;
	}
	void insertSphere(){
		GameObject sphereTrigger = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/CinematicGameplay/Prefabs/cinematicGameplaySphereTrigger.prefab", typeof (GameObject));
		GameObject instantiatedOne = (GameObject) PrefabUtility.InstantiatePrefab(sphereTrigger);
		sourceTrigger = instantiatedOne.gameObject;
	}
	#endregion
	
	#region mark and unmark selected as trigger
	void markSelected(){
		Texture2D triggerIconGizmo = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/CinematicGameplay/Gizmos/cinematic.png", typeof (Texture2D));
		GameObject selectedOne = Selection.activeGameObject;
		var editorGUIUtilityType = typeof(EditorGUIUtility);
		var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
		var args = new object[] { selectedOne, triggerIconGizmo };
		editorGUIUtilityType.InvokeMember("SetIconForObject", bindingFlags, null, null, args);

		if (selectedOne.gameObject.GetComponent<Collider>() == null){
			selectedOne.gameObject.AddComponent<BoxCollider>();
			selectedOne.gameObject.GetComponent<Collider>().isTrigger = true;
		}else{
			selectedOne.gameObject.GetComponent<Collider>().isTrigger = true;
		}
		if (selectedOne.gameObject.GetComponent("CinematicCameraTrigger")){

		}else{
			selectedOne.gameObject.AddComponent<CinematicCameraTrigger>();
		}
		sourceTrigger = selectedOne.gameObject;
	}
	void unMarkSelected(){
		Texture2D triggerIconGizmo = (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/CinematicGameplay/Gizmos/cinematic.png", typeof (Texture2D));
		GameObject selectedOne = Selection.activeGameObject;
		var editorGUIUtilityType = typeof(EditorGUIUtility);
		var bindingFlags = BindingFlags.InvokeMethod | BindingFlags.Static | BindingFlags.NonPublic;
		var args = new object[] { selectedOne, null };
		editorGUIUtilityType.InvokeMember("SetIconForObject", bindingFlags, null, null, args);
		if (selectedOne.gameObject.GetComponent<Collider>() == null){

		}else{
			DestroyImmediate(selectedOne.gameObject.GetComponent<Collider>());
		}
		if (selectedOne.gameObject.GetComponent("CinematicCameraTrigger")){
			DestroyImmediate(selectedOne.gameObject.GetComponent("CinematicCameraTrigger"));
		}else{

		}

	}
	#endregion
	
	#region delete, enable & disable ALL triggers
	void deleteAll(){
		int foundObjects = 0;
		object[] sceneObject = GameObject.FindSceneObjectsOfType(typeof (GameObject));
		foreach (object anObject in sceneObject)
		{
			GameObject inProgressGameObject = (GameObject) anObject;
			if (inProgressGameObject.gameObject.GetComponent("CinematicCameraTrigger")){
				foundObjects +=1;
				DestroyImmediate(inProgressGameObject.gameObject);
			}else{
				
			}
		}
		Debug.Log("Found [" +foundObjects+"] gameplayCinematic trigger and removed them !");
	}
	void enableAll(){
		int foundObjects = 0;
		object[] sceneObject = GameObject.FindSceneObjectsOfType(typeof (GameObject));
		foreach (object anObject in sceneObject)
		{
			GameObject inProgressGameObject = (GameObject) anObject;
			if (inProgressGameObject.gameObject.GetComponent("CinematicCameraTrigger")){
				foundObjects +=1;
				inProgressGameObject.gameObject.GetComponent<Collider>().enabled = true;
			}else{
				
			}
		}
		Debug.Log("Found [" +foundObjects+"] gameplayCinematic trigger and Enabled them !");
	}
	void disableAll(){
		int foundObjects = 0;
		object[] sceneObject = GameObject.FindSceneObjectsOfType(typeof (GameObject));
		foreach (object anObject in sceneObject)
		{
			GameObject inProgressGameObject = (GameObject) anObject;
			if (inProgressGameObject.gameObject.GetComponent("CinematicCameraTrigger")){
				foundObjects +=1;
				inProgressGameObject.gameObject.GetComponent<Collider>().enabled = false;
			}else{
				
			}
		}
		Debug.Log("Found [" +foundObjects+"] gameplayCinematic trigger and Disabled them !");
	}
	#endregion
	
	#region set a main Entity
	void setMainEntity(){
		GameObject selectedEntity = Selection.activeGameObject;
		int foundObjects = 0;
		object[] sceneObject = GameObject.FindSceneObjectsOfType(typeof (GameObject));
		foreach (object anObject in sceneObject)
		{
			if (selectedEntity != null){
				GameObject inProgressGameObject = (GameObject) anObject;
				if (inProgressGameObject.gameObject.GetComponent("CinematicCameraTrigger")){
					foundObjects +=1;
					inProgressGameObject.gameObject.GetComponent<CinematicCameraTrigger>().ActivationEntities[0] = selectedEntity.gameObject;
				}else{
					
				}
				Debug.Log("The main Entity to use on triggerin have been for [" +foundObjects+"] CinematicGameplay Trigger");
			}else{
				Debug.Log("Select an Entity first !!!!");
			}
		}
	}
	#endregion

	void resetCameraCoords(){
		float tempEuler = 0.0f;
		Vector4 valueHolder = new Vector4();
		Vector3 posHolder = new Vector3();

		Vector3 tempVal = camera.gameObject.transform.localEulerAngles;
		tempVal = tempDefaultRot;
		camera.gameObject.transform.localEulerAngles = tempVal;

		posHolder = camera.gameObject.transform.localPosition;
		posHolder = tempDefaultPos;
		camera.gameObject.transform.localPosition = posHolder;
		
		Debug.Log(posHolder + "  <::>  " + tempVal);

	}
}
