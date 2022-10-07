using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Serialization;

public class Blackboard : MonoBehaviour
{
    // States
    private StateCollection sc_worldState;
    // ActionSets
    private ActionSet as_actionSet;
    private ActionSet as_plannedSet;
    // Agents
    private GoapAgent ga_woodchuck;
    // Planner
    private Planner planner;

    private float debug_stateChange = 20f;
    private float f_currentChange;
    private float f_currentTime;

    [SerializeField] Canvas _can;
    [SerializeField] Debugger bug_logger;
    [SerializeField] Text t_defaultText;

    // Start is called before the first frame update
    void Start()
    {
        sc_worldState = GenericXMLReader.ReadXML<StateCollection>("/Goap/", "WorldState.xml");
        ga_woodchuck = GenericXMLReader.ReadXML<GoapAgent>("/Goap/Agents/", "TestAgent.xml");
        f_currentChange = Random.Range(0.1f, debug_stateChange);
        as_actionSet = GenericXMLReader.ReadXML<ActionSet>("/Goap/Actions/", "ActionSet.xml");
        SetupText();
        planner = new Planner(bug_logger);
    }

    // Update is called once per frame
    void Update()
    {
        if (f_currentTime >= f_currentChange)
        {
            sc_worldState.GetState("CanSee")?.FlipState();
            sc_worldState.GetState("HasPath")?.FlipState();
            f_currentChange = Random.Range(0.1f, debug_stateChange);
            f_currentTime = 0f;
        }
        f_currentTime += Time.deltaTime;
        if (!planner.Planning)
        {
            foreach (StateCollection _goal in ga_woodchuck.sL_goals)
            {
                if((as_plannedSet is object))
                    break;
                else
                    as_plannedSet = planner.Plan(as_actionSet, sc_worldState, _goal);
            }
        }
        UpdateDebugText();
    }

    private void SetupText()
    {
        bug_logger.AddTextCategory("Time");
        bug_logger.AddTextCategory("World");
        bug_logger.AddTextCategory("Actions");
        bug_logger.AddTextCategory("Plan");
        bug_logger.AddText("Time", $"Current time: {f_currentTime}", $"Target time: {f_currentChange}");

        foreach (State _state in sc_worldState.GetStates())
            bug_logger.AddText("World", $"{_state.Name}: {_state.CheckState()} ");
        foreach (Action _action in as_actionSet.GetActionSet)
            bug_logger.AddText("Actions", $"{_action.ActionName}: {_action.CheckActionEligibility(sc_worldState)}");
    }

    private void UpdateDebugText()
    {
        bug_logger.UpdateText("Time", $"Current time: {f_currentTime.ToString("n2")}", $"Target time: {f_currentChange.ToString("n2")}");
        string[] _worldStates = new string[sc_worldState.GetStates().Count];
        string[] _actions = new string[as_actionSet.GetActionSet.Count];

        for (int i = 0; i < sc_worldState.GetStates().Count; i++)
            _worldStates[i] = $"{sc_worldState.GetStates()[i].Name}: {sc_worldState.GetStates()[i].CheckState()}";
        for (int i = 0; i < as_actionSet.GetActionSet.Count; i++)
            _actions[i] = $"{as_actionSet.GetActionSet[i].ActionName}: {as_actionSet.GetActionSet[i].CheckActionEligibility(sc_worldState)}";

        bug_logger.UpdateText("World", _worldStates);
        bug_logger.UpdateText("Actions", _actions);

        string[] _plan = new string[as_plannedSet is object ? as_plannedSet.GetActionSet.Count + 1 : 2];
        _plan[0] = "Plan";
        for (int i = 1; i < _plan.Length; i++)
            if (as_plannedSet is object)
                _plan[i] = as_plannedSet.GetActionSet[i-1].ActionName;
            else
                _plan[i] = "No Actions";
        bug_logger.AddText("Plan", _plan);
    }
}
