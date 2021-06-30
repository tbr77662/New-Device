using UnityEngine;
using System.Xml;
using System;
using System.IO;

public class SaveXml : MonoBehaviour
{
    private string path;
    private Save save1;
    // Start is called before the first frame update
    void Start()
    {
        path = Application.dataPath + "/DataXML.xml";
        save1 = new Save();
        if (File.Exists(path))
        {
            //LoadByXml();
        }
        else
        {
            StreamWriter sw;
            FileInfo t = new FileInfo(path);
            sw = t.CreateText();
            sw.Close();
            sw.Dispose();
            Debug.Log("Creat");
        }
    }

    void Update()
    {
        //SaveByXml();
    }

    public void SaveByXml()
    {
        XmlDocument xml = new XmlDocument();

        #region CreatXML elements

        XmlElement root = xml.CreateElement("Save");
        root.SetAttribute("FileName", "wiiFitdata");

        XmlElement wiiFitAllwei = xml.CreateElement("wiiFitAllwei");
        wiiFitAllwei.InnerText = save1.allwei.ToString();
        root.AppendChild(wiiFitAllwei);

        XmlElement wiiFitRT = xml.CreateElement("wiiFitRT");
        wiiFitRT.InnerText = save1.RT.ToString();
        root.AppendChild(wiiFitRT);

        XmlElement wiiFitRB = xml.CreateElement("wiiFitRB");
        wiiFitRB.InnerText = save1.RB.ToString();
        root.AppendChild(wiiFitRB);

        XmlElement wiiFitLT = xml.CreateElement("wiiFitLT");
        wiiFitLT.InnerText = save1.LT.ToString();
        root.AppendChild(wiiFitLT);

        XmlElement wiiFitLB = xml.CreateElement("wiiFitLB");
        wiiFitLB.InnerText = save1.LB.ToString();
        root.AppendChild(wiiFitLB);

        XmlElement wiiFitX_Pos = xml.CreateElement("wiiFitX_Pos");
        wiiFitX_Pos.InnerText = save1.X_Pos.ToString();
        root.AppendChild(wiiFitX_Pos);

        XmlElement wiiFitY_Pos = xml.CreateElement("wiiFitY_Pos");
        wiiFitY_Pos.InnerText = save1.Y_Pos.ToString();
        root.AppendChild(wiiFitY_Pos);
        #endregion

        xml.AppendChild(root);

        xml.Save(path);
        if(File.Exists(path))
        {
            Debug.Log("XML FILE SAVED");
        }
    }

    public void LoadByXml()
    {
        if (File.Exists(path))
        {
            Save save = new Save();
            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.Load(path);

            XmlNodeList wiiFitAllwei = xmlDocument.GetElementsByTagName("wiiFitAllwei");
            float allwei = float.Parse(wiiFitAllwei[0].InnerText);
            save.allwei = allwei;

            XmlNodeList wiiFitRT = xmlDocument.GetElementsByTagName("wiiFitRT");
            float rt = float.Parse(wiiFitRT[0].InnerText);
            save.RT = rt;

            XmlNodeList wiiFitRB = xmlDocument.GetElementsByTagName("wiiFitRB");
            float rb = float.Parse(wiiFitRB[0].InnerText);
            save.RB = rb;

            XmlNodeList wiiFitLT = xmlDocument.GetElementsByTagName("wiiFitLT");
            float lt = float.Parse(wiiFitLT[0].InnerText);
            save.LT = lt;

            XmlNodeList wiiFitLB = xmlDocument.GetElementsByTagName("wiiFitLB");
            float lb = float.Parse(wiiFitLB[0].InnerText);
            save.LB = lb;

            XmlNodeList wiiFitX_Pos = xmlDocument.GetElementsByTagName("wiiFitX_Pos");
            float xp = float.Parse(wiiFitX_Pos[0].InnerText);
            save.X_Pos = xp;

            XmlNodeList wiiFitY_Pos = xmlDocument.GetElementsByTagName("wiiFitY_Pos");
            float yp = float.Parse(wiiFitY_Pos[0].InnerText);
            save.Y_Pos = yp;

            Debug.Log("Load XML");

            AdolfSensorLink.Instance.allwei = save.allwei;
            AdolfSensorLink.Instance.RB = save.RB;
            AdolfSensorLink.Instance.RT = save.RT;
            AdolfSensorLink.Instance.LB = save.LB;
            AdolfSensorLink.Instance.LT = save.LT;
            AdolfSensorLink.Instance.X_POS = save.X_Pos;
            AdolfSensorLink.Instance.Y_POS = save.Y_Pos;
        }
        else 
        {
            Debug.LogError("NOT FILE");
        }
    }

    public void SaveJson()
    {
        string json = JsonUtility.ToJson(save1);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("關閉遊戲");
    }
}

[Serializable]
public class Save
{
    public float RT;
    public float RB;
    public float LT;
    public float LB;
    public float allwei;
    public float X_Pos;
    public float Y_Pos;
}
