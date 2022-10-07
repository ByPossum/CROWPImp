using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

[XmlRoot("Agent")]
public class GoapAgent
{
    [XmlAttribute("Name")]
    public string s_name;
    [XmlElement("Actions")]
    public ActionSet as_actions = new ActionSet();
    [XmlElement("Goals")]
    public List<StateCollection> sL_goals = new List<StateCollection>();

    public GoapAgent() { }
    public GoapAgent(string _name, ActionSet _actions, params StateCollection[] _goals)
    {
        s_name = _name;
        as_actions = _actions;
        for (int i = 0; i < _goals.Length; i++)
            sL_goals.Add(_goals[i]);
    }
}
