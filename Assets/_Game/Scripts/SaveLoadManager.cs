using Lightbug.CharacterControllerPro.Core;
using SaveSystem;
using SaveSystem.Serialization.Formatters.Json;
using SmartPoint;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public CheckPointController checkPointController;
    private string saveFileName = "allCheckpoints";

    private void Start()
    {
        SaveGameSettings saveGameSettings = new SaveGameSettings();
        saveGameSettings.Encrypt = false;
        saveGameSettings.Formatter = new JsonFormatter();
        SaveGame.DefaultSettings = saveGameSettings;

        LoadAllCheckpoints();
    }

    private void OnApplicationQuit()
    {
        SaveCheckpointData();
    }

    public void SaveCheckpointData()
    {
        List<CheckpointData> checkpointDataList = new List<CheckpointData>();

        foreach (CheckPoint cp in checkPointController.checkPoints)
        {
            CheckpointData data = new CheckpointData(
                cp.GetActive(),
                cp.GetPosition(),
                cp.GetBounds(),
                cp.GetDirection()
            );

            checkpointDataList.Add(data);
        }

        SaveGame.Save(saveFileName, checkpointDataList);
    }
      
    public void LoadAllCheckpoints()
    {
        if (SaveGame.Exists(saveFileName))
        {
            List<CheckpointData> loadedData = SaveGame.Load<List<CheckpointData>>(saveFileName);

            for (int i = 0; i < loadedData.Count; i++)
            {
                CheckPoint cp = checkPointController.checkPoints[i];
                CheckpointData data = loadedData[i];

                cp.SetActive(data.isActive);
                cp.SetPosition(data.position);
                cp.SetBounds(data.bounds);
                cp.SetDirection(data.direction);
            }

	        CharacterActor charactorActor = FindObjectOfType<CharacterActor>();
	        CheckPoint latestCheckpoint = checkPointController.GetLatestCheckpoint();
	        
	        if(latestCheckpoint == null)
	        {
	        	return;
	        }
	        
	        charactorActor.Teleport(latestCheckpoint.GetAbsolutePosition());
        }
    }
}
