using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tizen.System;

namespace swiftPainReport
{
    internal class PainDataManager
    {
        private readonly string csvFilePath;
        private const string CSV_HEADER = "date,time,area,pain_score";

        public PainDataManager()
        {
            csvFilePath = InitializeCsvFilePath();
            EnsureCsvFileExists();
        }

        private string InitializeCsvFilePath()
        {
            var storages = StorageManager.Storages;
            var internalStorage = storages.Where(s => s.StorageType == StorageArea.Internal).FirstOrDefault();

            if (internalStorage == null)
                throw new InvalidOperationException("Internal storage not found");

            string documentsPath = internalStorage.GetAbsolutePath(DirectoryType.Documents);
            string painDataDir = Path.Combine(documentsPath, "PainData");

            if (!Directory.Exists(painDataDir))
            {
                Directory.CreateDirectory(painDataDir);
            }

            return Path.Combine(painDataDir, "pain_record.csv");
        }

        private void EnsureCsvFileExists()
        {
            if (!File.Exists(csvFilePath))
            {
                using (StreamWriter sw = File.CreateText(csvFilePath))
                {
                    sw.WriteLine(CSV_HEADER);
                }
                Tizen.Log.Info("painReport3", $"Created new CSV file at: {csvFilePath}");
            }
        }

        public bool SavePainRecord(string csvLine)
        {
            try
            {
                var storages = StorageManager.Storages;
                var internalStorage = storages.Where(s => s.StorageType == StorageArea.Internal).FirstOrDefault();

                if (internalStorage == null || internalStorage.State != StorageState.Mounted)
                    throw new InvalidOperationException("Storage is not accessible");

                if (internalStorage.AvailableSpace < 1024 * 1024)
                    throw new InvalidOperationException("Insufficient storage space");

                using (StreamWriter sw = File.AppendText(csvFilePath))
                {
                    sw.WriteLine(csvLine);
                }

                Tizen.Log.Info("painReport3", $"Pain record saved successfully: {csvLine}");
                return true;
            }
            catch (Exception ex)
            {
                Tizen.Log.Error("painReport3", $"Error saving pain record: {ex.Message}");
                return false;
            }
        }

        public string FormatCsvLine(int area, int painLevel)
        {
            string timestampDate = DateTime.Now.ToString("yyyy-MM-dd");
            string timestampTime = DateTime.Now.ToString("HH:mm:ss");
            return $"{timestampDate},{timestampTime},{area},{painLevel}";
        }
    }
}