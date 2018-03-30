using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRizDriver
{
    public static class Helper
    {
        /// <summary>
        /// 根据输入值，获取设备点对应的真正值，此方法正好和Transform相反
        /// </summary>
        /// <param name="point"></param>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public static object GetRealValue(Newtonsoft.Json.Linq.JToken point, object objValue)
        {
            double value;
            try
            {
                value = Convert.ToDouble(objValue);
            }
            catch
            {
                return objValue;
            }

            var detail = point["detail"];
            var isDigital = point.Value<bool>("isDigital");
            if(isDigital)
            {
                return value > 0 ? 1 : 0;
            }
            var isTransform = detail.Value<bool>("IsTransform");
            var isLinear = detail.Value<bool>("IsLinear");
            var isSquare = detail.Value<bool>("IsSquare");
            if (isTransform)
            {
                //传感器上限
                double sensorMax = detail.Value<double>("SensorMax");
                double sensorMin = detail.Value<double>("SensorMin");
                double max = point.Value<double>("max");
                double min = point.Value<double>("min");
                var k = (max - min) / (sensorMax - sensorMin);
                var b = (min * (sensorMax - sensorMin) - sensorMin * (max - min)) / (sensorMax - sensorMin);
               // value = k * value + b;//y=k*x+b

                value = (value - b) / k;
            }

            if (isLinear)
            {
                //无法逆推
            }
            return value;
        }

        /// <summary>
        /// 转换模拟量的值
        /// </summary>
        /// <param name="point"></param>
        /// <param name="objValue"></param>
        /// <returns></returns>
        public static object Transform(Newtonsoft.Json.Linq.JToken point , object objValue)
        {
            double value;
            try
            {
                value = Convert.ToDouble(objValue);
            }
            catch
            {
                return objValue;
            }


            var detail = point["detail"];
            var isDigital = point.Value<bool>("isDigital");
            if(isDigital)
            {
                //开关量，只能是1或者0
                value = Convert.ToInt32(value);
                if (value > 1)
                    return 1;
                else if (value < 0)
                    return 0;
                return value;
            }
            var isTransform = detail.Value<bool>("IsTransform");
            var isLinear = detail.Value<bool>("IsLinear");
            var isSquare = detail.Value<bool>("IsSquare");
            var DPCount = detail.Value<string>("DPCount");
            if ( isTransform )
            {
                //传感器上限
                double sensorMax = detail.Value<double>("SensorMax");
                double sensorMin = detail.Value<double>("SensorMin");
                double max = point.Value<double>("max");
                double min = point.Value<double>("min");
                var k = (max - min) / (sensorMax - sensorMin);
                var b = (min * (sensorMax - sensorMin) - sensorMin * (max - min)) / (sensorMax - sensorMin);
                value = k * value + b;//y=k*x+b
            }

            if(isLinear)
            {
                double x1 = 0;
                double y1 = 0;
                double x2 = 0;
                double y2 = 0;
                bool finded = false;
                for(int i = 1; i <= 6; i ++)
                {
                    var curX = detail.Value<double>("LinearX" + i);
                    var curY = detail.Value<double>("LinearY" + i);
                    if(value <= curX)
                    {
                        x2 = curX;
                        y2 = curY;
                        finded = true;
                    }
                    else
                    {
                        x1 = curX;
                        y1 = curY;
                    }
                }
                if (finded)
                {
                    var k = (y2 - y1) / (x2 - x1);
                    value *= k;
                }
                else
                {
                    value = 0;
                }
            }

            if(!string.IsNullOrEmpty(DPCount))
            {
                try
                {
                    value = Math.Round(value, Convert.ToInt32(DPCount));
                }
                catch { }
            }
            return value;
        }
    }
}
