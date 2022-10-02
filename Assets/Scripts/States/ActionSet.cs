using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
[XmlRoot("ActionSet")]
public class ActionSet
{
    private List<Action> aL_actionSet = new List<Action>();
    public List<Action> GetActionSet { get { return aL_actionSet; } }

    public ActionSet() { }

    public ActionSet(Action[] _set)
    {
        for (int i = 0; i < _set.Length; i++)
            aL_actionSet.Add(_set[i]);
    }

    public int GetActionSetFCost()
    {
        int _cost = 0;
        for (int i = 0; i < aL_actionSet.Count; i++)
            _cost += aL_actionSet[i].FCost;
        return _cost;
    }

    public bool Contains(Action _action)
    {
        return aL_actionSet.Contains(_action);
    }

    public bool Contains(string _actionName)
    {
        for (int i = 0; i < aL_actionSet.Count; i++)
        {
            if (aL_actionSet[i].ActionName == _actionName)
                return true;
        }
        return false;
    }

}
