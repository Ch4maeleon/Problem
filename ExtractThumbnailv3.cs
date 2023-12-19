using System;
using System.IO;

namespace Problem
{
    // c#의 tuple 공부하기
    // IDisposable protocol 공부하기

    public class Result
    {
        public bool Result { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
    }

    public class Jpeg
    {
        public readonly byte[] SignatureHeader = { 0xFF, 0xD8, 0xFF };
        public readonly byte[] SignatureFooter = { 0xFF, 0xD9 };

        public string InputFilePath;

        public Jpeg(string inputFilePath)
        {
            // TODO
            // check path
            InputFilePath = inputFilePath;
        }

        public Result FindThumbnail()
        {
            if (File.Exists(InputFilePath))
                return new Result { Result = false, Start = -1, End = -1 };

            using (var inFile = new FileStream(InputFilePath, FileMode.Open))
            {
                var buffer = new byte[inFile.Length];
                inFile.Read(buffer, 0, buffer.Length);

                var thumbnailHeaderIndex = -1;

                for (var i = 3; i < buffer.Length; i++)
                    if (buffer[i] == 0xFF && buffer[i + 1] == 0xD8 && buffer[i + 2] == 0xFF)
                    {
                        thumbnailHeaderIndex = i;
                        break;
                    }

                var thumbnailFooterIndex = -1;

                for (var i = buffer.Length - 2; i > 0; i--)
                    if (buffer[i - 1] == 0xFF && buffer[i] == 0xD9)
                    {
                        thumbnailFooterIndex = i;
                        break;
                    }

                return new Result {
                    Result = thumbnailHeaderIndex != -1 && thumbnailFooterIndex != -1, 
                    Start = thumbnailHeaderIndex, 
                    End = thumbnailFooterIndex
                };
            }
        }

        public void SaveTo(string outputFilePath, Result result)
        {
            using (var inFile = new FileStream(InputFilePath, FileMode.Open))
            using (var outFile = new FileStream(outputFilePath, FileMode.Create))
            {
                var buffer = new byte[inFile.Length];
                inFile.Read(buffer, 0, buffer.Length);

                outFile.Write(buffer, result.Start, result.End - result.Start + 2);
            }
        }
    }


    internal class ExtractThumbnail
    {
        static void Main(string[] args)
        {
            string inputFilePath = @"C:\Users\user\Desktop\input.jpg";
            string outputFilePath = @"C:\Users\user\Desktop\output.jpg";

            var jpeg = new Jpeg(inputFilePath);
            var result = jpeg.FindThumbnail();

            if (result.Result)
            {
                jpeg.SaveTo(outputFilePath, result);
                Console.WriteLine("SUCCESS");
            }
            else
                Console.WriteLine("FAIL");
        }
    }
}
