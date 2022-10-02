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
    [SerializeField] Text t_timeText;
    [SerializeField] Text t_targetText;
    [SerializeField] Text t_defaultText;
    List<Text> tL_stateText = new List<Text>();
    List<Text> tL_actionText = new List<Text>();

    // Start is called before the first frame update
    void Start()
    {
        sc_worldState = GenericXMLReader.ReadXML<StateCollection>("/Goap/", "WorldState.xml");
        sc_goalState = new StateCollection();
        State canSeeGoal = new State(sc_worldState.GetState("CanSee"));
        canSeeGoal.UpdateState(true);
        State isAliveGoal = new State(sc_worldState.GetState("IsAlive"));
        isAliveGoal.UpdateState(false);
        sc_goalState.AddStates(new State[] { canSeeGoal, isAliveGoal });
        f_currentChange = Random.Range(0.1f, debug_stateChange);
        as_actionSet = GenericXMLReader.ReadXML<ActionSet>("/Goap/", "ActionSet.xml");
        tL_stateText = SetupText(0, sc_worldState.GetStates());
        tL_actionText = SetupText(1, as_actionSet.GetActionSet);
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
        foreach (Action _item in as_actionSet.GetActionSet)
            _item.CalculateHCost(sc_worldState);
        UpdateDebugText();
    }

    private List<Text> SetupText<T>(int _xoffSet, List<T> _col)
    {
        List<Text> newText = new List<Text>();
        foreach(T obj in _col)
        {
            Text _text = Instantiate(t_defaultText);
            _text.gameObject.SetActive(true);
            newText.Add(_text);
            Vector3 pos = _text.transform.position;
            pos.x = t_defaultText.transform.position.x + (t_defaultText.rectTransform.rect.width * _xoffSet);
            pos.y += _text.rectTransform.rect.height * newText.Count;
            _text.transform.position = pos;
            _text.transform.parent = _can.transform;

        }
        return newText;
    }


    private void UpdateDebugText()
    {
        List<State> states = sc_worldState.GetStates();
        List<Action> _actionSet = as_actionSet.GetActionSet; 
        for(int i = 0; i < tL_stateText.Count; i++)
        {
            tL_stateText[i].text = $"{states[i].Name}: {states[i].CheckState()}";
        }
        for (int j = 0; j < _actionSet.Count; j++)
        {
            tL_actionText[j].text = $"{_actionSet[j].ActionName}: {_actionSet[j].CheckActionEligibility(sc_worldState)} | Cost: {_actionSet[j].HCost}";
        }
        t_timeText.text = $"Current Time: {f_currentTime.ToString()}";
        t_targetText.text = $"Target Time: {f_currentChange}";
    }
}
