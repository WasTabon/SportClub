using System;
using System.Text.RegularExpressions;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ClubManager : MonoBehaviour
{
    public static ClubManager Instance;
    
    [SerializeField] private string _playerName;
    [SerializeField] private int _money;
    [SerializeField] private int _hype;
    [SerializeField] private int _reputation;
    [SerializeField] private int _fans;
    [SerializeField] private int _loyality;
    [SerializeField] private int _level;

    public int GetMoney() => _money;
    public int GetReputation() => _reputation;
    public int GetHype() => _hype;
    public int GetLevel() => _level;
    public int GetFans() => _fans;
    public int GetLoyality() => _loyality;
    
    public void SetMoney(int amount) => _money = amount;
    public void SetReputation(int value) => _reputation = value;
    public void SetHype(int value) => _hype = value;
    public void SetLevel(int value) => _level = value;
    public void SetFans(int value) => _fans = value;
    public void SetLoyality(int value) => _loyality = value;
    
    private void Awake()
    {
        Instance = this;
    }

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
