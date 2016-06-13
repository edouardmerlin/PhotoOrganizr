using ExifLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrganizr
{
   public class Organizer
    {
        public static int CountNumberOfJpgFiles(string path) {

            return GetFiles(path).Count();
        }

        private static string[] GetFiles(string path)
        {
            return Directory.GetFiles(path).Where(x => x.ToLower().EndsWith(".jpg")).ToArray();
        }


        public static string  ReadExif(string path) {

            string[] files = GetFiles(path);
            StringBuilder sb = new StringBuilder();
            foreach(string file in files)
            {
                try
                {
                    using (ExifReader reader = new ExifReader(file))
                    {
                        DateTime dateTaken = DateTime.MinValue;
                        reader.GetTagValue(ExifTags.DateTimeOriginal, out dateTaken);
                        sb.AppendLine($"{file} was taken on: {dateTaken}");
                    }
                }
                catch (Exception)
                {
                    sb.AppendLine($"«Could not read exif for : {file}");
                }
                         }
            return sb.ToString();
        }
    }
}
