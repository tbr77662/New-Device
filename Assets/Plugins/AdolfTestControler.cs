using UnityEngine;
using System.Collections;
using AdolfTech.ForcePlate;

public class AdolfTestControler : MonoBehaviour {

    public static AdolfTestControler Instance;

    public enum diveType { device1, device2};
    public diveType device;

    ForcePlate ForcePlate = null;
    public float X_POS, Y_POS;
    public float RT, LT, RB, LB;
    public float allwei; //體重

    public float wei_R; //右邊壓力
    public float wei_L; //左邊壓力
    public float wei_T; //前方壓力
    public float wei_B; //後方壓力
    public float wei_RS, wei_LS, wei_TS, wei_BS; //4個方向的直方圖比重

    public int countStyle; //連線狀況
    public int countNum;

    // Use this for initialization
    void Start () {
        Instance = this; //指定Instance這個程式   

        switch (device)
        {
            case diveType.device1:
                countNum = 0;
                break;
            case diveType.device2:
                countNum = 1;
                break;
        }

        OnLink();
        //OnLink(countNum);
    }

    //連線
    public void OnLink(int diveNum)
    {
        /* Create a ForcePlate */
        ForcePlate = new ForcePlate(diveNum);
        ForcePlate.Convert(-1);
        InvokeRepeating("UpdateForceData", 0, 0.01f);
    }

    public void OnLink()
    {
        /* Create a ForcePlate */
        ForcePlate = new ForcePlate();
        ForcePlate.Convert(-1);
        InvokeRepeating("UpdateForceData", 0, 0.01f);
    }

    //每秒呼叫一次
    void UpdateForceData()
    {
        ForcePlate.UpdateForceData();

        X_POS = ForcePlate.LR_RATIO;
        Y_POS = ForcePlate.AP_RATIO;
        LB = ForcePlate.LB;
        RT = ForcePlate.RT;
        RB = ForcePlate.RB;
        LT = ForcePlate.LT;

        allwei = ForcePlate.SUM;

        wei_L = (LB + LT) / allwei * 100;
        wei_R = (RT + RB) / allwei * 100;
        wei_T = (RT + LT) / allwei * 100;
        wei_B = (RB + LB) / allwei * 100;

        wei_LS = ((LB + LT) / allwei * 100) * 0.72f;
        wei_RS = ((RB + RT) / allwei * 100) * 0.72f;
        wei_TS = ((RT + LT) / allwei * 100) * 0.72f;
        wei_BS = ((LB + RB) / allwei * 100) * 0.72f;

        countStyle = 1;
    }

	// Update is called once per frame
	void Update () {

    }

    private void OnDestroy()
    {
        if (ForcePlate != null)
        {
            ForcePlate.Convert(0);
            ForcePlate.Close();
        }
    }
}
