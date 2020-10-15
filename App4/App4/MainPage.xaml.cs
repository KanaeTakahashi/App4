using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App4
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            // タイトルバーを非表示にしたい場合
            NavigationPage.SetHasNavigationBar(this, false);

            // イベントハンドラの紐づけ
            NextPageButton.Clicked += NextPageButton_Clicked;
            NextButton.Clicked += NextButton_Clicked;
        }

        private void NextPageButton_Clicked(object sender, EventArgs e)
        {
            // モーダルでページを表示する場合
            //Navigation.PushModalAsync(new BPage());

            // モーダレスで表示する場合
            Navigation.PushAsync(new BPage());
        }

        private void NextButton_Clicked(object sender, EventArgs e)
        {
            var current = int.Parse(lblCount.Text);
            lblCount.Text = (current + 1).ToString();
        }

        public void Button_OnClicked(object sender, ClickedEventArgs e)
        {
            var current = int.Parse(lblCount.Text);
            lblCount.Text = (current + 1).ToString();
        }
    }
}
