using System.Collections.Generic;

namespace Backups
{
    public class JobObject
    {
        // Файл которыq будут бэкапится
        public JobObject(string filePath, string name)
        {
            FilePath = filePath + name;
            Name = name;
        }

        public string FilePath { get; }
        public string Name { get; }
    }
}