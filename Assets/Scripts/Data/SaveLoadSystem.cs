using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveLoadSystem : MonoBehaviour
{
    private string _json;

    private Clothes _hatOne;
    private Clothes _hatTwo;

    private Clothes _femaleHair;
    private Clothes _maleHair;

    private Clothes _femaleUnderwear;
    private Clothes _maleUnderwear;

    private Clothes _outfitOne;
    private Clothes _outfitTwo;

    private string[] filePaths;

    private void Awake()
    {
        _hatOne = new Clothes();
        _hatTwo = new Clothes();
        _femaleHair = new Clothes();
        _maleHair = new Clothes();
        _femaleUnderwear = new Clothes();
        _maleUnderwear = new Clothes();
        _outfitOne = new Clothes();
        _outfitTwo = new Clothes();

        filePaths = Directory.GetFiles(Application.persistentDataPath + "/", "*.json");
        if (filePaths.Length == 0)
            CreateData();

        LoadData();
    }

    private void CreateData()
    {
        _hatOne.ID = "HatOne";
        _hatOne.FilePath = Application.persistentDataPath + "/HatOneData.json";
        _hatOne.Price = 10.00d;
        _hatOne.Equiped = false;
        _hatOne.Purchased = false;
        SaveData(_hatOne.FilePath, _hatOne);

        _hatTwo.ID = "HatTwo";
        _hatTwo.FilePath = Application.persistentDataPath + "/HatTwoData.json";
        _hatTwo.Price = 12.00d;
        _hatTwo.Equiped = false;
        _hatTwo.Purchased = false;
        SaveData(_hatTwo.FilePath, _hatTwo);

        _femaleHair.ID = "FemaleHair";
        _femaleHair.FilePath = Application.persistentDataPath + "/FemaleHairData.json";
        _femaleHair.Price = 15.00d;
        _femaleHair.Equiped = false;
        _femaleHair.Purchased = false;
        SaveData(_femaleHair.FilePath, _femaleHair);

        _maleHair.ID = "MaleHair";
        _maleHair.FilePath = Application.persistentDataPath + "/MaleHairData.json";
        _maleHair.Price = 15.00d;
        _maleHair.Equiped = false;
        _maleHair.Purchased = false;
        SaveData(_maleHair.FilePath, _maleHair);

        _femaleUnderwear.ID = "FemaleUnderwear";
        _femaleUnderwear.FilePath = Application.persistentDataPath + "/FemaleUnderwearData.json";
        _femaleUnderwear.Price = 10.00d;
        _femaleUnderwear.Equiped = false;
        _femaleUnderwear.Purchased = false;
        SaveData(_femaleUnderwear.FilePath, _femaleUnderwear);

        _maleUnderwear.ID = "MaleUnderwear";
        _maleUnderwear.FilePath = Application.persistentDataPath + "/MaleUnderwearData.json";
        _maleUnderwear.Price = 8.00d;
        _maleUnderwear.Equiped = false;
        _maleUnderwear.Purchased = false;
        SaveData(_maleUnderwear.FilePath, _maleUnderwear);

        _outfitOne.ID = "OutfitOne";
        _outfitOne.FilePath = Application.persistentDataPath + "/OutfitOneData.json";
        _outfitOne.Price = 30.00d;
        _outfitOne.Equiped = false;
        _outfitOne.Purchased = false;
        SaveData(_outfitOne.FilePath, _outfitOne);

        _outfitTwo.ID = "OutfitTwo";
        _outfitTwo.FilePath = Application.persistentDataPath + "/OutfitTwoData.json";
        _outfitTwo.Price = 28.00d;
        _outfitTwo.Equiped = false;
        _outfitTwo.Purchased = false;
        SaveData(_outfitTwo.FilePath, _outfitTwo);
    }

    private void Update()
    {
        // Temporary - Debug:
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            UpdateData(_outfitTwo.FilePath, _outfitTwo, true, true);
    }

    private void UpdateData(string filePath, Clothes clothes, bool purchased, bool equiped)
    {
        clothes.Purchased = purchased;
        clothes.Equiped = equiped;
        SaveData(filePath, clothes);
    }

    private void SaveData(string filepath, Clothes clothes)
    {
        _json = JsonUtility.ToJson(clothes);
        JsonUtility.FromJsonOverwrite(_json, clothes);
        File.WriteAllText(filepath, _json);
    }

    private void LoadData()
    {
        _json = File.ReadAllText(filePaths[0]);
        _hatOne = JsonUtility.FromJson<Clothes>(_json);

        _json = File.ReadAllText(filePaths[1]);
        _hatTwo = JsonUtility.FromJson<Clothes>(_json);

        _json = File.ReadAllText(filePaths[2]);
        _femaleHair = JsonUtility.FromJson<Clothes>(_json);

        _json = File.ReadAllText(filePaths[3]);
        _maleHair = JsonUtility.FromJson<Clothes>(_json);

        _json = File.ReadAllText(filePaths[4]);
        _femaleUnderwear = JsonUtility.FromJson<Clothes>(_json);

        _json = File.ReadAllText(filePaths[5]);
        _maleUnderwear = JsonUtility.FromJson<Clothes>(_json);

        _json = File.ReadAllText(filePaths[6]);
        _outfitOne = JsonUtility.FromJson<Clothes>(_json);

        _json = File.ReadAllText(filePaths[7]);
        _outfitTwo = JsonUtility.FromJson<Clothes>(_json);
    }
}
