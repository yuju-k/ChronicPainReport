using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace swiftPainReport
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private PainDataManager painDataManager;
        public MainPage()
        {
            InitializeComponent();
            painDataManager = new PainDataManager();
        }

        private async void OnPainButtonClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            int selectedArea;

            // 버튼에 따른 시각적 효과 처리
            if (button.BackgroundColor == Color.Transparent)
            {
                button.BackgroundColor = Color.FromHex("#EEEEEE");
                await Task.Delay(50);
                button.BackgroundColor = Color.Transparent;
            }
            else
            {
                await button.FadeTo(0.7, 50);
                await button.FadeTo(1, 50);
            }

            if (button.ClassId == "waist")
            {
                selectedArea = 1; //허리
            }
            else if (button.ClassId == "shoulder")
            {
                selectedArea = 2; //어깨
            }
            else if (button.ClassId == "neck")
            {
                selectedArea = 3;
            }
            else
            {
                selectedArea = 0;
                int painLevel = 0;

                // 통증없음 선택하면 다음 화면 넘어가지 않고 그냥 저장함
                try
                {
                    string csvLine = painDataManager.FormatCsvLine(selectedArea, painLevel);
                    bool saveResult = painDataManager.SavePainRecord(csvLine);

                    if (saveResult)
                    {
                        //await DisplayAlert("Success", "Pain level saved", "OK");
                        //Tizen.Applications.Application.Current.Exit();

                        // Alert를 보여주고 자동으로 앱 종료
                        var alertTask = DisplayAlert("통증없음", "통증 기록이 완료되었습니다.", "확인");
                        var delayTask = Task.Delay(3000);

                        // 둘 중 먼저 완료되는 작업(사용자가 확인 누르거나 3초 지남) 후 앱 종료
                        await Task.WhenAny(alertTask, delayTask);
                        Tizen.Applications.Application.Current.Exit();
                    }
                    else
                    {
                        throw new Exception("Failed to save pain record");
                    }
                }
                catch (Exception ex)
                {
                    Tizen.Log.Error("painReport3", "Error saving pain record: " + ex.Message);
                    await DisplayAlert("Error", "Failed to save pain level", "OK");
                }
                return;
            }

            // 새로운 페이지로 이동하면서 버튼 텍스트 전달
            await Navigation.PushAsync(new DetailPage(selectedArea));
            Tizen.Log.Info("painReport3", $"Selected area: {selectedArea}");
        }
    }
}