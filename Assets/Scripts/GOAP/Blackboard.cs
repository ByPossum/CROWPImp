using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Serialization;

public class Blackboard : MonoBehaviour
{
    private StateCollection sc_worldState;
    private StateCollection sc_goalState;
    private ActionSet as_actionSet;
    private float debug_stateChange = 20f;
    private float f_currentChange;
    private float f_currentTime;

    [SerializeField] Canvas _can;
    [SerializeField] Debugger bug_logger;
    [SerializeField] Text t_timeText;
    [SerializeField] Text t_targetText;
    [SerializeField] Text t_defaultText;
    List<Text> tL_stateText = new List<Text>();
    List<Text> tL_actionText = new List<Text>();

    // Start is called before the first frame update
    void Start()
    {
        sc_worldState = GenericXMLReader.ReadXML<StateCollection>("/Goap/", "WorldState.xml");
        sc_goalState = GenericXMLReader.ReadXML<StateCollection>("/Goap/Agents/", "GoalState_AgentName.xml");
        f_currentChange = Random.Range(0.1f, debug_stateChange);
        as_actionSet = GenericXMLReader.ReadXML<ActionSet>("/Goap/Actions/", "ActionSet.xml");
        SetupText();
        Planner planner = new Planner(bug_logger);
        planner.Plan(as_actionSet, sc_worldState, sc_goalState);
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
        UpdateDebugText();
    }

    private void SetupText()
    {
        bug_logger.AddTextCategory("Time");
        bug_logger.AddTextCategory("World");
        bug_logger.AddTextCategory("Actions");

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
        // TODO: Stop being lazy and clean this up a bit.
        int _lziter = 0;
        foreach (State _state in sc_worldState.GetStates())
        {
            _worldStates[_lziter] = $"{_state.Name}: {_state.CheckState()}";
            _lziter++;
        }
        _lziter = 0;
        foreach (Action _action in as_actionSet.GetActionSet)
        {

            _actions[_lziter] = $"{_action.ActionName}: {_action.CheckActionEligibility(sc_worldState)}";
            _lziter++;
        }
        bug_logger.UpdateText("World", _worldStates);
        bug_logger.UpdateText("Actions", _actions);
    }
}
