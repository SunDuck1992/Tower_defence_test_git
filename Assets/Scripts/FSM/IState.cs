using System;
public interface IState : IDisposable
{
    public void Enter();
    public void Update();
    public void Exit();
}
