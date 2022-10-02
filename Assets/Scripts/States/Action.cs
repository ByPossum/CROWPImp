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
    public string ActionName { get { return s_actionName; } }
    public int HCost { get { return i_hCost; } }
    public int FCost { get { return i_hCost + i_gCost; } }

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

    public int CalculateHCost(StateCollection _worldState)
    {
        i_hCost = sc_postCondition - _worldState;
        return i_hCost;
    }

}

public delegate void GoapAction(GameObject _target);