namespace Banks
{
    public interface IOperation
    {
        public bool IsComplete { get; }
        public string Id { get; }
        void CancelOperation();
        void Execute();
    }
}