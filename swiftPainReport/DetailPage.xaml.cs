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
    public partial class DetailPage : ContentPage
    {
        private int selectedArea;
        private PainDataManager painDataManager;
        public DetailPage(int buttonName)
        {
            InitializeComponent();
            selectedArea = buttonName;
            painDataManager = new PainDataManager();
        }

        private async void OnPainLevelClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            int painLevel = int.Parse(button.ClassId);

            await button.FadeTo(0.7, 50);
            await button.FadeTo(1, 50);

            try
            {
                string csvLine = painDataManager.FormatCsvLine(selectedArea, painLevel);
                bool saveResult = painDataManager.SavePainRecord(csvLine);

                if (saveResult)
                {
                    // selectedArea를 name_area에 저장
                    // selectedArea: 허리-1 / 어깨 -2 / 목-3/
                    string name_area = "";

                    if (selectedArea == 1) {
                        name_area = "허리";
                    }
                    else if (selectedArea == 2)
                    {
                        name_area = "어깨";
                    }
                    else if (selectedArea == 3)
                    {
                        name_area = "목";
                    }

                    // painLevel: 통증없음-0 / 경증-1 / 심함-2 / 매우심함-3

                    string painLevelString = "";

                    if (painLevel == 0)
                    {
                        painLevelString = "통증없음";
                    }
                    else if (painLevel == 1)
                    {
                        painLevelString = "경증";
                    }
                    else if (painLevel == 2)
                    {
                        painLevelString = "심함";
                    }
                    else if (painLevel == 3)
                    {
                        painLevelString = "매우심함";
                    }


                    var alertTask = DisplayAlert($"{name_area}-{painLevelString}", "통증 기록이 완료되었습니다.", "확인");
                    var delayTask = Task.Delay(3000);

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
        }
    }
}