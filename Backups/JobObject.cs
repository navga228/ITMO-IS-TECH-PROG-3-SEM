using System;
using System.Collections.Generic;

namespace Backups
{
    [Serializable]
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