using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class SaveLoadSystem : MonoBehaviour
{
    private string _json;
    private string[] filePaths;

#if UNITY_EDITOR
    [ReadOnly] public Clothes HatOne;
    [ReadOnly] public Clothes HatTwo;

    [ReadOnly] public Clothes FemaleHair;
    [ReadOnly] public Clothes MaleHair;

    [ReadOnly] public Clothes FemaleUnderwear;
    [ReadOnly] public Clothes MaleUnderwear;

    [ReadOnly] public Clothes OutfitOne;
    [ReadOnly] public Clothes OutfitTwo;
#else
    public Clothes HatOne;
    public Clothes HatTwo;

    public Clothes FemaleHair;
    public Clothes MaleHair;

    public Clothes FemaleUnderwear;
    public Clothes MaleUnderwear;

    public Clothes OutfitOne;
    public Clothes OutfitTwo;
#endif

    private void Awake()
    {
        HatOne = new Clothes();
        HatTwo = new Clothes();
        FemaleHair = new Clothes();
        MaleHair = new Clothes();
        FemaleUnderwear = new Clothes();
        MaleUnderwear = new Clothes();
        OutfitOne = new Clothes();
        OutfitTwo = new Clothes();

        filePaths = Directory.GetFiles(Application.persistentDataPath + "/", "*.json");
        if (filePaths.Length == 0)
            CreateData();

        LoadData();
    }

    private void CreateData()
    {
        HatOne.ID = "HatOne";
        HatOne.FilePath = Application.persistentDataPath + "/HatOneData.json";
        HatOne.Price = 10.00d;
        HatOne.Equiped = false;
        HatOne.Purchased = false;
        SaveData(HatOne.FilePath, HatOne);

        HatTwo.ID = "HatTwo";
        HatTwo.FilePath = Application.persistentDataPath + "/HatTwoData.json";
        HatTwo.Price = 12.00d;
        HatTwo.Equiped = false;
        HatTwo.Purchased = false;
        SaveData(HatTwo.FilePath, HatTwo);

        FemaleHair.ID = "FemaleHair";
        FemaleHair.FilePath = Application.persistentDataPath + "/FemaleHairData.json";
        FemaleHair.Price = 15.00d;
        FemaleHair.Equiped = false;
        FemaleHair.Purchased = false;
        SaveData(FemaleHair.FilePath, FemaleHair);

        MaleHair.ID = "MaleHair";
        MaleHair.FilePath = Application.persistentDataPath + "/MaleHairData.json";
        MaleHair.Price = 15.00d;
        MaleHair.Equiped = false;
        MaleHair.Purchased = false;
        SaveData(MaleHair.FilePath, MaleHair);

        FemaleUnderwear.ID = "FemaleUnderwear";
        FemaleUnderwear.FilePath = Application.persistentDataPath + "/FemaleUnderwearData.json";
        FemaleUnderwear.Price = 10.00d;
        FemaleUnderwear.Equiped = false;
        FemaleUnderwear.Purchased = false;
        SaveData(FemaleUnderwear.FilePath, FemaleUnderwear);

        MaleUnderwear.ID = "MaleUnderwear";
        MaleUnderwear.FilePath = Application.persistentDataPath + "/MaleUnderwearData.json";
        MaleUnderwear.Price = 8.00d;
        MaleUnderwear.Equiped = false;
        MaleUnderwear.Purchased = false;
        SaveData(MaleUnderwear.FilePath, MaleUnderwear);

        OutfitOne.ID = "OutfitOne";
        OutfitOne.FilePath = Application.persistentDataPath + "/OutfitOneData.json";
        OutfitOne.Price = 30.00d;
        OutfitOne.Equiped = false;
        OutfitOne.Purchased = false;
        SaveData(OutfitOne.FilePath, OutfitOne);

        OutfitTwo.ID = "OutfitTwo";
        OutfitTwo.FilePath = Application.persistentDataPath + "/OutfitTwoData.json";
        OutfitTwo.Price = 28.00d;
        OutfitTwo.Equiped = false;
        OutfitTwo.Purchased = false;
        SaveData(OutfitTwo.FilePath, OutfitTwo);
    }

    private void Update()
    {
        // Temporary - Debug:
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
            UpdateData(OutfitTwo.FilePath, OutfitTwo, true, true);
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
        HatOne = JsonUtility.FromJson<Clothes>(_json);

        _json = File.ReadAllText(filePaths[1]);
        HatTwo = JsonUtility.FromJson<Clothes>(_json);

        _json = File.ReadAllText(filePaths[2]);
        FemaleHair = JsonUtility.FromJson<Clothes>(_json);

        _json = File.ReadAllText(filePaths[3]);
        MaleHair = JsonUtility.FromJson<Clothes>(_json);

        _json = File.ReadAllText(filePaths[4]);
        FemaleUnderwear = JsonUtility.FromJson<Clothes>(_json);

        _json = File.ReadAllText(filePaths[5]);
        MaleUnderwear = JsonUtility.FromJson<Clothes>(_json);

        _json = File.ReadAllText(filePaths[6]);
        OutfitOne = JsonUtility.FromJson<Clothes>(_json);

        _json = File.ReadAllText(filePaths[7]);
        OutfitTwo = JsonUtility.FromJson<Clothes>(_json);
    }
}
