using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace App4
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BPage : ContentPage
    {
        public BPage()
        {
            InitializeComponent();

            // イベントを紐づけ
            btnPosition.Clicked += async (sender, e) => { await GetPositionButton_ClickedAsync(sender, e); };
        }

        /// <summary>
        /// 位置情報を取得ボタン クリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async Task GetPositionButton_ClickedAsync(object sender, EventArgs e)
        {
            if (!IsLocationAvailable()) return;

            var result = await GetCurrentPosition();
            editPosition.Text = ProcessingForDisplay(result);
        }

        /// <summary>
        /// ゲオロケーターのチェック
        /// </summary>
        /// <returns></returns>
        public bool IsLocationAvailable()
        {
            if (!CrossGeolocator.IsSupported)
                return false;

            return CrossGeolocator.Current.IsGeolocationAvailable;
        }

        /// <summary>
        /// 現在のロケーションを取得
        /// </summary>
        /// <returns></returns>
        public static async Task<Plugin.Geolocator.Abstractions.Position> GetCurrentPosition()
        {
            Plugin.Geolocator.Abstractions.Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //got a cahched position, so let's use it.
                    return position;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    //not available or enabled
                    return null;
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location: " + ex);
            }

            if (position == null)
                return null;

            Debug.WriteLine(ProcessingForDisplay(position));

            return position;
        }

        /// <summary>
        /// 表示用に加工
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private static string ProcessingForDisplay(Plugin.Geolocator.Abstractions.Position position)
        {
            return string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                    position.Timestamp, position.Latitude, position.Longitude,
                    position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);
        }

        /// <summary>
        /// OnAppearingメソッド（Pageが表示される直前）
        /// 参考サイト：https://shuhelohelo.hatenablog.com/entry/2020/01/02/031311
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                //東京へ移動させる
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.GoogleMaps.Position(35.6762, 139.6503), Distance.FromKilometers(100)));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}