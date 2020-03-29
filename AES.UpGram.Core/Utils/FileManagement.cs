using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace AES.UpGram.Core.Utils
{
    public static class FileManagement
    {
        public static void WriteToBinaryFile<T>(string filePath, T objectToWrite, bool append = false)
        {
            using var stream = File.Open(filePath, append ? FileMode.Append : FileMode.Create, FileAccess.ReadWrite);
            
            new BinaryFormatter().Serialize(stream, objectToWrite);
        }

        public static T ReadFromBinaryFile<T>(string filePath)
        {
            using Stream stream = File.Open(filePath, FileMode.Open);
            
            return (T)new BinaryFormatter().Deserialize(stream);
        }

        public static bool FromFile(string path)
        {
            return File.Exists(path);
        }

        public static void CreatePath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
