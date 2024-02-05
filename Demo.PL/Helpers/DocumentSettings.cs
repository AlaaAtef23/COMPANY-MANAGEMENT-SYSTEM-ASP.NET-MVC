using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Reflection.Metadata;

namespace Demo.PL.Helpers
{
    public class DocumentSettings
    {
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. get located folder path
            //string folderPath = "G:\\course\\back\\07 ASP.NET Core MVC\\Session 03\\Assignments\\Demo.PL\\Demo.PL\\wwwroot\\files\\" + folderName; 
            //string folderPath = Directory.GetCurrentDirectory() + @"\wwwroot\files\" + folderName;
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files", folderName);

            // 2. get file name and make it unique
            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3. get file path
            string filePath=Path.Combine(folderPath, fileName);

            // 4. save file as streams[ Data per time]
            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName; 

        }
        
        public static void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\files",folderName,fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
            
        }
    }
}
