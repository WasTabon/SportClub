using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class ClubManager : MonoBehaviour
{
    [SerializeField] private string _name;

    public void SetName(TMP_InputField inputField)
    {
        string input = inputField.text.Trim();
        
        if (string.IsNullOrWhiteSpace(input))
        {
            UIManager.Instance.ShowWarning(Warnings.EnterName);
        }
        else if (!Regex.IsMatch(input, @"^[A-Za-z]+$"))
        {
            UIManager.Instance.ShowWarning(Warnings.EnterEnglishName);
        }
        else
        {
            Debug.Log(_name);
        }
        
    }
}
