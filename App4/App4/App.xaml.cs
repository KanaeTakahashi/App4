using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App4
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //  モーダル表示の場合
            //MainPage = new MainPage();

            // モーダレス表示の場合
            // NavigationPageのインスタンスを設定する
            MainPage = new NavigationPage(new App4.MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
