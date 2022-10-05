using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planner
{
    private Debugger bug_logger;
    public Planner(Debugger _bug)
    {
        bug_logger = _bug;
        bug_logger.AddTextCategory("Planner");
        bug_logger.AddText("Planner", "Status: Beginning");
    }

    public ActionSet Plan(ActionSet _initialSet, StateCollection _worldState, StateCollection _targetState)
    {
        bug_logger.UpdateText("Planner", "Status: Initializing plan");
        StateCollection world = new StateCollection(_worldState);
        ActionSet _openSet = new ActionSet(_initialSet.GetActionSet.ToArray());

        bool _planning = true;

        Action _currentNode = _openSet.RemoveAction(_openSet.GetLowestCostAction());
        ActionSet _closedSet = new ActionSet(_currentNode);

        while (_planning)
        {
            bug_logger.UpdateText("Planner", "Status: Planning");
            foreach (State _resultingState in _currentNode.PostCondition.GetStates())
                world.UpdateState(_resultingState);

            if (_targetState == world)
            {
                // Plan Satisfied.
                _planning = false;
                break;
            }
            if (_openSet.IsEmpty)
            {
                // Plan cannot be satisfied. Move onto a lower priority Goal
                _planning = false;
                _closedSet = null;
                break;
            }
            ActionSet _currentNeighbours = new ActionSet();
            foreach(Action _nextAction in _openSet.GetActionSet)
            {
                if (!_closedSet.Contains(_nextAction) && _nextAction.CheckIfNodeIsTraversable(world))
                    _currentNeighbours.AddAction(_nextAction);
            }
            if (_currentNeighbours.IsEmpty)
            {
                _planning = false;
                _closedSet = null;
                break;
            }
            int nextCost = 100;
            foreach(Action _nextAction in _currentNeighbours.GetActionSet)
            {
                _nextAction.CalculateHCost(_targetState);
                _nextAction.CalculateGCost(world);
                if(_nextAction.FCost < nextCost)
                {
                    nextCost = _nextAction.FCost;
                    _currentNode = _nextAction;
                }
            }
            _openSet.RemoveAction(_currentNode);

        }
        bug_logger.UpdateText("Planner", "Status: Complete");
        if (_closedSet != null)
            foreach (Action _a in _closedSet.GetActionSet)
                bug_logger.AddText("Planner", _a.ActionName);
        else
            bug_logger.AddText("Planner", "No valid plan");
        return _closedSet;
    }
}
