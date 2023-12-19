using System;
using System.IO;

namespace Problem
{
    public class Jpeg
    {
        public readonly byte[] signatureHeader = { 0xFF, 0xD8, 0xFF };
        public readonly byte[] signatureFooter = { 0xFF, 0xD9 };

        public Jpeg()
        {

        }

        private int start = -1;
        private int end = -1;

        public void ExtractThumbnail(string inputFilePath, string outputFilePath)
        {
            start = -1;
            end = -1;

            using (var inFile = new FileStream(inputFilePath, FileMode.Open))
            {
                var buffer = new byte[inFile.Length];
                inFile.Read(buffer, 0, buffer.Length);

                Console.WriteLine(buffer[0]);
                Console.WriteLine(buffer[1]);
                Console.WriteLine(buffer[2]);

                Console.WriteLine();

                Console.WriteLine(buffer[buffer.Length - 2]);
                Console.WriteLine(buffer[buffer.Length - 1]);

                Console.WriteLine();

                // 썸네일 시작 인덱스 찾기
                var thumbnailHeaderIndex = -1;

                for (var i = 3; i < buffer.Length; i++)
                    if (buffer[i] == 0xFF && buffer[i + 1] == 0xD8 && buffer[i + 2] == 0xFF)
                    {
                        thumbnailHeaderIndex = i;
                        break;
                    }

                Console.WriteLine(thumbnailHeaderIndex);

                // 썸네일 끝 인덱스 찾기
                var thumbnailFooterIndex = -1;

                for (var i = buffer.Length - 2; i > 0; i--)
                    if (buffer[i - 1] == 0xFF && buffer[i] == 0xD9)
                        thumbnailFooterIndex = i;

                Console.WriteLine(thumbnailFooterIndex);

                using (var outFile = new FileStream(outputFilePath, FileMode.Create))
                {
                    outFile.Write(buffer, thumbnailHeaderIndex, thumbnailFooterIndex - thumbnailHeaderIndex + 2);
                }
            }
        }

    }

    class Result
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

            Jpeg jpeg = new Jpeg();

            jpeg.ExtractThumbnail(inputFilePath, outputFilePath);

            // 주어진 사진에서, 썸네일에 해당하는 영역을 찾아서 저장하는 프로그램을 작성하시오
            // Given Jpeg picture, wwite a program that find out thumbnail and save to disk

            /*
               
                var buffer = File.ReadAll(path);
                var found, s, e = Thumbnail.Find(byte[] buffer);
                File.Save(buffer, to: path);

                // Given Jpeg picture, wwite a program that find out thumbnail and save to disk
                var jpeg = new Jpeg(string path)
                var result = jpeg.FindThumbnail(); // bool
                jpeg.SaveTo(outPath);

             */
        }
    }
}
