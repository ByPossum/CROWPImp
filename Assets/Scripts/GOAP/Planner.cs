using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planner
{
    private Debugger bug_logger;
    private bool b_planning = false;
    public bool Planning { get { return b_planning; } }
    public Planner(Debugger _bug)
    {
        bug_logger = _bug;
        bug_logger.AddTextCategory("Planner");
        bug_logger.AddText("Planner", "Status: Beginning");
    }

    public ActionSet Plan(ActionSet _initialSet, StateCollection _worldState, StateCollection _targetState)
    {
        bug_logger.UpdateText("Planner", "Status: Initializing plan");
        // Deep copy the states
        StateCollection world = new StateCollection(_worldState);
        ActionSet _openSet = new ActionSet(_initialSet.GetActionSet.ToArray());
        // Set to planning
        b_planning = true;

        // Initialize action costs
        foreach (Action _action in _openSet.GetActionSet)
        {
            _action.CalculateGCost(world);
            _action.CalculateHCost(_targetState);
        }

        // Get our first node
        Action _currentNode = _openSet.RemoveAction(_openSet.GetLowestCostAction());
        ActionSet _closedSet = new ActionSet(_currentNode);

        while (b_planning)
        {
            bug_logger.UpdateText("Planner", "Status: Planning");
            // Update the simulated worldstate with the last action's effects
            foreach (State _resultingState in _currentNode.PostCondition.GetStates())
                world.UpdateState(_resultingState);

            // Plan Satisfied.
            if (_targetState == world)
            {
                b_planning = false;
                break;
            }
            // Plan cannot be satisfied. Move onto a lower priority Goal
            if (_openSet.IsEmpty)
            {
                b_planning = false;
                _closedSet = null;
                break;
            }
            // Collect potential other actions
            ActionSet _currentNeighbours = new ActionSet();
            foreach(Action _nextAction in _openSet.GetActionSet)
            {
                if (!_closedSet.Contains(_nextAction) && _nextAction.CheckIfNodeIsTraversable(world))
                    _currentNeighbours.AddAction(_nextAction);
            }
            // There are no other actions so plan cannot be satisfied. Move onto a lower priority Goal
            if (_currentNeighbours.IsEmpty)
            {
                b_planning = false;
                _closedSet = null;
                break;
            }
            // Choose next action to take
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
            // Remove it from available actions (N.B. Consider making actions repeatable?)
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
