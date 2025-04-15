using System;

public interface IStateMachine 
{
    public IState CurrentState {  get; }
    public void SwitchState<T, Owner>(Owner owner, Action<T> callback = null)
         where T : BaseState<Owner>, new()
        where Owner : class, IStateMachineOwner;
    public void UpdateState();
}
