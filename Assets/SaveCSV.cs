using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveCSV : MonoBehaviour
{
    private Save save;
    string fileName = "";
    int count = 0;

    bool saveData = false;
    public GameObject green;
    public GameObject red;

    public SaveList saveList;
    // Start is called before the first frame update
    void Start()
    {
        fileName = Application.dataPath + "/data" + count.ToString() + ".csv";
        saveList = new SaveList();

    }

    private void FixedUpdate()
    {
        InitSave();

        if (saveData)
        {
            saveList.saves.Add(save);
            WriteCSV();
            green.SetActive(!saveData);
            red.SetActive(saveData);
        }
        else
        {
            green.SetActive(!saveData);
            red.SetActive(saveData);
        }
    }

    private void InitSave()
    {
        save = new Save();

        save.allwei = AdolfSensorLink.Instance.allwei;
        save.RB = AdolfSensorLink.Instance.RB;
        save.RT = AdolfSensorLink.Instance.RT;
        save.LB = AdolfSensorLink.Instance.LB;
        save.LT = AdolfSensorLink.Instance.LT;
        save.X_Pos = AdolfSensorLink.Instance.X_POS;
        save.Y_Pos = AdolfSensorLink.Instance.Y_POS;

    }

    public void WriteCSV()
    {
        TextWriter tw = new StreamWriter(fileName, false);//false 創建新文件,如果存在原文件，則覆蓋
        tw.WriteLine("allwei, RB, RT, LB, LT, X_Pos, Y_Pos");
        tw.Close();

        tw = new StreamWriter(fileName, true);//true 打開文件保留原來數據

        for (int i = 0; i < saveList.saves.Count; i++)
        {
            tw.WriteLine($"{saveList.saves[i].allwei},{saveList.saves[i].RB},{saveList.saves[i].RT}," +
                $"{saveList.saves[i].LB},{saveList.saves[i].LT},{saveList.saves[i].X_Pos},{saveList.saves[i].Y_Pos}");
        }
        tw.Close();
    }

    private void CreatSaveData()
    {
        if (File.Exists(fileName))
        {
            count++;
            fileName = Application.dataPath + "/data" + count.ToString() + ".csv";
            CreatSaveData();
        }
    }

    public void OnSaveCsv()
    {
        CreatSaveData();
        saveList.saves.Clear();
        saveData = true;
    }

    public void OnCloseSave()
    {
        saveData = false;
    }

    public void OnQuitGame()
    {
        Application.Quit();
    }
}

[System.Serializable]
public class SaveList
{
    public List<Save> saves;

    public SaveList()
    {
        saves = new List<Save>();
    }
}
