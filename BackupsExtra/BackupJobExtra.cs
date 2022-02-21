using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups;
using BackupsExtra.Tools;

namespace BackupsExtra
{
    [Serializable]
    public class BackupJobExtra
    {
        private BackupJob _backupJob;
        private IRepositoryExtra _repositoryExtra;
        private ILog _logger;
        private ISelectRPMethod _selectRpMethod;
        private IDeleteRPMethod _deleteRPMethod;
        public BackupJobExtra(string name, string projectPath, IBackupAlgorithmExtra backupAlgorithmExtra, IRepositoryExtra repositoryExtra, ILog logger, ISelectRPMethod selectRpMethod, IDeleteRPMethod deleteRpMethod)
        {
            BackupJobExtra backupJobExtra = repositoryExtra.GetData(name);
            if (backupJobExtra != null)
            { // Если джоба существовала, то восстанавливаем ее состояние
                _backupJob = backupJobExtra._backupJob;
                _repositoryExtra = backupJobExtra._repositoryExtra;
                _logger = backupJobExtra._logger;
                _selectRpMethod = backupJobExtra._selectRpMethod;
                _deleteRPMethod = backupJobExtra._deleteRPMethod;
                _logger.Print($"{InfoAboutClass()} Message: BackupJobExtra was successfully recovered!");
            }
            else
            { // если джобы не было, то создаем заново
                if (string.IsNullOrEmpty(name))
                {
                    throw new BackupsExtraException("Name is null or empty");
                }

                if (projectPath == null)
                {
                    throw new BackupsExtraException("Project path is null");
                }

                if (repositoryExtra == null)
                {
                    throw new BackupsExtraException("Repository is null");
                }

                if (backupAlgorithmExtra == null)
                {
                    throw new BackupsExtraException("Backup Algorithm is null");
                }

                if (repositoryExtra == null)
                {
                    throw new BackupsExtraException("RepositoryExtra is null");
                }

                if (logger == null)
                {
                    throw new BackupsExtraException("Logger is null");
                }

                if (selectRpMethod == null)
                {
                    throw new BackupsExtraException("selectSelectRpMethod is null");
                }

                if (deleteRpMethod == null)
                {
                    throw new BackupsExtraException("deleteRpMethod is null");
                }

                _backupJob = new BackupJob(name, projectPath, repositoryExtra, backupAlgorithmExtra);
                _repositoryExtra = repositoryExtra;
                _logger = logger;
                _selectRpMethod = selectRpMethod;
                _deleteRPMethod = deleteRpMethod;
                _repositoryExtra.SaveData(this);
                _logger.Print($"{InfoAboutClass()} Message: BackupJobExtra was successfully created!");
            }
        }

        public BackupJob GetBackupJob
        {
            get => _backupJob;
        }

        public void AddJobObject(JobObject jobObject)
        {
            _backupJob.AddJobObject(jobObject);
            _repositoryExtra.SaveData(this);
            _logger.Print($"{InfoAboutClass()} Message: JobObject was successfully added!");
        }

        public void RemoveJobObject(JobObject jobObject)
        {
            _backupJob.RemoveJobObject(jobObject);
        }

        public void SetRPSelectMethod(ISelectRPMethod selectRpMethod)
        {
            _selectRpMethod = selectRpMethod ?? throw new BackupsExtraException("selectRpMethod is null");

            _selectRpMethod = selectRpMethod;
            _repositoryExtra.SaveData(this);
            _logger.Print($"{InfoAboutClass()} Message: selectRpMethod was successfully changed!");
        }

        public void SetDeleteRPMethod(IDeleteRPMethod deleteRpMethod)
        {
            _deleteRPMethod = deleteRpMethod ?? throw new BackupsExtraException("deleteRpMethod is null");

            _deleteRPMethod = deleteRpMethod;
            _repositoryExtra.SaveData(this);
            _logger.Print($"{InfoAboutClass()} Message: deleteRpMethod was successfully changed!");
        }

        public void BackupProcessing(string newRestorePointName)
        {
            if (string.IsNullOrEmpty(newRestorePointName))
            {
                throw new BackupsExtraException("newRestorePointName is null or empty");
            }

            _backupJob.BackupProcessing(newRestorePointName);
            _logger.Print($"{InfoAboutClass()} Message: Restore point was successfully created!");
            List<RestorePoint> selectedRestorePoints = _selectRpMethod?.Select(this);
            _deleteRPMethod?.Delete(selectedRestorePoints, this); // Поиск неподходящих по лимиту рп и их удаление выбраным способом
            _repositoryExtra.SaveData(this);
            if (selectedRestorePoints.Any()) _logger.Print($"{InfoAboutClass()} Message: Restore points was successfully deleted by limits!");
        }

        public void DeleteRestorePoint(string restorePointName)
        {
            if (string.IsNullOrEmpty(restorePointName))
            {
                throw new BackupsExtraException("restorePointName is null or empty");
            }

            foreach (var restorePoint in _backupJob.RestorePoints.Where(restorePoint => restorePoint.Name.Equals(restorePointName)))
            {
                _backupJob.RestorePoints.Remove(restorePoint);
                _repositoryExtra.DeleteRestorePoints(this, restorePoint);
                _repositoryExtra.SaveData(this);
                _logger.Print($"{InfoAboutClass()} Message: Restore points was successfully deleted!");
                break;
            }
        }

        private string InfoAboutClass()
        {
            return $"Object name: {this.GetType().Name}";
        }
    }
}