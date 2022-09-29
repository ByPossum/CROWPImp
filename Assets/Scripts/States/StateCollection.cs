using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCollection
{
    private List<State> sL_states = new List<State>();

    public StateCollection()
    {

    }

    /// <summary>
    /// Add a new state to the list of states. Must be unique!
    /// </summary>
    /// <param name="_newState">The state to add</param>
    public void AddState(State _newState)
    {
        if (sL_states.Contains(_newState))
        {
            Debug.LogError("SE001: State you are trying to add is already in collection.");
            return;
        }
        sL_states.Add(_newState);
    }

    public void AddStates(State[] _newStates)
    {
        foreach(State _newState in _newStates)
        {
            AddState(_newState);
        }
    }

    public void RemoveState(State _toRemove)
    {
        if (sL_states.Contains(_toRemove))
        {
            sL_states.Remove(_toRemove);
            return;
        }
        Debug.LogError("SE002: State you are trying to remove isn't in the collection!");
    }

    public State GetState(string _name)
    {
        foreach (State _state in sL_states)
            if (_state.Name == _name)
                return _state;
        Debug.LogError("SE003: The state you're trying to get is not in the list.");
        return null;
    }

    public List<State> GetStates()
    {
        return sL_states;
    }

    public static bool operator ==(StateCollection _lhStates, StateCollection _rhStates)
    {
        // If State Collections have 0 common nodes, they are (for all intents and purposes) equal?
        foreach (State _state in _lhStates.GetStates())
        {
            State rhState = _rhStates.GetState(_state.Name);
            if(rhState is object)
            {
                if (_state != rhState)
                    return false;
            }
        }
        return true;
    }
    public static bool operator !=(StateCollection _lhStates, StateCollection _rhStates)
    {
        return !(_lhStates == _rhStates);
    }

    public override bool Equals(object obj)
    {
        return this == (StateCollection)obj;
    }

    public static int operator -(StateCollection _lhStates, StateCollection _rhStates)
    {
        int difference = 0;
        foreach(State _state in _lhStates.GetStates())
        {
            State _rhState = _rhStates.GetState(_state.Name);
            if(_rhState is object)
                if (_state.GetState() != _rhState.GetState())
                    difference++;
        }
        return difference;
    }
}
