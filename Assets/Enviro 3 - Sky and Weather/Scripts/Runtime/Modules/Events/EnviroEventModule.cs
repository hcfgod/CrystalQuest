using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using System;

namespace Enviro
{
    [Serializable]
	public class EnviroActionEvent : UnityEngine.Events.UnityEvent
	{
  
	}

    [Serializable]
    public class EnviroEvents 
    {
        public EnviroActionEvent onHourPassedActions = new EnviroActionEvent();
        public EnviroActionEvent onDayPassedActions = new EnviroActionEvent();
        public EnviroActionEvent onYearPassedActions = new EnviroActionEvent();
        public EnviroActionEvent onWeatherChangedActions = new EnviroActionEvent();
        public EnviroActionEvent onSeasonChangedActions = new EnviroActionEvent();
        public EnviroActionEvent onNightActions = new EnviroActionEvent();
        public EnviroActionEvent onDayActions = new EnviroActionEvent();
        public EnviroActionEvent onZoneChangedActions = new EnviroActionEvent();
    }  

    [Serializable]
    public class EnviroEventModule : EnviroModule 
    {   
        public Enviro.EnviroEvents Settings;
        public EnviroEventModule preset;
        public bool showEventsControls;

        public override void Enable () 
        { 
            if(EnviroManager.instance == null)
               return;

            if(EnviroManager.instance.Time != null)
            {
                EnviroManager.instance.OnHourPassed += () => HourPassed ();
                EnviroManager.instance.OnDayPassed += () => DayPassed ();
                EnviroManager.instance.OnYearPassed += () => YearPassed ();

                EnviroManager.instance.OnNightTime += () => NightTime ();
                EnviroManager.instance.OnDayTime += () => DayTime ();
            }
 
            if(EnviroManager.instance.Weather != null)
            { 
               EnviroManager.instance.OnWeatherChanged += (EnviroWeatherType type) =>  WeatherChanged ();
            }

            if(EnviroManager.instance.Environment != null)
            {
               EnviroManager.instance.OnSeasonChanged += (EnviroEnvironment.Seasons season) => SeasonsChanged ();
            } 
        }

        public override void Disable ()
        { 
  
        }

        public override void UpdateModule ()
        { 
  
        }

        //Event Invoke
        private void HourPassed()
        {
            Settings.onHourPassedActions.Invoke();
        }

        private void DayPassed()
        {
            Settings.onDayPassedActions.Invoke();
        }
            
        private void YearPassed()
        {
            Settings.onYearPassedActions.Invoke();
        }

        private void WeatherChanged()
        {
            Settings.onWeatherChangedActions.Invoke();
        }

        private void SeasonsChanged()
        {
            Settings.onSeasonChangedActions.Invoke();
        }

        private void NightTime()
        {
            Settings.onNightActions.Invoke ();
        }

        private void DayTime()
        {
            Settings.onDayActions.Invoke ();
        }

        private void ZoneChanged()
        {
            Settings.onZoneChangedActions.Invoke ();
        }

        //Save and Load
        public void LoadModuleValues ()
        {
            if(preset != null)
            {
                Settings = JsonUtility.FromJson<Enviro.EnviroEvents>(JsonUtility.ToJson(preset.Settings));
            }
            else
            {
                Debug.Log("Please assign a saved module to load from!");
            }
        } 

         public void SaveModuleValues ()
        {
#if UNITY_EDITOR
        EnviroEventModule t =  ScriptableObject.CreateInstance<EnviroEventModule>();
        t.name = "Event Preset";
        t.Settings = JsonUtility.FromJson<Enviro.EnviroEvents>(JsonUtility.ToJson(Settings));
 
        string assetPathAndName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(EnviroHelper.assetPath + "/New " + t.name + ".asset");
        UnityEditor.AssetDatabase.CreateAsset(t, assetPathAndName);
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
#endif
        }
        public void SaveModuleValues (EnviroEventModule module)
        {
            module.Settings = JsonUtility.FromJson<Enviro.EnviroEvents>(JsonUtility.ToJson(Settings));

            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(module);
            UnityEditor.AssetDatabase.SaveAssets();
            #endif
        }
    }
}