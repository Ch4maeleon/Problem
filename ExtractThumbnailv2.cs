using System;
using System.IO;

namespace Problem
{
    public class Jpeg
    {
        public readonly byte[] SignatureHeader = { 0xFF, 0xD8, 0xFF };
        public readonly byte[] SignatureFooter = { 0xFF, 0xD9 };

        public string InputFilePath;

        public Jpeg(string inputFilePath)
        {
            InputFilePath = inputFilePath;
        }

        public Result FindThumbnail()
        {
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
                return new Result
                {
                    result = thumbnailHeaderIndex != -1 && thumbnailFooterIndex != -1,
                    start = thumbnailHeaderIndex,
                    end = thumbnailFooterIndex
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

                outFile.Write(buffer, result.start, result.end - result.start + 2);
            }
        }
    }

    public class Result
    {
        public bool result;
        public int start;
        public int end;
    }

    internal class ExtractThumbnail
    {
        static void Main(string[] args)
        {
            string inputFilePath = @"C:\Users\user\Desktop\input.jpg";
            string outputFilePath = @"C:\Users\user\Desktop\output.jpg";

            // 주어진 사진에서, 썸네일에 해당하는 영역을 찾아서 저장하는 프로그램을 작성하시오
            // Given Jpeg picture, wwite a program that find out thumbnail and save to disk

            /*
               
                var buffer = File.ReadAll(path);
                var found, s, e = Thumbnail.Find(byte[] buffer);
                File.Save(buffer, to: path);

             */
        }
    }
}