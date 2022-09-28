using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

public class State
{
    protected string s_name;
    protected bool b_state = false;
    public string Name { get { return s_name; } }
    public State(string _name, bool _initialState = false)
    {
        s_name = _name;
        b_state = _initialState;
    }

    public virtual bool GetState()
    {
        return b_state;
    }

    public virtual bool CheckState()
    {
        return b_state;
    }

    public virtual void FlipState()
    {
        b_state = !b_state;
    }

    public virtual void UpdateState(bool _newVal)
    {
        b_state = _newVal;
    }

    public static bool operator ==(State _lhState, State _rhState)
    {
        return _lhState.b_state == _rhState.b_state;
    }

    public static bool operator !=(State _lhState, State _rhState)
    {
        return _lhState.b_state != _rhState.b_state;
    }
}