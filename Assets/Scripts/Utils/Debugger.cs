using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Debugger : MonoBehaviour
{
    Dictionary<string, List<Text>> tD_textLists = new Dictionary<string, List<Text>>();
    [SerializeField] private Canvas c_can;
    [SerializeField] private Text t_defaultText;

    public void AddTextCategory(string _key)
    {
        if (!tD_textLists.ContainsKey(_key))
        {
            tD_textLists.Add(_key, new List<Text>());
            List<Text> _newText = new List<Text>();
            return;
        }
        Debug.LogError("DB001: Key you're trying to add is already in the dictionary!");
    }

    public void AddTextCategory(string _key, params string[] _text)
    {
        AddTextCategory(_key);
        foreach(string _t in _text)
            tD_textLists[_key].Add(CreateNewText(_key, _t));
    }

    public void AddText(string _key, params string[] _text)
    {
        if (!tD_textLists.ContainsKey(_key))
        {
            AddTextCategory(_key, _text);
            return;
        }

        foreach(string _t in _text)
        {
            bool add = true;
            foreach (Text _cloneCheck in tD_textLists[_key])
                if (_cloneCheck.text == _t)
                    add = false;
            if(add)
                tD_textLists[_key].Add(CreateNewText(_key, _t));
        }
    }

    public void UpdateText(string _key, params string[] _text)
    {
        List<Text> _textToUpdate = tD_textLists[_key];
        for (int i = 0; i < _text.Length; i++)
            _textToUpdate[i].text = _text[i];
    }

    private Text CreateNewText(string _key, string _textToSee = "")
    {
        Text _text = Instantiate(t_defaultText);
        float x = (t_defaultText.rectTransform.rect.width * GetKeyIndex(_key)) + (t_defaultText.rectTransform.rect.width * 0.5f);
        float y = t_defaultText.transform.position.y - t_defaultText.rectTransform.rect.height * tD_textLists[_key].Count+1;
        _text.transform.position = new Vector3(x, y, 0f);
        _text.text = _textToSee;
        _text.transform.parent = c_can.transform;
        _text.gameObject.SetActive(true);
        return _text;
    }

    private int GetKeyIndex(string _key)
    {
        int i = -1;
        foreach(string _s in tD_textLists.Keys)
        {
            i++;
            if (_s == _key)
                return i;
        }
        return -1;
    }
}
