using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Backups
{
    [Serializable]
    public class RestorePoint
    {
      public RestorePoint(string name, DateTime dateTimeCreation, List<JobObject> bachupedFiles)
      {
          Name = name;
          DateOfCreation = dateTimeCreation;
          BachupedFiles = bachupedFiles;
      }

      public string Name { get; }
      public DateTime DateOfCreation { get; }
      public List<JobObject> BachupedFiles { get; }
    }
}