using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PigCommunication
{
    public enum DecodingStatus
    {
        Check_BagBeginning,
        ReceiveDataBag,
        Decoding,
        Decoded
    }
    /// <summary>
    /// 功能类型号
    /// </summary>
    public enum FunctionType
    {
        CameraSend = 0,
    }

    public class Communication
    {
        /// <summary>
        /// 包头
        /// </summary>
        public byte[] BagBeginning = new byte[] { 0x55, 0xAA };
        /// <summary>
        /// 接受缓存区
        /// </summary>
        public List<byte> ReceivedBuff = new List<byte> { };
        /// <summary>
        /// 功能参数列表
        /// </summary>
        public List<int> ParaList = new List<int>();
        /// <summary>
        /// 数据包总长
        /// </summary>
        public int DataBagLength = 0;
        /// <summary>
        /// 数据包缓存
        /// </summary>
        public List<byte> DataBag = new List<byte> { };
        /// <summary>
        /// 数据包读取索引(类似游标)
        /// </summary>
        public int DataBagReadIndex = 0;
        /// <summary>
        /// 数据包读取完毕标志
        /// </summary>
        public bool DataBagReadFinish = false;
        public DecodingStatus NowDecodingStatus = DecodingStatus.Decoded;

        public FunctionType NowDecodingFunction = FunctionType.CameraSend;

        public Dictionary<FunctionType, byte[]> FunctionCheckCodeDic = new Dictionary<FunctionType, byte[]>();
        public Dictionary<FunctionType, int> FunctionParaLenghtDic = new Dictionary<FunctionType, int>();
        public Communication()
        { SomeDicInit(); }
        /// <summary>
        /// 功能号对应的功能校验码字典初始化
        /// </summary>
        public void SomeDicInit()
        {
            FunctionCheckCodeDic.Add(FunctionType.CameraSend, new byte[] { 0xFF, 0x00 });
            FunctionParaLenghtDic.Add(FunctionType.CameraSend, 4);

        }
        /// <summary>
        /// 串口数据解包程序
        /// </summary>
        /// <param name="data"></param>
        /// <param name="bufferlen"></param>
        public void DataDecoding()   //接收到串口字节后事件处理方法
        {
            while (true)
            {
                List<byte> ReveiceData = new List<byte> { };
                if (NowDecodingStatus != DecodingStatus.Decoded)
                {
                    Console.WriteLine("********解包线程捕捉到非Decoded状态********");
                    lock (this)
                        ReceivedBuff.ForEach(k => ReveiceData.Add(k));
                    Console.WriteLine("解包缓存字符串：");
                    //lock (this)
                    //    PrintByteStrWithByteArr(ReveiceData);
                    Console.WriteLine("********数据缓存完毕,进入解包程序********");
                }

                if (NowDecodingStatus == DecodingStatus.Check_BagBeginning)
                {
                    #region 检测包头
                    int BagBeginningIndex = FindSonArrayInByteArray(ReveiceData.ToArray(), BagBeginning);
                    if (BagBeginningIndex != -1)
                        Console.WriteLine("检测到55 AA的包头！");
                    else
                    { NowDecodingStatus = DecodingStatus.Decoded; continue; }
                    #endregion
                    Console.WriteLine("********检测包头完毕,进入检测功能********");
                    # region 检测功能号
                    int FunctionIndex = 0;
                    FunctionType FTBuff = FunctionType.CameraSend;
                    if (BagBeginningIndex + BagBeginning.Length + 1 > ReveiceData.Count)//长度不够功能号检测了
                    { NowDecodingStatus = DecodingStatus.Decoded; continue; }
                    else
                    {
                        FunctionIndex = BagBeginningIndex + BagBeginning.Length;
                        int FunctionNum = Convert.ToInt16(ReveiceData[FunctionIndex]);
                        FTBuff = (FunctionType)(FunctionNum);
                        NowDecodingFunction = FTBuff;
                    }
                    # endregion
                    Console.WriteLine("检测到功能：" + NowDecodingFunction.ToString());
                    Console.WriteLine("********检测功能完毕,进入检测参数********");
                    #region 检测相应功能号的校验码
                    //if (FunctionIndex + FunctionCheckCodeDic[FTBuff].Length + 1 > ReveiceData.Count)
                    //{ NowDecodingStatus = DecodingStatus.Decoded; continue; }
                    //bool CheckCodeMatch = ByteListMatch(ReveiceData, FunctionCheckCodeDic[FTBuff], FunctionIndex + 1);
                    //if (CheckCodeMatch)
                    //{
                    //    Console.WriteLine("检测到功能" + FTBuff.ToString());
                    //    NowDecodingFunction = FTBuff;
                    //}
                    //else
                    //{ NowDecodingStatus = DecodingStatus.Decoded; continue; }
                    # endregion
                    # region 检测相应功能的参数信息
                    int ParaIndex = FunctionIndex + 1;
                    if (!FunctionParaLenghtDic.ContainsKey(FTBuff))
                    { NowDecodingStatus = DecodingStatus.Decoded; continue; }
                    if (ParaIndex + FunctionParaLenghtDic[FTBuff] > ReveiceData.Count)
                    { NowDecodingStatus = DecodingStatus.Decoded; continue; }
                    switch (NowDecodingFunction)
                    {
                        case FunctionType.CameraSend:
                            Console.WriteLine("Height:" + Convert.ToInt16(ReveiceData[ParaIndex]) * 256 + Convert.ToInt16(ReveiceData[ParaIndex + 1]).ToString());
                            Console.WriteLine("Width:" + Convert.ToInt16(ReveiceData[ParaIndex + 2]) * 256 + Convert.ToInt16(ReveiceData[ParaIndex + 3]).ToString());
                            int ImageHeight = Convert.ToInt32(ReveiceData[ParaIndex])* 256 + Convert.ToInt32(ReveiceData[ParaIndex + 1]);
                            int ImageWidth = Convert.ToInt32(ReveiceData[ParaIndex + 2]) * 256 + Convert.ToInt32(ReveiceData[ParaIndex + 3]);
                            ParaList.Clear();
                            ParaList.Add(ImageHeight);
                            ParaList.Add(ImageWidth);
                            DataBagLength = ImageHeight * ImageWidth;
                            break;
                        default:
                            break;
                    }
                    # endregion
                    Console.WriteLine("********检测参数完毕,接受数据开始********");
                    # region 剩下的数据存入数据缓存区
                    lock (this)
                    { DataBag.Clear(); }
                    DataBagReadIndex = ParaIndex + FunctionParaLenghtDic[NowDecodingFunction];
                    
                    # endregion
                    NowDecodingFunction = FTBuff;
                    NowDecodingStatus = DecodingStatus.ReceiveDataBag;
                }
                else if (NowDecodingStatus == DecodingStatus.ReceiveDataBag)
                {
                    lock (this)
                    {
                        DataReceiveAndCheck(ReveiceData);
                    }
                    if (NowDecodingStatus == DecodingStatus.Check_BagBeginning)
                        continue;
                    else
                        NowDecodingStatus = DecodingStatus.Decoded;
                }
            }
        }
        /// <summary>
        /// 数据包接受状态的数据接收程序
        /// </summary>
        /// <param name="ReveiceData">解包缓存数据</param>
        private void DataReceiveAndCheck(List<byte> ReveiceData)
        {
            # region 数据存入数据缓存区
            for (int i = DataBagReadIndex; i < ReveiceData.Count; i++)
            {
                DataBag.Add(ReveiceData[i]);
                # region 检测数据读取结束（或者包尾）
                if (DataBag.Count >= DataBagLength && NowDecodingFunction == FunctionType.CameraSend)
                {
                    NowDecodingStatus = DecodingStatus.Check_BagBeginning;
                    Console.WriteLine("数据包全部接受完成，进入包头检测模式!");
                    # region 删除前面所有的数据包
                    //Console.WriteLine("数据包：");
                    //PrintByteStrWithByteArr(DataBag);
                    ReceivedBuff.RemoveRange(0, DataBagReadIndex + DataBagLength);                                     
                    # endregion
                    Console.WriteLine("********接受数据完毕,进入包头检测********");
                    DataBagReadFinish = true;
                    break;
                }
                # endregion
            }
            DataBagReadIndex = ReveiceData.Count;
            //Console.WriteLine("现在的数据包：");
            //PrintByteStrWithByteArr(DataBag);
            # endregion
        }
        /// <summary>
        /// 将Byte数组转换成Byte字符串
        /// </summary>
        /// <param name="ByteList">Byte列表</param>
        /// <returns>Byte字符串</returns>
        public static string ByteArrToStr(byte[] ByteList)
        {
            string ByteStr = "";
            foreach (byte Byte in ByteList)
            {
                int GaoWei = Byte / 16;
                int DiWei = Byte % 16;

                string ByteStrBuff = IntToHexChar(GaoWei) + IntToHexChar(DiWei);
                ByteStrBuff += " ";
                ByteStr += ByteStrBuff;
            }
            return ByteStr;
        }
        /// <summary>
        /// 将Byte数组以Byte字符串打印出来，如55 AA这样的字符串
        /// </summary>
        /// <param name="PrintStr">待打印的字符串</param>
        public void PrintByteStrWithByteArr(List<byte> PrintByteArray)
        {
            //int PrintCount = 0;
            //foreach (byte Byte in PrintByteArray)
            //{
            //    int GaoWei = Byte / 16;
            //    int DiWei = Byte % 16;

            //    string ByteStr = IntToHexChar(GaoWei) + IntToHexChar(DiWei);

            //    Console.Write(ByteStr + " ");
            //    PrintCount++;
            //    if (PrintCount > 20)
            //    {
            //        Console.WriteLine();
            //        PrintCount = 0;
            //    }
            //}
            //if (PrintCount != 0)
            //{
            //    Console.WriteLine();                
            //}
        
        }

        /// <summary>
        /// int数字转换成Hex字符
        /// </summary>
        public static string IntToHexChar(int HexNum)
        {
            if (HexNum >= 16 || HexNum < 0)
            { System.Windows.Forms.MessageBox.Show("输入的Hex数字超过了0~15"); return ""; }
            else if (HexNum < 10)
                return HexNum.ToString();
            else
            {
                switch (HexNum)
                {
                    case 10: return "A";
                    case 11: return "B";
                    case 12: return "C";
                    case 13: return "D";
                    case 14: return "E";
                    case 15: return "F";
                    default: return "";
                }
            }
        }
        /// <summary>
        /// Hex字符转Byte
        /// </summary>
        /// <param name="HexChar">Hex字符</param>
        /// <returns>Byte输出</returns>
        public static byte HexCharToByte(string HexChar)
        {
            int NumBuff = 0;
            if (int.TryParse(HexChar, out NumBuff))
            {
                return Convert.ToByte(NumBuff);
            }
            else
            {
                switch (HexChar)
                {
                    case "A": return Convert.ToByte(10);
                    case "B": return Convert.ToByte(11);
                    case "C": return Convert.ToByte(12);
                    case "D": return Convert.ToByte(13);
                    case "E": return Convert.ToByte(14);
                    case "F": return Convert.ToByte(15);
                    case "a": return Convert.ToByte(10);
                    case "b": return Convert.ToByte(11);
                    case "c": return Convert.ToByte(12);
                    case "d": return Convert.ToByte(13);
                    case "e": return Convert.ToByte(14);
                    case "f": return Convert.ToByte(15);
                    default: System.Windows.Forms.MessageBox.Show("Hex格式不正确"); return Convert.ToByte(0);
                }
            }
        }
        /// <summary>
        /// 在Byte数组中寻找子数组的位置，-1为没找到
        /// </summary>
        public static int FindSonArrayInByteArray(byte[] FatherArray, byte[] SonArray)
        {
            int SearchStartIndex = 0;
            if (SonArray.Length == 1)
                return Array.IndexOf(FatherArray, SonArray[0], 0);
            else if (SonArray.Length == 0)
                return -1;
            else
            {
                while (true)
                {
                    int IndexBuff = Array.IndexOf(FatherArray, SonArray[0], SearchStartIndex);
                    if (IndexBuff == -1)
                        break;
                    else
                    {
                        if (SonArray.Length > FatherArray.Length - IndexBuff)//检测子数组大小是否长于父数组IndexBuff开始到结尾的长度
                        {
                            SearchStartIndex = IndexBuff + 1;
                            continue;
                        }
                        bool MacthFlag = true;
                        //检验剩下的元素是否符合
                        for (int i = 1; i < SonArray.Length; i++)
                        {
                            if (FatherArray[IndexBuff + i] != SonArray[i])
                            {
                                MacthFlag = false;
                                break;
                            }
                        }
                        if (MacthFlag)
                        {
                            return IndexBuff;
                        }
                        else
                        {
                            SearchStartIndex = IndexBuff + 1;
                            continue;
                        }

                    }
                }
            }

            return -1;
        }
        /// <summary>
        /// 检测Byte列表中从Offset开始是否有连续的CheckCode
        /// </summary>
        public static bool ByteListMatch(List<byte> DataList, byte[] CheckCode, int Offset)
        {
            bool MacthFlag = true;
            //检验所有元素是否符合
            for (int i = 0; i < CheckCode.Length; i++)
            {
                if (DataList[Offset + i] != CheckCode[i])
                {
                    MacthFlag = false;
                    break;
                }
            }
            return MacthFlag;
        }
    }
}
