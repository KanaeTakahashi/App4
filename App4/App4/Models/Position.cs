using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.GoogleMaps;

namespace App4.Models
{
    /// <summary>
    /// 位置情報クラス
    /// </summary>
    public class Position
    {
        /// <summary>
        /// 取得時間
        /// </summary>
        public static DateTime TimeStamp { get; set; }
        /// <summary>
        /// 緯度
        /// </summary>
        public static double Latitude { get; set; }
        /// <summary>
        /// 経度
        /// </summary>
        public static double Longitude { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="p"></param>
        public Position(Plugin.Geolocator.Abstractions.Position p)
        {
            // DateTimeOffset から DateTime へ変換（DateTimeOffsetは時差を考慮する場合に使用する構造体）
            TimeStamp = p.Timestamp.DateTime;
            Latitude = p.Latitude;
            Longitude = p.Longitude;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="p"></param>
        public Position(Pin p) {
            TimeStamp = DateTime.Now;
            Latitude = p.Position.Latitude;
            Longitude = p.Position.Longitude;
        }

        /// <summary>
        /// 表示用に加工
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public string ProcessingForDisplay()
        {
            return string.Format("取得日時: {0} \n緯度: {1} \n経度: {2}",
                    TimeStamp, Latitude, Longitude);
        }
    }
}
