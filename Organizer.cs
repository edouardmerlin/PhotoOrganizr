using ExifLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoOrganizr
{
    public class Organizer
    {
        public static int CountNumberOfJpgFiles(string path)
        {

            return GetFiles(path).Count();
        }

        private static string[] GetFiles(string path)
        {
            return Directory.GetFiles(path).Where(x => x.ToLower().EndsWith(".jpg")).ToArray();
        }


        public static string ReadExif(string pathSource, string pathDestination)
        {
            int numberOfFilesMooved = 0;
            int numberOfFilesNotMooved = 0;
            string[] files = GetFiles(pathSource);
            StringBuilder sb = new StringBuilder();
            foreach (string file in files)
            {
                try
                {
                    DateTime dateTaken = DateTime.MinValue;
                    string make = "", model = "", next = "";
                    object obj = null;
                    using (ExifReader reader = new ExifReader(file))
                    {
                        reader.GetTagValue(ExifTags.DateTimeOriginal, out dateTaken);
                        reader.GetTagValue(ExifTags.Make, out make);
                        reader.GetTagValue(ExifTags.Model, out model);

                        sb.AppendLine($"{file.Split('\\').Last()} - model:[{model}], make:[{make}] was taken on: {dateTaken.ToString("yyyyddMM-HHmmss-fff", CultureInfo.InvariantCulture)}");
                        //foreach (ExifTags exift in Enum.GetValues(typeof(ExifTags)))
                        //{
                        //    reader.GetTagValue(exift, out obj);
                        //    if (obj != null)
                        //        sb.AppendLine($"{exift}:{obj};");

                        //}
                    }
                    var year = dateTaken.Year;
                    var month = dateTaken.Month;

                    DirectoryInfo dinfo = Directory.CreateDirectory($"{pathDestination}/{year.ToString().PadLeft(4, '0')}/{month.ToString().PadLeft(2, '0')}");
                    var destinationFile = dinfo.FullName + "\\" + dateTaken.ToString("yyyy-dd-MM-HH-mm-ss", CultureInfo.InvariantCulture);
                    string filePathDestination = destinationFile + ".jpg";
                    bool exist = File.Exists(destinationFile + ".jpg");
                    bool copy = true;
                    if (exist)
                    {
                        if (AreEquals(file, destinationFile + ".jpg"))
                        {
                            copy = false;
                            numberOfFilesNotMooved++;
                            sb.AppendLine($"#################### le fichier à destination semble identique: {destinationFile}.jpg");
                        }
                        else
                        {

                            //If already exist, add numbers at the end
                            int index = 0;
                            while (exist && copy)
                            {
                                filePathDestination = destinationFile + ++index + ".jpg";
                                exist = File.Exists(filePathDestination);
                                if (AreEquals(file, filePathDestination))
                                {
                                    copy = false;
                                    numberOfFilesNotMooved++;
                                    sb.AppendLine($"#################### le fichier à destination semble identique: {destinationFile}.jpg");
                                }
                            }
                        }
                    }

                    if (copy)
                    {
                        File.Copy(file, filePathDestination);
                        numberOfFilesMooved++;
                    }
                }
                catch (Exception ex)
                {
                    sb.AppendLine("ERROR :");
                    sb.AppendLine(ex.ToString());
                }
            }
            sb.AppendLine($"{numberOfFilesMooved} photos ont été rennomées et triées vers le dossier de destination.");
            sb.AppendLine($"{numberOfFilesNotMooved} photos existent deja dans le dossier de destination.");

            return sb.ToString();
        }

        private static bool AreEquals(string filepath1, string filepath2)
        {
            FileInfo fi1 = new FileInfo(filepath1);
            FileInfo fi2 = new FileInfo(filepath2);
            DateTime dateTaken1, dateTaken2;
            string make1, make2, model1, model2;
            object exposure1, exposure2,
                fNumber1, fNumber2,
                focalL1, focalL2,
                PixelXDimension1, PixelXDimension2,
                PixelYDimension1, PixelYDimension2,
                PhotographicSensitivity1, PhotographicSensitivity2;
            if (fi1.Length == fi2.Length)
            {
                using (ExifReader reader = new ExifReader(filepath1))
                {
                    reader.GetTagValue(ExifTags.DateTimeOriginal, out dateTaken1);
                    reader.GetTagValue(ExifTags.Make, out make1);
                    reader.GetTagValue(ExifTags.Model, out model1);
                    reader.GetTagValue(ExifTags.ExposureTime, out exposure1);
                    reader.GetTagValue(ExifTags.FNumber, out fNumber1);
                    reader.GetTagValue(ExifTags.FocalLength, out focalL1);
                    reader.GetTagValue(ExifTags.PixelXDimension, out PixelXDimension1);
                    reader.GetTagValue(ExifTags.PixelYDimension, out PixelYDimension1);
                    reader.GetTagValue(ExifTags.PhotographicSensitivity, out PhotographicSensitivity1);
                }

                using (ExifReader reader = new ExifReader(filepath2))
                {
                    reader.GetTagValue(ExifTags.DateTimeOriginal, out dateTaken2);
                    reader.GetTagValue(ExifTags.Make, out make2);
                    reader.GetTagValue(ExifTags.Model, out model2);
                    reader.GetTagValue(ExifTags.ExposureTime, out exposure2);
                    reader.GetTagValue(ExifTags.FNumber, out fNumber2);
                    reader.GetTagValue(ExifTags.FocalLength, out focalL2);
                    reader.GetTagValue(ExifTags.PixelXDimension, out PixelXDimension2);
                    reader.GetTagValue(ExifTags.PixelYDimension, out PixelYDimension2);
                    reader.GetTagValue(ExifTags.PhotographicSensitivity, out PhotographicSensitivity2);
                }

                if (dateTaken1 == dateTaken2
                    && make1 == make2
                    && model1 == model2
                    && exposure1?.ToString() == exposure2?.ToString()
                    && fNumber1?.ToString() == fNumber2?.ToString()
                    && focalL1?.ToString() == focalL2?.ToString()
                    && PixelXDimension1?.ToString() == PixelXDimension2?.ToString()
                    && PixelYDimension1?.ToString() == PixelYDimension2?.ToString()
                    && PhotographicSensitivity1?.ToString() == PhotographicSensitivity2?.ToString())
                {
                    return true;
                }
            }
            return false;
        }

    }
}

