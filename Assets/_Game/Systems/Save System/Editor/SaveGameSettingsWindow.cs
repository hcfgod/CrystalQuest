using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

using SaveSystem.Utilities;

namespace SaveSystem.Editor
{

    /// <summary>
    /// Save Game Settings Window.
    /// </summary>
    public class SaveGameSettingsWindow : EditorWindow
    {

        protected Vector2 scrollPosition;
        protected int selectedTab;
        protected string[] tabs = new string[] {
            "General",
        };
        [MenuItem("Window/Save System/Settings")]
        public static void Initialize()
        {
            SaveGameSettingsWindow window = EditorWindow.GetWindow<SaveGameSettingsWindow>();
            window.minSize = new Vector2(400f, 100f);
            window.Show();
        }

        protected virtual void OnEnable()
        {
            
        }

        protected virtual void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            this.selectedTab = GUILayout.Toolbar(this.selectedTab, this.tabs, EditorStyles.toolbarButton);
            EditorGUILayout.EndHorizontal();
            this.scrollPosition = EditorGUILayout.BeginScrollView(this.scrollPosition);
            EditorGUILayout.BeginVertical();
            switch (this.selectedTab)
            {
                case 0:
                    GeneralTab();
                    break;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Made by Morgan Interactive", EditorStyles.centeredGreyMiniLabel);
        }

        protected virtual void GeneralTab()
        {
            GUILayout.Label("Actions", EditorStyles.boldLabel);
            GUILayout.Label("The below are some general actions you can apply and manage the data on this device", EditorStyles.wordWrappedLabel);
            EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
            if (GUILayout.Button("Clear Saved Data", GUILayout.Width(150f)))
            {
                bool clear = EditorUtility.DisplayDialog(
                    "Clearing Saved Data",
                    "Are you sure you want to clear all saved data on this device?",
                    "Yes", "No");
                if (clear)
                {
                    SaveGame.Clear();
                }
            }
            if (GUILayout.Button("Save Random Data", GUILayout.Width(150f)))
            {
                string randomIdentifier = StringUtils.RandomString(8) + ".txt";
                SaveGame.Save(randomIdentifier, "This is random data");
                EditorUtility.DisplayDialog(
                    "Saving Random Data",
                    string.Format("Random Data genereated and saved successfully with the below information:\n- Identifier: {0}", randomIdentifier),
                    "Done");
            }
           
            EditorGUILayout.EndVertical();
            GUILayout.Label("Information", EditorStyles.boldLabel);
            GUILayout.Label("The below are informational and readonly fields for copy/paste and such porpuses.", EditorStyles.wordWrappedLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("Persistent Data Path", Application.persistentDataPath);
            if (GUILayout.Button("Copy", GUILayout.Width(80f)))
            {
                EditorGUIUtility.systemCopyBuffer = Application.persistentDataPath;
            }
            if (GUILayout.Button("Open", GUILayout.Width(80f)))
            {
                EditorUtility.RevealInFinder(Application.persistentDataPath);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.TextField("Data Path", Application.dataPath);
            if (GUILayout.Button("Copy", GUILayout.Width(80f)))
            {
                EditorGUIUtility.systemCopyBuffer = Application.dataPath;
            }
            if (GUILayout.Button("Open", GUILayout.Width(80f)))
            {
                EditorUtility.RevealInFinder(Application.dataPath);
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }

}