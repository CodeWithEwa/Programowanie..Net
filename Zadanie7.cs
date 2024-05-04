using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        string sourceFilePath = "sourceFile.txt";
        GenerateLargeFile(sourceFilePath, 300);

        TestFileCopy(sourceFilePath, "File.Copy");
        TestFileStream(sourceFilePath, "FileStream");
        TestBufferedStream(sourceFilePath, "BufferedStream");
        TestBinaryReaderWriter(sourceFilePath, "BinaryReader/BinaryWriter");
    }

    static void GenerateLargeFile(string filePath, int sizeInMB)
    {
        byte[] buffer = new byte[1024 * 1024];
        using (FileStream fs = new FileStream(filePath, FileMode.Create))
        {
            for (int i = 0; i < sizeInMB; i++)
            {
                fs.Write(buffer, 0, buffer.Length);
            }
        }
    }

    static void TestFileCopy(string sourcePath, string methodName)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        string destinationPath = "destinationFile_Copy.txt";
        File.Copy(sourcePath, destinationPath);
        stopwatch.Stop();
        Console.WriteLine($"{methodName}: {stopwatch.ElapsedMilliseconds} ms");
    }

    static void TestFileStream(string sourcePath, string methodName)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        string destinationPath = "destinationFile_FileStream.txt";
        using (FileStream sourceStream = new FileStream(sourcePath, FileMode.Open))
        using (FileStream destinationStream = new FileStream(destinationPath, FileMode.Create))
        {
            byte[] buffer = new byte[1024 * 1024];
            int bytesRead;
            while ((bytesRead = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                destinationStream.Write(buffer, 0, bytesRead);
            }
        }
        stopwatch.Stop();
        Console.WriteLine($"{methodName}: {stopwatch.ElapsedMilliseconds} ms");
    }

    static void TestBufferedStream(string sourcePath, string methodName)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        string destinationPath = "destinationFile_BufferedStream.txt";
        using (FileStream sourceStream = new FileStream(sourcePath, FileMode.Open))
        using (FileStream destinationStream = new FileStream(destinationPath, FileMode.Create))
        using (BufferedStream bufferedStream = new BufferedStream(sourceStream))
        {
            byte[] buffer = new byte[1024 * 1024];
            int bytesRead;
            while ((bytesRead = bufferedStream.Read(buffer, 0, buffer.Length)) > 0)
            {
                destinationStream.Write(buffer, 0, bytesRead);
            }
        }
        stopwatch.Stop();
        Console.WriteLine($"{methodName}: {stopwatch.ElapsedMilliseconds} ms");
    }

    static void TestBinaryReaderWriter(string sourcePath, string methodName)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        string destinationPath = "destinationFile_BinaryReaderWriter.txt";
        using (BinaryReader reader = new BinaryReader(File.OpenRead(sourcePath)))
        using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(destinationPath)))
        {
            byte[] buffer = new byte[1024 * 1024];
            int bytesRead;
            while ((bytesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
            {
                writer.Write(buffer, 0, bytesRead);
            }
        }
        stopwatch.Stop();
        Console.WriteLine($"{methodName}: {stopwatch.ElapsedMilliseconds} ms");
    }
}
