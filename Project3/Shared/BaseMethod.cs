using System.Xml.Linq;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.VisualBasic.FileIO;
using Project3.Dtos;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using DeviceDetectorNET;
using DeviceDetectorNET.Parser;
using DeviceDetectorNET.Cache;

namespace Project3.Shared
{


    public static class BaseMethod
    { 
        public static async Task<UpImageResult> UploadImage(IFormFile file)
        {
            string[] Types = { ".jpg", ".png", ".jpeg", ".svg" };
            try
            {
                string FileNameConvert = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() + new Random().Next();
                var FileType = Path.GetExtension(file.FileName);
                if (!Types.Contains(FileType))
                {
                    return new UpImageResult() { Result = false, Text = BaseText.TypeImageError };
                }

                var FileURL = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", $"{FileNameConvert}{FileType}");
                using (var stream = new FileStream(FileURL, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return new UpImageResult() { Result = true, Text = FileNameConvert + FileType };
            }
            catch (Exception ex)
            {
                return new UpImageResult() { Result = false, Text = BaseText.Error };
            }

        }

        public static bool DeleteFile(string Name)
        {
            if (Name == "default.jpg") {
                return true;
            }
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", $"{Name}");
            FileInfo fileInfo = new(path);

            if (fileInfo.Exists)
            {
                fileInfo.Delete();
                return true;
            }
            return false;
        }

        public  static double ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }

        public static void ConvertTempData(ITempDataDictionary tempData, ViewDataDictionary viewData, string key)
        {
            if (tempData.ContainsKey(key))
            {
                viewData[key] = tempData[key];
            }
        }

       
    }
    
}