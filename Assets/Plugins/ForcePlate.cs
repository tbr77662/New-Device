using System;
using System.IO.Ports;
using System.IO;
using UnityEngine;

namespace AdolfTech.ForcePlate
{
    public class ForcePlate
    {
        private static object Device = null;
        private object xForceDataObj = null;
        private StreamWriter streamWriter = null;
        private string filePath = string.Empty; 

        /// <summary>
        /// Weight of left-top loadcell
        /// </summary>
        public float LT { get; private set; }

        /// <summary>
        /// Weight of right-top loadcell
        /// </summary>
        public float LB { get; private set; }

        /// <summary>
        /// Weight of left-bottom loadcell
        /// </summary>
        public float RT { get; private set; }

        /// <summary>
        /// Weight of right-bottom loadcell
        /// </summary>
        public float RB { get; private set; }

        /// <summary>
        /// Total Weight of all loadcell
        /// </summary>
        public float SUM { get { return LT + RT + LB + RB; } }

        /// <summary>
        /// Ratio of forward/backward 
        /// </summary>
        public float AP_RATIO
        {
            get
            {
                if (SUM == 0)
                    return 0;
                else
                    return ((LB + RB) - (LT + RT)) / SUM;  //前後對調  ---20161108-fair
            }
        }

        /// <summary>
        /// Ration of Left/Right
        /// </summary>
        public float LR_RATIO
        {
            get
            {
                if (SUM == 0)
                    return 0;
                else
                    return ((RT + RB) - (LT + LB)) / SUM;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ForcePlate(int deviceNum)
        {
            string[] portName = SerialPort.GetPortNames();
            Debug.Log(string.Format("portName[{0}]:{1}", deviceNum, portName[deviceNum]));

            LT = RT = LB = RB = 0;

            if (Device != null)
                return;

            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    Debug.Log("platform:Android");
                    filePath = Application.persistentDataPath + @"/"; //指定android 路徑
                    Device = new AndroidJavaObject("com.lib.unityjavabind.AndroidForcePlate", 115200, 8, "None", 1f);
                    (Device as AndroidJavaObject).Call("connect");
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    filePath = @".\";
                    Device = new WinForcePlate(portName[deviceNum], 115200, 8, Parity.None, StopBits.One);
                    (Device as WinForcePlate).connet();
                    break;
                default:
                    break;
            }
        }

        //
        public ForcePlate()
        {

            LT = RT = LB = RB = 0;

            if (Device != null)
                return;

            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    Debug.Log("platform:Android");
                    filePath = Application.persistentDataPath + @"/"; //指定android 路徑
                    Device = new AndroidJavaObject("com.lib.unityjavabind.AndroidForcePlate", 115200, 8, "None", 1f);
                    (Device as AndroidJavaObject).Call("connect");
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    filePath = @".\";
                    Device = new WinForcePlate(115200, 8, Parity.None, StopBits.One);
                    (Device as WinForcePlate).connet();
                    break;
                default:
                    break;
            }
        }
        //
        public void Close()
        {
            if (Device == null)
                return;

            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    (Device as AndroidJavaObject).Call("disconnect");
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    (Device as WinForcePlate).disconnect();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public void BeginRecod(string fileName)
        {
            EndRecod();
            streamWriter = new StreamWriter(filePath + fileName);
            streamWriter.AutoFlush = true;
            streamWriter.WriteLine(@"LT,RT,LB,RB,A/P Ratio,L/R Ratio");
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndRecod()
        {
            if (streamWriter != null) {
                streamWriter.Flush();
                streamWriter.Close();
                streamWriter = null;
            }
        }

        /// <summary>
        /// Excute this function periodly.
        /// </summary>
        public void UpdateForceData()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    xForceDataObj = (Device as AndroidJavaObject).Call<AndroidJavaObject>("getForceData");
                    if (xForceDataObj != null)
                    {
                        LT = (xForceDataObj as AndroidJavaObject).Get<float>("leftTopValue");
                        RT = (xForceDataObj as AndroidJavaObject).Get<float>("rightTopValue");
                        LB = (xForceDataObj as AndroidJavaObject).Get<float>("leftBottomValue");
                        RB = (xForceDataObj as AndroidJavaObject).Get<float>("rightBottomValue");
                        UpdateRecord();
                    }
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    xForceDataObj = (Device as WinForcePlate).getForceData();
                    if (xForceDataObj != null)
                    {
                        LT = (xForceDataObj as ForceData).leftTopValue;
                        RT = (xForceDataObj as ForceData).rightTopValue;
                        LB = (xForceDataObj as ForceData).leftBottomValue;
                        RB = (xForceDataObj as ForceData).rightBottomValue;
                        UpdateRecord();
                    }
                    break;
                default:
                    break;
            }
            
        }

        private void UpdateRecord()
        {
            if (streamWriter != null)
            {
                streamWriter.WriteLine("{0:F2},{1:F2},{2:F2},{3:F2},{4:F2},{5:F2}",
                                        LT, RT, LB, RB, AP_RATIO, LR_RATIO);                                        
            }
        }

        //寫入csv的值--20161108-fair
        public void OnWriteRecord()
        {
            xForceDataObj = (Device as WinForcePlate).getForceData();
            if (xForceDataObj != null)
            {
                if (streamWriter != null)
                {
                    streamWriter.WriteLine("{0:F2},{1:F2},{2:F2},{3:F2},{4:F2},{5:F2}", 1, 2, 3, 4, 5, 6);
                }
            }
        }

        private void WriteCmd(int cmdId, int cmdPara)
        {
            byte[] cmdIdArray = BitConverter.GetBytes(cmdId);
            byte[] cmdParaArray = BitConverter.GetBytes(cmdPara);
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    (Device as AndroidJavaObject).Call("writeBytes", cmdIdArray);
                    (Device as AndroidJavaObject).Call("writeBytes", cmdParaArray);
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    (Device as WinForcePlate).writeBytes(cmdIdArray);
                    (Device as WinForcePlate).writeBytes(cmdParaArray);
                    break;
                default:
                    break;
            }
        }

        private void WriteCmd(int cmdId, float cmdPara)
        {
            byte[] cmdIdArray = BitConverter.GetBytes(cmdId);
            byte[] cmdParaArray = BitConverter.GetBytes(cmdPara);
            switch (Application.platform)
            {
                case RuntimePlatform.Android:
                    (Device as AndroidJavaObject).Call("writeBytes", cmdIdArray);
                    (Device as AndroidJavaObject).Call("writeBytes", cmdParaArray);
                    break;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    (Device as WinForcePlate).writeBytes(cmdIdArray);
                    (Device as WinForcePlate).writeBytes(cmdParaArray);
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void Dummy()
        {
            WriteCmd(0x00, 0x00);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Offset()
        {
            WriteCmd(0x01, 0x00);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Convert(int sampleNumb)
        {
            WriteCmd(0x02, sampleNumb);
        }

        /// <summary>
        /// 
        /// </summary>
        public void BeginCal()
        {
            WriteCmd(0x04, 0x00);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="f"></param>
        public void SetCalPoint(float f)
        {
            WriteCmd(0x05, f / 4f);    // f = Total Weight, f/4 = each sensor weight
        }

        /// <summary>
        /// 
        /// </summary>
        public void EndCal()
        {
            WriteCmd(0x06, 0x00);
        }

        /// <summary>
        /// 
        /// </summary>
        public void AbortCal()
        {
            WriteCmd(0x07, 0x00);
        }
    }
}
