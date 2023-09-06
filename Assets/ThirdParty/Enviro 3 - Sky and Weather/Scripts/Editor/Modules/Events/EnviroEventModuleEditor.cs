using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Enviro
{
    [CustomEditor(typeof(EnviroEventModule))]
    public class EnviroEventModuleEditor : EnviroModuleEditor
    {  
        private EnviroEventModule myTarget; 

        //Properties
        private SerializedProperty onHourPassedActions, onDayPassedActions, onYearPassedActions, onWeatherChangedActions, onSeasonChangedActions, onNightActions, onDayActions;


        //On Enable
        public override void OnEnable()
        {
            base.OnEnable();

            if(!target)
                return;

            myTarget = (EnviroEventModule)target;
            serializedObj = new SerializedObject(myTarget);
            preset = serializedObj.FindProperty("preset");
            onHourPassedActions = serializedObj.FindProperty("Settings.onHourPassedActions"); 
            onDayPassedActions = serializedObj.FindProperty("Settings.onDayPassedActions"); 
            onYearPassedActions = serializedObj.FindProperty("Settings.onYearPassedActions"); 
            onWeatherChangedActions = serializedObj.FindProperty("Settings.onWeatherChangedActions"); 
            onSeasonChangedActions = serializedObj.FindProperty("Settings.onSeasonChangedActions"); 
            onNightActions = serializedObj.FindProperty("Settings.onNightActions"); 
            onDayActions = serializedObj.FindProperty("Settings.onDayActions"); 
        } 

        public override void OnInspectorGUI()
        {
            if(!target)
                return;

            base.OnInspectorGUI();
            
            GUI.backgroundColor = baseModuleColor;
            GUILayout.BeginVertical("",boxStyleModified);
            GUI.backgroundColor = Color.white;
            EditorGUILayout.BeginHorizontal();
            myTarget.showModuleInspector = GUILayout.Toggle(myTarget.showModuleInspector, "Events", headerFoldout);
            
            GUILayout.FlexibleSpace();
            if(GUILayout.Button("x", EditorStyles.miniButtonRight,GUILayout.Width(18), GUILayout.Height(18)))
            {
                EnviroManager.instance.RemoveModule(EnviroManager.ModuleType.Events); //Add Remove
                DestroyImmediate(this);
                return;
            } 
            
            EditorGUILayout.EndHorizontal();
            
            if(myTarget.showModuleInspector)
            {
                //EditorGUILayout.LabelField("This module will control your.");
                serializedObj.UpdateIfRequiredOrScript ();
                EditorGUI.BeginChangeCheck();
                
                // Set Values
                GUI.backgroundColor = categoryModuleColor;
                GUILayout.BeginVertical("",boxStyleModified);
                GUI.backgroundColor = Color.white;
                myTarget.showEventsControls = GUILayout.Toggle(myTarget.showEventsControls, "Event Controls", headerFoldout);               
                if(myTarget.showEventsControls)
                {
                    
                    GUILayout.Space(10); 
                    EditorGUILayout.PropertyField(onHourPassedActions);    
                    EditorGUILayout.PropertyField(onDayPassedActions);   
                    EditorGUILayout.PropertyField(onYearPassedActions);  
                    GUILayout.Space(5);
                    EditorGUILayout.PropertyField(onWeatherChangedActions); 
                    EditorGUILayout.PropertyField(onSeasonChangedActions); 
                    GUILayout.Space(5); 
                    EditorGUILayout.PropertyField(onDayActions);
                    EditorGUILayout.PropertyField(onNightActions);
                }
                GUILayout.EndVertical();


                // Save Load
                GUI.backgroundColor = categoryModuleColor;
                GUILayout.BeginVertical("",boxStyleModified);
                GUI.backgroundColor = Color.white;
                myTarget.showSaveLoad = GUILayout.Toggle(myTarget.showSaveLoad, "Save/Load", headerFoldout);
                
                if(myTarget.showSaveLoad)
                {
                    EditorGUILayout.PropertyField(preset);

                    GUILayout.BeginHorizontal("",wrapStyle);

                    if(myTarget.preset != null)
                    {
                        if(GUILayout.Button("Load"))
                        {
                            myTarget.LoadModuleValues();
                        }
                        if(GUILayout.Button("Save"))
                        {
                            myTarget.SaveModuleValues(myTarget.preset);
                        }
                    }
                    if(GUILayout.Button("Save As New"))
                    {
                        myTarget.SaveModuleValues();
                    }

                    GUILayout.EndHorizontal();

     
                }
                GUILayout.EndVertical();
                /// Save Load End
                
                ApplyChanges ();
            }
            GUILayout.EndVertical();

            if(myTarget.showModuleInspector)
             GUILayout.Space(20);
        }
    }
}
