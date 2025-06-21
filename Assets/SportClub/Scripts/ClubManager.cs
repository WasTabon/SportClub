using System;
using System.Text.RegularExpressions;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class ClubManager : MonoBehaviour
{
    public static ClubManager Instance;

    [SerializeField] private GameObject _setNamePanel;
    
    [SerializeField] private string _playerName;
    [SerializeField] private int _money;
    [SerializeField] private int _hype;
    [SerializeField] private int _reputation;
    [SerializeField] private int _fans;
    [SerializeField] private int _loyality;
    [SerializeField] private int _level;

    private const string PlayerNameKey = "PlayerName";
    private const string MoneyKey = "Money";
    private const string HypeKey = "Hype";
    private const string ReputationKey = "Reputation";
    private const string FansKey = "Fans";
    private const string LoyalityKey = "Loyality";
    private const string LevelKey = "Level";
    
    public int GetMoney() => _money;
    public int GetReputation() => _reputation;
    public int GetHype() => _hype;
    public int GetLevel() => _level;
    public int GetFans() => _fans;
    public int GetLoyality() => _loyality;
    
    public void SetMoney(int amount)
    {
        _money = amount;
        PlayerPrefs.SetInt(MoneyKey, _money);
    }

    public void SetReputation(int value)
    {
        _reputation = Mathf.Clamp(value, 0, 100);
        PlayerPrefs.SetInt(ReputationKey, _reputation);
    }

    public void SetHype(int value)
    {
        _hype = Mathf.Clamp(value, 0, 100);
        PlayerPrefs.SetInt(HypeKey, _hype);
    }

    public void SetLevel(int value)
    {
        _level = value;
        PlayerPrefs.SetInt(LevelKey, _level);
    }

    public void SetFans(int value)
    {
        _fans = value;
        PlayerPrefs.SetInt(FansKey, _fans);
    }

    public void SetLoyality(int value)
    {
        _loyality = Mathf.Clamp(value, 0, 100);
        PlayerPrefs.SetInt(LoyalityKey, _loyality);
    }
    
    private void Awake()
    {
        Instance = this;

        _money = PlayerPrefs.GetInt(MoneyKey, 50);
        _hype = PlayerPrefs.GetInt(HypeKey, 10);
        _reputation = PlayerPrefs.GetInt(ReputationKey, 10);
        _fans = PlayerPrefs.GetInt(FansKey, 10);
        _loyality = PlayerPrefs.GetInt(LoyalityKey, 10);
        _level = PlayerPrefs.GetInt(LevelKey, 1);
        
        _playerName = PlayerPrefs.GetString(PlayerNameKey, "");

        if (string.IsNullOrWhiteSpace(_playerName))
        {
            _setNamePanel.SetActive(true);
        }
        else
        {
            _setNamePanel.SetActive(false);
            UIManager.Instance.SetNameOnPanel(_playerName);
        }
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
            
            PlayerPrefs.SetString(PlayerNameKey, _playerName);
            PlayerPrefs.Save();
            
            UIManager.Instance.SetNameOnPanel(_playerName);
            
            DOVirtual.DelayedCall(0.3f, (() =>
            {
                UIManager.Instance.ShowPopup(Popups.NameEnter);
            }));
        }
    }
}
