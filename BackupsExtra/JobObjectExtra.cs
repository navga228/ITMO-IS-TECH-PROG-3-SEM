using System;
using Backups;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    [Serializable]
    public class JobObjectExtra
    {
        private JobObject _jobObject;
        public JobObjectExtra(string filePath, string name)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new BackupsExtraException("File path is null or empty!");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new BackupsExtraException("Name is null or empty!");
            }

            _jobObject = new JobObject(filePath, name);
        }
    }
}