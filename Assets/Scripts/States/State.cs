using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;

[XmlRoot("State")]
public class State
{
    [XmlAttribute("Name")]
    public string s_name;
    [XmlAttribute("CurrentState")]
    public bool b_state;
    public string Name { get { return s_name; } }
    public virtual bool GetState { get { return b_state; } }

    public State() { }
    public State(string _name, bool _initialState = false)
    {
        s_name = _name;
        b_state = _initialState;
    }

    public State(State _clone)
    {
        s_name = _clone.s_name;
        b_state = _clone.b_state;
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