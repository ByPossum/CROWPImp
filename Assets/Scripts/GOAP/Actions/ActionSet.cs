using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
[XmlRoot("ActionSet")]
public class ActionSet
{
    private List<Action> aL_actionSet = new List<Action>();
    public List<Action> GetActionSet { get { return aL_actionSet; } }
    public bool IsEmpty { get { return aL_actionSet.Count <= 0; } }
    public ActionSet() { }

    public ActionSet(params Action[] _set)
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

    public Action GetLowestCostAction()
    {
        // Assign first node
        Action _lowestCost = aL_actionSet[0];
        // Find the lowest cost node (O(n))
        for (int i = 1; i < aL_actionSet.Count; i++)
        {
            if (aL_actionSet[i].FCost < _lowestCost.FCost)
                _lowestCost = aL_actionSet[i];
        }
        // Return lowest cost node
        return _lowestCost;
    }

    public void AddAction(Action _actionToAdd)
    {
        if (aL_actionSet.Contains(_actionToAdd))
        {
            Debug.LogError("AE001: Action you are trying to Add is already in this ActionSet");
            return;
        }
        aL_actionSet.Add(_actionToAdd);
    }

    public Action RemoveAction(Action _actionToRemove)
    {
        Action _removedAction = GetAction(_actionToRemove);
        if (_removedAction == null)
            return null;
        aL_actionSet.Remove(_removedAction);
        return _removedAction;
    }

    public Action GetAction(string _actionName)
    {
        for (int i = 0; i < aL_actionSet.Count; i++)
        {
            if (aL_actionSet[i].ActionName == _actionName)
                return aL_actionSet[i];
        }
        return null;
    }

    public Action GetAction(Action _action)
    {
        if (!aL_actionSet.Contains(_action))
            return null;
        return GetAction(_action.ActionName);
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
