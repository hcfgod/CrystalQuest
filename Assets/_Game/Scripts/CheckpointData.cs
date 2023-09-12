using UnityEngine;

[System.Serializable]
public class CheckpointData
{
    public bool isActive;
    public Vector3 position;
    public Bounds bounds;
    public float direction;

    public CheckpointData(bool isActive, Vector3 position, Bounds bounds, float direction)
    {
        this.isActive = isActive;
        this.position = position;
        this.bounds = bounds;
        this.direction = direction;
    }
}
