using UnityEngine;

public interface IState<in TOwner>
{
    public void Enter(TOwner owner);
    public void Exit();
    public void Update();
    public void HandleEvent(StateEvent stateEvent);
}

public class StateMachine<TOwner> where TOwner : MonoBehaviour
{
    IState<TOwner> _curState;

    public void ChangeState(IState<TOwner> nextState, TOwner owner)
    {
        _curState?.Exit();
        _curState = nextState;
        _curState.Enter(owner);
    }

    public void Update()
    {
        _curState.Update();
    }

    public void GenerateStateEvent(StateEvent stateEvent)
    {
        _curState.HandleEvent(stateEvent);
    }

    public void Reset()
    {
        _curState?.Exit();
        _curState = null;
    }
}