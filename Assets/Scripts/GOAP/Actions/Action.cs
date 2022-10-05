using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[XmlRoot("Action")]
public class Action
{
    [XmlAttribute("ActionName")]
    public string s_actionName;
    [XmlElement("Precondition")]
    public StateCollection sc_preCondition;
    [XmlElement("PostCondition")]
    public StateCollection sc_postCondition;
    
    private GoapAction ga_action;
    private int i_hCost = 0;
    private int i_gCost = 0;
    private bool b_validAction = true;
    public string ActionName { get { return s_actionName; } }
    public int HCost { get { return i_hCost; } }
    public int FCost { get { return i_hCost + i_gCost; } }
    public StateCollection PreCondition { get { return sc_preCondition; } }
    public StateCollection PostCondition { get { return sc_postCondition; } }

    public Action() { }

    public Action(string _actionName, GoapAction _action, State[] _pre, State[] _post)
    {
        sc_preCondition = AssignStateCollection(_pre);
        sc_postCondition = AssignStateCollection(_post);
        s_actionName = _actionName;
        ga_action = _action;
    }

    private StateCollection AssignStateCollection(State[] _states)
    {
        StateCollection newCollection = new StateCollection();
        for (int i = 0; i < _states.Length; i++)
            newCollection.AddState(_states[i]);
        return newCollection;
    }

    public bool CheckActionEligibility(StateCollection _worldState)
    {
        return sc_preCondition == _worldState;
    }

    public void CalculateHCost(StateCollection _goalState)
    {

        i_hCost = _goalState.GetStates().Count - (sc_postCondition - _goalState);
    }

    public void CalculateGCost(StateCollection _previousState)
    {
        i_gCost = sc_preCondition - _previousState;
    }

    public bool CheckIfNodeIsTraversable(StateCollection _worldState)
    {
        return sc_preCondition == _worldState;
    }
}

public delegate void GoapAction(GameObject _target);