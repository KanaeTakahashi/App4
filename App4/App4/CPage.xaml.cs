using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace App4
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CPage : ContentPage
    {
        /// <summary>
        /// 検索条件入力コントロール。入力された値をイベントハンドラ内から取得するためメンバ変数に保持しておく
        /// </summary>
        private SearchBar searchBar;
        /// <summary>
        /// 地図コントロール。検索結果から該当地点へ移動などを行うためメンバ変数に保持しておく
        /// </summary>
        private Map map;
        /// <summary>
        /// Geocoding処理は非同期で実施するため、ローカル変数ではなくメンバ変数に保持しておく
        /// </summary>
        private readonly Geocoder geocoder = new Geocoder();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        [Obsolete]
        public CPage()
        {
            InitializeComponent();

            // 検索バーと地図コントロールを作成し、StackLayoutへ設定する
            searchBar = new SearchBar();
            map = new Map { IsShowingUser = true };
            var stack = new StackLayout();
            stack.Children.Add(searchBar);
            stack.Children.Add(map);
            // ページコンテンツとしてStackLayoutを登録する
            Content = stack;

            // 検索バーへ検索イベントのイベントハンドラを設定する
            searchBar.SearchButtonPressed += OnSearchButtonPressed;

            //map.MapLongClicked += Map_MapLongClickedAsync; // 同期形式のイベント関連付けの書き方
            map.MapLongClicked += async (sender, e) => { await Map_MapLongClickedAsync(sender, e); };
        }

        /// <summary>
        /// 検索イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnSearchButtonPressed(object sender, EventArgs e)
        {
            // 検索バーから入力された地名から緯度・経度を取得する
            var positions = await geocoder.GetPositionsForAddressAsync(searchBar.Text);
            // 座標は複数とれる可能性があるが、今回は先頭の座標を利用する
            var position = positions.FirstOrDefault();
            // 座標が一つ以上とれていた場合のみ以下を処理する
            if (position != null)
            {
                // 該当地点へ移動する
                map.MoveToRegion(
                    MapSpan.FromCenterAndRadius(
                        position,
                        Distance.FromMiles(0.2)));
                // 座標から住所を逆引きする
                var addresses = await geocoder.GetAddressesForPositionAsync(position);
                // 住所は複数とれる可能性があるが、今回は先頭の住所を利用する
                var address = addresses.FirstOrDefault();
                // 住所が一つ以上とれていた場合、以下を処理する
                if (address != null)
                {
                    // 以前設定したピンがあればピンを除去する
                    map.Pins.Clear();
                    // 新たにピンを作成し地図へ登録する
                    var pin = new Pin
                    {
                        Type = PinType.Place,               // ピンの形状
                        Position = position,                // ピンを登録する座標
                        Label = searchBar.Text,             // ピンのラベル。検索条件を設定
                        Address = address.Replace("\n", "") // ピンの住所。取得した住所の先頭が「日本\n～」となるので改行を除去する
                    };
                    map.Pins.Add(pin);
                }
            }
        }

        /// <summary>
        /// MAPの長押しイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async Task Map_MapLongClickedAsync(object sender, MapLongClickedEventArgs e)
        {
            var position = new Position(e.Point.Latitude, e.Point.Longitude);
            // 座標から住所を逆引きする
            var addresses = await geocoder.GetAddressesForPositionAsync(position);
            // 住所は複数とれる可能性があるが、今回は先頭の住所を利用する
            var address = addresses.FirstOrDefault();
            // 住所が一つ以上とれていた場合、以下を処理する
            if (address != null)
            {
                // 以前設定したピンがあればピンを除去する
                map.Pins.Clear();
                // 新たにピンを作成し地図へ登録する
                var pin = new Pin
                {
                    Type = PinType.Place,               // ピンの形状
                    Position = position,                // ピンを登録する座標
                    Label = "追加の場所",  // ピンのラベル。検索条件を設定
                    Address = address.Replace("\n", "") // ピンの住所。取得した住所の先頭が「日本\n～」となるので改行を除去する
                };
                map.Pins.Add(pin);

                searchBar.Text = address.Replace("\n", "");
            }

        }
    }
}