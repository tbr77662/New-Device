using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using AdolfTech.ForcePlate;

public class AdolfSensorLink : MonoBehaviour {

    public static AdolfSensorLink Instance; // 設定Instance，讓其他程式能讀取GameFunction裡的東西
    
    public InputField inputField;

    ForcePlate ForcePlate = null;
    public float X_POS, Y_POS;
    public float RT, LT, RB, LB;
    public float allwei; //體重

    public static bool alreadyLink = false;

	// Use this for initialization
	void Start () {
        Instance = this; //指定Instance這個程式
        //OnLink();
	}
	
	// Update is called once per frame
	void Update () {

	}

    //每秒呼叫一次
    void UpdateForceData()
    {
        ForcePlate.UpdateForceData();

        if (ForcePlate == null)
            return;

        X_POS = ForcePlate.LR_RATIO;
        Y_POS = ForcePlate.AP_RATIO;
        LB = ForcePlate.LB;
        RT = ForcePlate.RT;
        RB = ForcePlate.RB;
        LT = ForcePlate.LT;

        allwei = ForcePlate.SUM;
    }

    //連線
    public void OnLink() {
        /* Create a ForcePlate */
        Debug.Log("LINK");
        ForcePlate = new ForcePlate();
        if (alreadyLink == false)
        {
            ForcePlate.Offset(); //第一次連線，所以必須歸零
            alreadyLink = true;
        }
        ForcePlate.Convert(-1);
        InvokeRepeating("UpdateForceData", 0, 0.01f);
    }

    //開始校正
    public void OnBegin()
    {
        ForcePlate.BeginCal();
    }

    //開始歸零校正
    public void OnReset()
    {
        ForcePlate.SetCalPoint(0.0f);
    }

    //重量校正
    public void OnSetPoint()
    {
        //ForcePlate.SetCalPoint(ForcePlate.SUM); //可惜人體重誤差頗大，否則可用自體重量校正
        //ForcePlate.SetCalPoint(2.0f); //2公斤校正
        //ForcePlate.SetCalPoint(2.26795f); //5磅校正
        //ForcePlate.SetCalPoint(3.62873896f); //8磅校正
        float tmpAllwei = float.Parse(inputField.text);
        ForcePlate.SetCalPoint(tmpAllwei); //自訂義公斤數
    }

    //結束校正
    public void OnEnd()
    {
        ForcePlate.EndCal();
    }

    //關閉連線
    public void OnClose()
    {
        ForcePlate.Convert(0);
    }

    public void OnBeginZero()
    {
        OnBegin();
        OnReset();
        OnSetPoint();
        OnEnd();
    }
}
