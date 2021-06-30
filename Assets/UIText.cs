using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{
    public Text RT;
    public Text LT;
    public Text RB;
    public Text LB;
    public Text Allwei;
    public Text X_Pos;
    public Text Y_Pos;

    private void Update()
    {
        RT.text = "RT: " + AdolfSensorLink.Instance.RT.ToString();
        LT.text = "LT: " + AdolfSensorLink.Instance.LT.ToString();
        RB.text = "RB: " + AdolfSensorLink.Instance.RB.ToString();
        LB.text = "LB: " + AdolfSensorLink.Instance.LB.ToString();
        Allwei.text = "Allwei: " + AdolfSensorLink.Instance.allwei.ToString();
        X_Pos.text = "X_Pos: " + AdolfSensorLink.Instance.X_POS.ToString();
        Y_Pos.text = "Y_Pos: " + AdolfSensorLink.Instance.Y_POS.ToString();
    }
}
