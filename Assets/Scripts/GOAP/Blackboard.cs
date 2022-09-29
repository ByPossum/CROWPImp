using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackboard : MonoBehaviour
{
    private StateCollection sc_worldState;
    private StateCollection sc_goalState;
    private List<Action> aL_actionSet = new List<Action>();
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
        sc_worldState = new StateCollection();
        State canSee = new State("CanSee");
        State hasPath = new State("HasPath");
        State isAlive = new State("IsAlive", true);
        State isTired = new State("IsTired");
        State nearTarget = new State("NearTarget");
        sc_worldState.AddStates(new State[] { canSee, hasPath, isAlive, isTired, nearTarget });
        sc_goalState.AddStates(new State[] { })
        f_currentChange = Random.Range(0.1f, debug_stateChange);
        Action look = new Action("Look", null, new State[] { new State("CanSee", true), new State("IsAlive", true) }, new State[] { new State("HasPath") });
        Action run = new Action("Run", null, new State[] { new State("CanSee", true), new State("HasPath", true), new State("IsAlive", true), new State("IsTired", false) },
            new State[] { new State("NearTarget", true) });
        aL_actionSet.Add(look);
        aL_actionSet.Add(run);
        tL_stateText = SetupText(0, sc_worldState.GetStates());
        tL_actionText = SetupText(1, aL_actionSet);
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
        foreach (Action _item in aL_actionSet)
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
        for(int i = 0; i < tL_stateText.Count; i++)
        {
            tL_stateText[i].text = $"{states[i].Name}: {states[i].CheckState()}";
        }
        for (int j = 0; j < aL_actionSet.Count; j++)
        {
            tL_actionText[j].text = $"{aL_actionSet[j].ActionName}: {aL_actionSet[j].CheckActionEligibility(sc_worldState)} | Cost: {aL_actionSet[j].HCost}";
        }
        t_timeText.text = $"Current Time: {f_currentTime.ToString()}";
        t_targetText.text = $"Target Time: {f_currentChange}";
    }
}
