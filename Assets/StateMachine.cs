using UnityEngine;

// State Machines are responsible for processing states, notifying them when they're about to begin or conclude, etc.
public class StateMachine
{
    private State _current_state;

    public void ChangeState(State new_state)
    {
        if (_current_state != null)
        {
            _current_state.OnFinish();
        }

        _current_state = new_state;
        // States sometimes need to reset their machine. 
        // This reference makes that possible.
        _current_state.state_machine = this;
        _current_state.OnStart();
    }

    public void Reset()
    {
        if (_current_state != null)
            _current_state.OnFinish();
        _current_state = null;
    }

    public void Update()
    {
        if (_current_state != null)
        {
            float time_delta_fraction = Time.deltaTime / (1.0f / Application.targetFrameRate);
            _current_state.OnUpdate(time_delta_fraction);
        }
    }

    public bool IsFinished()
    {
        return _current_state == null;
    }
}

// A State is merely a bundle of behavior listening to specific events, such as...
// OnUpdate -- Fired every frame of the game.
// OnStart -- Fired once when the state is transitioned to.
// OnFinish -- Fired as the state concludes.
// State Constructors often store data that will be used during the execution of the State.
public class State
{
    // A reference to the State Machine processing the state.
    public StateMachine state_machine;

    public virtual void OnStart() { }
    public virtual void OnUpdate(float time_delta_fraction) { } // time_delta_fraction is a float near 1.0 indicating how much more / less time this frame took than expected.
    public virtual void OnFinish() { }

    // States may call ConcludeState on themselves to end their processing.
    public void ConcludeState() { state_machine.Reset(); }
}


public class NextTurn : State
{

}







