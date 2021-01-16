using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Data;
using System.Net;
using System.IO;

namespace BaiduMapCorrect
{
    public class MapCorrect
    {
        /// <summary>
        /// 将GPS坐标中的经度或纬度转换成区域对应的经度或纬度
        /// </summary>
        /// <param name="GpsCoordinate"></param>
        /// <returns></returns>
        private int GetAreaPostion(int GpsCoordinate)
        {
            //计算"度"的部分
            int nDegree = GpsCoordinate / 1000000 * 1000000;
            //计算度后面小数部分
            int nSecond = (int)(0.000001 * (GpsCoordinate - nDegree) * 3600);
            //两者重新相加
            return nDegree + nSecond;
        }
        
        public Point GetFromBaidu(Point GpsCoordinate)
        {
            Point pt = new Point(0, 0);
            try
            {
                string url = string.Format("http://api.map.baidu.com/ag/coord/convert?from=0&to=4&x={0}&y={1}",
                    0.000001 * GpsCoordinate.X, 0.000001 * GpsCoordinate.Y);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = 3000;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream stream = response.GetResponseStream();
                byte[] bytes = new byte[1024];
                int n = stream.Read(bytes, 0, 1024);
                response.Close();
                if (n < 2)
                {
                    return pt;
                }
                else
                {
                    string s = System.Text.Encoding.UTF8.GetString(bytes).Substring(1, n - 2).Replace("\"", "");
                    foreach (string team in s.Split(','))
                    {
                        string[] infos = team.Split(':');
                        if (infos.Length < 2)
                        {
                            return pt;
                        }
                        string strValue = infos[1];
                        switch (infos[0])
                        {
                            case "error":
                                if (strValue != "0")
                                    return pt;
                                break;
                            case "x":
                                {
                                    byte[] outputb = Convert.FromBase64String(strValue);
                                    strValue = Encoding.Default.GetString(outputb);
                                    pt.X = (int)(double.Parse(strValue) * 1000000);
                                }
                                break;
                            case "y":
                                {
                                    byte[] outputb = Convert.FromBase64String(strValue);
                                    strValue = Encoding.Default.GetString(outputb);
                                    pt.Y = (int)(double.Parse(strValue) * 1000000);
                                }
                                break;
                        }
                    }

                }
            }
            catch
            {
                return pt;
            }
            return pt;
        }
    }
}
