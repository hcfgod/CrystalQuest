using UnityEngine;
using UnityEditor;

namespace FrustumCullingSpace
{
    [CustomEditor(typeof(FrustumCulling))]
    [CanEditMultipleObjects]

    public class FrustumCullingCustomInspector : Editor
    {
        SerializedProperty autoCatchCamera,
        mainCam,
        activationDirection,
        runEveryFrames,
        cullInScene,
        disableRootObject,
        distanceCulling,
        distanceToCull,
        prioritizeDistanceCulling,
        distanceCullingOnly;

        FrustumCulling[] scripts;
        

        void OnEnable()
        {
            autoCatchCamera = serializedObject.FindProperty("autoCatchCamera");
            mainCam = serializedObject.FindProperty("mainCam");
            activationDirection = serializedObject.FindProperty("activationDirection");
            runEveryFrames = serializedObject.FindProperty("runEveryFrames");
            cullInScene = serializedObject.FindProperty("cullInScene");
            disableRootObject = serializedObject.FindProperty("disableRootObject");
            distanceCulling = serializedObject.FindProperty("distanceCulling");
            distanceToCull = serializedObject.FindProperty("distanceToCull");
            prioritizeDistanceCulling = serializedObject.FindProperty("prioritizeDistanceCulling");
            distanceCullingOnly = serializedObject.FindProperty("distanceCullingOnly");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update ();

            var button = GUILayout.Button("Click for more tools");
            if (button) Application.OpenURL("https://assetstore.unity.com/publishers/39163");
            EditorGUILayout.Space(10);

            FrustumCulling script = (FrustumCulling) target;
            
            Object[] monoObjects = targets;
            scripts = new FrustumCulling[monoObjects.Length];
            for (int i = 0; i < monoObjects.Length; i++) {
                scripts[i] = monoObjects[i] as FrustumCulling;
            }


            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Camera Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(autoCatchCamera, new GUIContent("Auto Catch Camera", "Automatically get the main active camera on startup."));
            if (script.autoCatchCamera == false) EditorGUILayout.PropertyField(mainCam, new GUIContent("Main Cam", "Manually drag and drop the game camera here, better for performance on game start."));
            EditorGUILayout.PropertyField(activationDirection, new GUIContent("Activation Direction", "Enable/disable object when a ledge screen point >= this value. Better to leave this property alone."));
            EditorGUILayout.PropertyField(runEveryFrames, new GUIContent("Run Every Frames", "Run the logic and checks every (this set) frames. The larger the number, the better the performance, but may cause inaccuracies. Suggested from 5-7. Depends on the pace of your game."));
            EditorGUILayout.PropertyField(cullInScene, new GUIContent("Cull In Scene", "Show the culling in scene view. This may decrease precision. It may disable the object before it gets out of view. This property is editor only and on game build the system automatically falls back to max precision by not taking this into account."));
            

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Objects Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(disableRootObject, new GUIContent("Disable Root Object", "Will apply culling on the root object."));


            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Distance Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(distanceCulling, new GUIContent("Distance Culling", "Check whether culling should take distance into consideration. Distance culling only happens when object is outside view."));
            
            EditorGUI.BeginDisabledGroup(script.distanceCulling == false);
                EditorGUILayout.PropertyField(distanceToCull, new GUIContent("Distance To Cull", "The distance if exceeded the object will always be culled."));
                
                EditorGUI.BeginDisabledGroup(script.distanceCullingOnly == true);
                    EditorGUILayout.PropertyField(prioritizeDistanceCulling, new GUIContent("Prioritize Distance Culling", "If distance exceeded the object will instantly be turned off and not wait to be out of view first."));
                EditorGUI.EndDisabledGroup ();
                
                EditorGUILayout.PropertyField(distanceCullingOnly, new GUIContent("Distance Culling Only", "Only distance culling will be applied no camera culling will occur."));
            EditorGUI.EndDisabledGroup ();


            serializedObject.ApplyModifiedProperties();
        }
    }
}
