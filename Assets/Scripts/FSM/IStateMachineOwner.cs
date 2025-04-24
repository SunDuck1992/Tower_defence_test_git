namespace StateSpace
{
    public interface IStateMachineOwner
    {
        public IStateMachine StateMachine { get; }
    }
}