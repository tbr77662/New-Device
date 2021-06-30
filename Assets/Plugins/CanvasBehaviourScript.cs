using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AdolfTech.ForcePlate;

public class CanvasBehaviourScript : MonoBehaviour
{
    // Use this for initialization
    Text LT_Text;
    Text RT_Text;
    Text LB_Text;
    Text RB_Text;
    Text H_RATIO_Text;
    Text V_RATIO_Text;
    Image xImage;
    ForcePlate ForcePlate = null;
    float X_POS, Y_POS;

    void Start()
    {
        /* Get reference of Unity Text */
        LT_Text = GameObject.Find("Text_LT").GetComponent<Text>();
        RT_Text = GameObject.Find("Text_RT").GetComponent<Text>();
        LB_Text = GameObject.Find("Text_LB").GetComponent<Text>();
        RB_Text = GameObject.Find("Text_RB").GetComponent<Text>();
        V_RATIO_Text = GameObject.Find("Text_V").GetComponent<Text>();
        H_RATIO_Text = GameObject.Find("Text_H").GetComponent<Text>();

        /* Get reference of Unity Image */
        xImage = GameObject.Find("Image").GetComponent<Image>();

        /* Get reference of Unity Button & set onClick callback */
        List<string> btnNameList = new List<string>();
        btnNameList.Add("Button_Offset");
        btnNameList.Add("Button_BebingCal");
        btnNameList.Add("Button_CalPointA");
        btnNameList.Add("Button_CalPointB");
        btnNameList.Add("Button_EndCal");
        btnNameList.Add("Button_RecordStart");
        btnNameList.Add("Button_RecordStop");
        foreach (string str in btnNameList)
        {
            GameObject btnObj = GameObject.Find(str);
            Button btn = btnObj.GetComponent<Button>();
            btn.onClick.AddListener(delegate ()
            {
                this.OnClick(btnObj);
            });
        }

        /* Create a ForcePlate */
        ForcePlate = new ForcePlate(AdolfTestControler.Instance.countNum);

        /* Repeating updata force data @ 100Hz ( force plate sample rate * 2 ) */
        InvokeRepeating("UpdateForceData", 0, 0.01f);
    }
    
    void UpdateForceData()
    {
        ForcePlate.UpdateForceData();
    }

    void OnClick(GameObject sender)
    {
        switch (sender.name)
        {
            case "Button_Offset":
                ForcePlate.Offset();
                break;
            case "Button_BebingCal":
                ForcePlate.BeginCal();
                break;
            case "Button_CalPointA":
                ForcePlate.SetCalPoint(0.0f);
                break;
            case "Button_CalPointB":
                ForcePlate.SetCalPoint(3.0f);
                break;
            case "Button_EndCal":
                ForcePlate.EndCal();
                break;
            case "Button_RecordStart":
                ForcePlate.Convert(-1);
                ForcePlate.BeginRecod("rawData.csv");
                break;
            case "Button_RecordStop":
                ForcePlate.Convert(0);
                ForcePlate.EndRecod();
                break;
            default:
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ForcePlate == null)
            return;

        /* Update position of Image */
        //X_POS = ForcePlate.SUM > 5 ? (1 + ForcePlate.LR_RATIO) * Screen.width / 2 : Screen.width / 2;
        //Y_POS = ForcePlate.SUM > 5 ? (1 + ForcePlate.AP_RATIO) * Screen.height / 2 : Screen.height / 2;

        X_POS = ForcePlate.LR_RATIO;
        Y_POS = ForcePlate.AP_RATIO;

        xImage.transform.position = new Vector3(X_POS, Y_POS);

        /* Update text of Text */
        LT_Text.text = ForcePlate.LT.ToString("F2");
        RT_Text.text = ForcePlate.RT.ToString("F2");
        LB_Text.text = ForcePlate.LB.ToString("F2");
        RB_Text.text = ForcePlate.RB.ToString("F2");

        V_RATIO_Text.text = ForcePlate.AP_RATIO.ToString("F3");
        H_RATIO_Text.text = ForcePlate.LR_RATIO.ToString("F3");
    }

    void OnApplicationQuit()
    {
        if(ForcePlate != null)
            ForcePlate.EndRecod();
    }
}