using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Markup;
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
            btnNext.Clicked += BtnNext_Clicked;
            btnPosition.Clicked += async (sender, e) => { await GetPositionButton_ClickedAsync(sender, e); };
        }

        [Obsolete]
        private void BtnNext_Clicked(object sender, EventArgs e)
        {
            // モーダレスで表示する場合
            Navigation.PushAsync(new CPage());
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
            var pItem = new Models.Position(result);
            editPosition.Text = pItem.ProcessingForDisplay();
            AddPosition(result);
        }

        /// <summary>
        /// 位置の追加
        /// </summary>
        /// <param name="p"></param>
        private void AddPosition(Plugin.Geolocator.Abstractions.Position p)
        {
            var pin = new Pin()
            {
                Type = PinType.Place,
                Label = "現在位置",
                Address = "",
                Position = new Xamarin.Forms.GoogleMaps.Position(p.Latitude, p.Longitude),
                //Rotation = 33.3f,
                Tag = "",
                IsDraggable = true,
            };
            MyMap.Pins.Add(pin);
        }

        // MAPの長押しイベント
        private void MyMap_MapLongClicked(object sender, MapLongClickedEventArgs e)
        {
            var pin = new Pin()
            {
                Type = PinType.Place,
                Label = "Long Tapped",
                Address = "Added New Pin",
                Position = e.Point,//長押しした地図上の位置
                Rotation = 33.3f,
                Tag = "",
                IsDraggable = true,
            };
            MyMap.Pins.Add(pin);

            var pItem = new Models.Position(pin);

            editPosition.Text = pItem.ProcessingForDisplay();
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
                locator.DesiredAccuracy = 50;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //got a cahched p, so let's use it.
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

            var pItem = new Models.Position(position);
            Debug.WriteLine(pItem.ProcessingForDisplay());

            return position;
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
                //高知駅へ移動させる
                MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.GoogleMaps.Position(33.5684, 133.5566), Distance.FromKilometers(10)));

                //ピンを立てる
                var pin = new Pin()
                {
                    Type = PinType.Place,
                    Label = "ピンを追加テスト",
                    Address = "Hello Tokyo",
                    Position = new Xamarin.Forms.GoogleMaps.Position(35.6762, 139.6503),  //東京
                    Rotation = 0,  //  -33.3f などとするとピンを傾けることができる
                    Tag = "",
                    IsDraggable = true,//これだけでピンを長押しすると移動モードになる
                };
                //マップへ追加
                MyMap.Pins.Add(pin);

                //var pin2 = new Pin()
                //{
                //    Type = PinType.Place,
                //    Label = "高知駅",
                //    Address = "Hello Kochi",
                //    Position = new Xamarin.Forms.GoogleMaps.Position(33.5658, 133.5437),  //高知駅
                //    Rotation = 0,
                //    Tag = "",
                //    IsDraggable = true,
                //};
                //MyMap.Pins.Add(pin2);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}