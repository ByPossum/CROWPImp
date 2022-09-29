using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    private StateCollection sc_preCondition;
    private StateCollection sc_postCondition;
    private int i_HCost = 0;
    private int i_FCost = 0;


    private string s_actionName;
    public string ActionName { get { return s_actionName; } }
    public int HCost { get { return i_HCost; } }
    public int FCost { get { return i_FCost; } }
    public GoapAction ga_action;

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
        i_HCost = sc_postCondition - _worldState;
        return i_HCost;
    }

}

public delegate void GoapAction(GameObject _target);