using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Banks
{
    public class OperationManager
    {
        private static OperationManager _instance;
        static OperationManager()
        {
            _instance = new OperationManager();
        }

        public Collection<IOperation> Transactions { get; set; } = new Collection<IOperation>();
        public void AddOperation(IOperation operation)
        {
            Transactions.Add(operation);
        }

        public void ProcessOperations()
        {
            Transactions.Where(oper => !oper.IsComplete).ToList().ForEach(oper => oper.Execute());
        }

        public void CancelOperation(string id)
        {
            IOperation operation = Transactions.Where(oper => oper.Id == id).FirstOrDefault();
            if (operation != null)
            {
                operation.CancelOperation();
                Transactions.Remove(operation);
            }
        }
    }
}