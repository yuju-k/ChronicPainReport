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
                    await DisplayAlert("Success", "Pain level saved", "OK");
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