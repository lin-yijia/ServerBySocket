using BaiduMapCorrect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerBySocket
{
    public partial class GPS : Form
    {
        public GPS()
        {
            InitializeComponent();
        }

        SocketManager _sm = null;
        string ip = "120.25.176.139";
        int port = 1989;
        private void Form2_Load(object sender, EventArgs e)
        {
            _sm = new SocketManager(ip, port);
        }


        private void btn_zh_Click(object sender, EventArgs e)
        {
            string xtb = this.txt_xtb.Text;
            if (xtb.IndexOf("*") == 0)
            {
                string[] list = xtb.Substring(1, xtb.Length - 2).Split(',');
                //制造商名称
                this.txt_zzs.Text = list[0];
                //车载机序列号。10位数
                this.txt_sbh.Text = list[1];
                //时间
                string time = list[3];
                //数据有效位
                this.txt_yxw.Text = list[4].ToString() == "A" ? "有效位置" : "无效位置";
                //纬度
                this.txt_wd.Text = list[5].Replace(".", "");
                //纬度标志
                this.txt_wdbz.Text = list[6].ToString() == "N" ? "北纬" : "南纬";
                //经度
                this.txt_jd.Text = list[7].Replace(".", "");
                //经度标志
                this.txt_jdbz.Text = list[8].ToString() == "E" ? "东经" : "西经";
                //速度
                this.txt_sd.Text = list[9];
                //方位角
                this.txt_fx.Text = list[10];
                //日期
                this.txt_rq.Text = list[11].ToString().Substring(4, 2) + "-" + list[11].ToString().Substring(2, 2) + "-" + list[11].ToString().Substring(0, 2) + ":" + time;
                //车辆状态
                this.txt_zt.Text = list[12];
                //移动国家码
                this.txt_ydgjm.Text = list[13];
                //移动网络码
                this.txt_ydwlm.Text = list[14];
                //基站区域码
                this.txt_jzqym.Text = list[15];
                //基站编码
                this.txt_jzbm.Text = list[16];

                //纬度
                string lat = convert(this.txt_wd.Text, 0);
                //经度
                string lng = convert(this.txt_jd.Text, 1);

                string point = lng + "," + lat;

                this.txt_xsjwd.Text = point;

                MapCorrect map = new MapCorrect();

                double dLng, dLat;
                double.TryParse(lng, out dLng);
                double.TryParse(lat, out dLat);

                //如果输入的是度,转换成百万分之一度
                if (dLng < 360)
                    dLng *= 1000000;
                if (dLat < 360)
                    dLat *= 1000000;
                //检查是否在中过境范围内
                if (dLng < 72000000 || dLng > 136000000 // --经度异常
                       || dLat < 3000000 || dLat > 54000000) //纬度异常
                {
                    MessageBox.Show("坐标不正确");
                    return;
                }

                int Lng = (int)dLng, Lat = (int)dLat;
                Point pt = map.GetFromBaidu(new Point(Lng, Lat));
                this.txt_baidu.Text = string.Format("{0},{1}",
                     0.000001 * pt.X, 0.000001 * pt.Y);

            }
            else if (xtb.IndexOf("24") > -1)
            {
                //2461707188150817201807172237826006114036610E000000FFFFBBFF000001030000000001CC0027BA0E4D51
                string list = xtb.Substring(2, 10);

                //制造商名称
                this.txt_zzs.Text = "";
                //车载机序列号。10位数
                this.txt_sbh.Text = xtb.Substring(2, 10);
                //时间
                string time = xtb.Substring(18, 6) + "-" + xtb.Substring(12, 6) + "-";

                //纬度
                this.txt_wd.Text = xtb.Substring(24, 8);

                //电池
                string dianchi = xtb.Substring(32, 2);

                //经度
                this.txt_jd.Text = xtb.Substring(34, 9);

                ////纬度标志
                //this.txt_wdbz.Text = xtb.Substring(42, 1) == "N" ? "北纬" : "南纬";

                ////数据有效位
                //this.txt_yxw.Text = xtb.Substring(42, 1) == "A" ? "有效位置" : "无效位置";

                //经度标志
                this.txt_jdbz.Text = xtb.Substring(43, 1) == "E" ? "东经" : "西经";
                //速度
                this.txt_sd.Text = xtb.Substring(44, 3);
                //方位角
                this.txt_fx.Text = xtb.Substring(47, 3);
                //日期
                this.txt_rq.Text = time;
                //车辆状态
                this.txt_zt.Text = xtb.Substring(50, 8);

                //保留
                string baoliu = xtb.Substring(58, 2);

                //移动国家码
                this.txt_ydgjm.Text = xtb.Substring(60, 3);
                //移动网络码
                this.txt_ydwlm.Text = xtb.Substring(63, 2);
                //基站区域码
                this.txt_jzqym.Text = xtb.Substring(65, 5);
                //基站编码
                this.txt_jzbm.Text = xtb.Substring(70, 4);


                //纬度
                string lat = convert(this.txt_wd.Text, 0);
                //经度
                string lng = convert(this.txt_jd.Text, 1);

                string point = lng + "," + lat;

                this.txt_xsjwd.Text = point;

                MapCorrect map = new MapCorrect();

                double dLng, dLat;
                double.TryParse(lng, out dLng);
                double.TryParse(lat, out dLat);

                //如果输入的是度,转换成百万分之一度
                if (dLng < 360)
                    dLng *= 1000000;
                if (dLat < 360)
                    dLat *= 1000000;
                //检查是否在中过境范围内
                if (dLng < 72000000 || dLng > 136000000 // --经度异常
                       || dLat < 3000000 || dLat > 54000000) //纬度异常
                {
                    MessageBox.Show("坐标不正确");
                    return;
                }

                int Lng = (int)dLng, Lat = (int)dLat;
                Point pt = map.GetFromBaidu(new Point(Lng, Lat));
                this.txt_baidu.Text = string.Format("{0},{1}",
                     0.000001 * pt.X, 0.000001 * pt.Y);
            }
            else
            {
                MessageBox.Show("请输入数据有误。");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="number">经纬度值</param>
        /// <param name="type">0纬度1经度</param>
        /// <returns></returns>
        public string convert(string number, int type)
        {
            string value = "";
            if (type == 0)
            {
                string value1 = number.Substring(0, 2);
                string value2 = number.Substring(2, 2) + "." + number.Substring(4, number.Length - 4);
                value = value1 + "." + String.Format("{0:N4}", (Convert.ToDouble(value2) / 60)).Split('.')[1];
            }
            else
            {
                string value1 = number.Substring(0, 3);
                string value2 = number.Substring(3, 2) + "." + number.Substring(5, number.Length - 5);
                value = value1 + "." + String.Format("{0:N4}", (Convert.ToDouble(value2) / 60)).Split('.')[1];
            }

            return value;
        }

        /// <summary>
        /// 度分秒经纬度(必须含有'°')和数字经纬度转换
        /// </summary>
        /// <param name="digitalDegree">度分秒经纬度</param>
        /// <return>数字经纬度</return>
        public double ConvertDegreesToDigital(string degrees)
        {
            const double num = 60;
            double digitalDegree = 0.0;
            int d = degrees.IndexOf('°');           //度的符号对应的 Unicode 代码为：00B0[1]（六十进制），显示为°。
            if (d < 0)
            {
                return digitalDegree;
            }
            string degree = degrees.Substring(0, d);
            digitalDegree += Convert.ToDouble(degree);

            int m = degrees.IndexOf('′');           //分的符号对应的 Unicode 代码为：2032[1]（六十进制），显示为′。
            if (m < 0)
            {
                return digitalDegree;
            }
            string minute = degrees.Substring(d + 1, m - d - 1);
            digitalDegree += ((Convert.ToDouble(minute)) / num);

            int s = degrees.IndexOf('″');           //秒的符号对应的 Unicode 代码为：2033[1]（六十进制），显示为″。
            if (s < 0)
            {
                return digitalDegree;
            }
            string second = degrees.Substring(m + 1, s - m - 1);
            digitalDegree += (Convert.ToDouble(second) / (num * num));

            return digitalDegree;
        }



        private static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }

        public static byte[] str2ASCII(String xmlStr)
        {
            return Encoding.Default.GetBytes(xmlStr);
        }

        /// <summary>
        /// 将一条十六进制字符串转换为ASCII
        /// </summary>
        /// <param name="hexstring">一条十六进制字符串</param>
        /// <returns>返回一条ASCII码</returns>
        public static string HexStringToASCII(string hexstring)
        {
            byte[] bt = HexStringToBinary(hexstring);
            string lin = "";
            for (int i = 0; i < bt.Length; i++)
            {
                lin = lin + bt[i] + " ";
            }


            string[] ss = lin.Trim().Split(new char[] { ' ' });
            char[] c = new char[ss.Length];
            int a;
            for (int i = 0; i < c.Length; i++)
            {
                a = Convert.ToInt32(ss[i]);
                c[i] = Convert.ToChar(a);
            }

            string b = new string(c);
            return b;
        }

        /**/
        /// <summary>
        /// 16进制字符串转换为二进制数组
        /// </summary>
        /// <param name="hexstring">用空格切割字符串</param>
        /// <returns>返回一个二进制字符串</returns>
        public static byte[] HexStringToBinary(string hexstring)
        {
            try
            {


                string[] tmpary = hexstring.Trim().Split(' ');
                byte[] buff = new byte[tmpary.Length];
                for (int i = 0; i < buff.Length; i++)
                {
                    buff[i] = Convert.ToByte(tmpary[i], 16);
                }
                return buff;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        //字符串转换为16进制byte数组
        public byte[] StrToHexByte(string data)
        {
            data = data.Replace(" ", "");
            if ((data.Length % 2) != 0)
            {
                data += " ";
            }

            byte[] bytes = new byte[data.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(data.Substring(i * 2, 2), 16);
            }

            return bytes;
        }

        // 字节数组转16进制字符串   
        public string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");//ToString("X2") 为C#中的字符串格式控制符
                }
            }
            return returnStr;
        }

        private void btn_sendzl_Click(object sender, EventArgs e)
        {
            string clientIP = this.txt_IP.Text;
            string issend = _sm.SendMsg(this.txt_zl.Text, clientIP);
            if (issend.Equals("ok"))
            {
                MessageBox.Show("已发送");
            }
            else
            {
                MessageBox.Show("失败:"+issend);
            }
        }
    }
}
