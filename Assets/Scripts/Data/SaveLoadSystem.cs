using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveLoadSystem : MonoBehaviour
{
    private Stats _stats;
    private string json;

    void Start()
    {
        _stats = new Stats();
        _stats.number = 0;
        json = JsonUtility.ToJson(_stats);
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _stats.number++;
            SaveData();
        }
    }

    private void SaveData()
    {
        JsonUtility.FromJsonOverwrite(json, _stats);
        File.WriteAllText(Application.persistentDataPath + "/jsonTest.json", json);
    }

    private void LoadData()
    {
        _stats = JsonUtility.FromJson<Stats>(json);
    }
}
