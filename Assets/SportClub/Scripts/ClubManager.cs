using System.Text.RegularExpressions;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ClubManager : MonoBehaviour
{
    [SerializeField] private string _playerName;

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
            _playerName = input;
            
            UIManager.Instance.SetNameOnPanel(_playerName);
            
            DOVirtual.DelayedCall(0.3f, (() =>
            {
                UIManager.Instance.ShowPopup(Popups.NameEnter);
            }));
        }
        
    }
}
