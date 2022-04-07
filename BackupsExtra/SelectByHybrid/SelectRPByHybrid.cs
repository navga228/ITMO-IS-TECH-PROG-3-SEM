using System;
using System.Collections.Generic;
using System.Linq;
using Backups;

namespace BackupsExtra
{
    public enum HybridCondition
    {
        /// <summary>strict Condition</summary>
        StrictCondition,

        /// <summary>non-strict condition</summary>
        NonStrictCondition,
    }

    [Serializable]
    public class SelectRPByHybrid
    {
        private HybridCondition _condition;
        public SelectRPByHybrid(HybridCondition hybridCondition)
        {
            _condition = hybridCondition;
        }

        public void SetCondition(HybridCondition newHybridCondition)
        {
            _condition = newHybridCondition;
        }

        public List<RestorePoint> Select(BackupJobExtra backupJobExtra, List<ISelectRPMethod> selectRpMethods)
        { // Строгий метод и нестрогий. В нестрогом мы делаем пересечение точек, а в строгом их объединение.
            List<RestorePoint> restorePointsToDelete = selectRpMethods[0].Select(backupJobExtra);
            selectRpMethods.Remove(selectRpMethods[0]);
            if (_condition == HybridCondition.StrictCondition)
            {
                foreach (var selectMethod in selectRpMethods)
                {
                    restorePointsToDelete.Union(selectMethod.Select(backupJobExtra));
                }
            }
            else
            {
                foreach (var selectMethod in selectRpMethods)
                {
                    restorePointsToDelete.Intersect(selectMethod.Select(backupJobExtra));
                }
            }

            return restorePointsToDelete;
        }
    }
}