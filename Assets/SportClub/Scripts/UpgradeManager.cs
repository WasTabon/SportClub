using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _priceText;
    
    [SerializeField] private List<int> _upgradeCostAmount;

    [SerializeField] private GameObject[] _level2;
    [SerializeField] private GameObject[] _level3;
    [SerializeField] private GameObject[] _level4;
    [SerializeField] private GameObject[] _level5;
    [SerializeField] private GameObject[] _level6;
    [SerializeField] private GameObject[] _level7;
    [SerializeField] private GameObject[] _level8;
    [SerializeField] private GameObject[] _level9;
    [SerializeField] private GameObject[] _level10;
    [SerializeField] private GameObject[] _level11;

    [SerializeField] private AudioSource _upgradeSound;

    [SerializeField] private TextMeshProUGUI _levelDescriptionsText;
    
    private const string UpgradeLevelKey = "UpgradedLevel";
    
    private readonly string[] _levelDescriptions = new string[]
    {
        "Level 1: PR and partial club management access",
        "Level 2: PR and full club management access",
        "Level 3: All of the above, plus basic merchandise sales",
        "Level 4: All of the above, plus full control over merchandise",
        "Level 5: All of the above, plus the ability to carry out minor sabotages",
        "Level 6: All of the above, plus full control over sabotages",
        "Level 7: Increased income generation",
        "Level 8: Increased reputation gain",
        "Level 9: Increased fan gain",
        "Level 10: Increased hype gain",
        "Level 11: Increased loyalty gain"
    };

    private void Start()
    {
        LoadUpgradeState();
        UpdateLevelDescriptionUI();
        UpdatePriceText();
    }

    public void UpgradeLevel()
    {
        int currentLevel = ClubManager.Instance.GetLevel();

        // Максимум 11 уровень
        if (currentLevel >= 11) return;

        int nextLevel = currentLevel + 1;

        if (_upgradeCostAmount.Count < nextLevel)
        {
            Debug.LogWarning("Нет цены для следующего уровня.");
            return;
        }

        int cost = _upgradeCostAmount[nextLevel - 1];
        int playerMoney = ClubManager.Instance.GetMoney();

        if (playerMoney < cost)
        {
            UIManager.Instance.ShowPopup(Popups.NotEnoughResources);
            return;
        }

        // Списываем деньги и сохраняем
        ClubManager.Instance.SetMoney(playerMoney - cost);
        ClubManager.Instance.SetLevel(nextLevel);
        PlayerPrefs.SetInt(UpgradeLevelKey, nextLevel);

        // Включаем объекты уровня
        EnableLevelObjects(nextLevel);

        // Проигрываем звук, если есть
        if (_upgradeSound != null)
            _upgradeSound.Play();

        // Показываем попап
        UIManager.Instance.ShowPopup(Popups.LevelUpgraded);
        
        UpdateLevelDescriptionUI();
        UpdatePriceText();
    }

    private void UpdatePriceText()
    {
        int currentLevel = ClubManager.Instance.GetLevel();

        // Если достигнут максимальный уровень — скрываем цену или пишем что апгрейд недоступен
        if (currentLevel >= 11)
        {
            if (_priceText != null)
                _priceText.text = "Max Level";
            return;
        }

        if (_upgradeCostAmount.Count < currentLevel + 1)
        {
            if (_priceText != null)
                _priceText.text = "No price set";
            return;
        }

        int nextCost = _upgradeCostAmount[currentLevel]; // индекс текущего уровня + 1, т.к. уровни начинаются с 1
        if (_priceText != null)
            _priceText.text = $"Price: {nextCost}$";
    }
    
    private void EnableLevelObjects(int level)
    {
        GameObject[] levelObjects = GetObjectsForLevel(level);
        if (levelObjects == null) return;

        foreach (var obj in levelObjects)
        {
            if (obj != null)
                obj.SetActive(true);
        }
    }

    private GameObject[] GetObjectsForLevel(int level)
    {
        return level switch
        {
            2 => _level2,
            3 => _level3,
            4 => _level4,
            5 => _level5,
            6 => _level6,
            7 => _level7,
            8 => _level8,
            9 => _level9,
            10 => _level10,
            11 => _level11,
            _ => null
        };
    }

    private void UpdateLevelDescriptionUI()
    {
        if (_levelDescriptionsText == null)
        {
            Debug.LogWarning("LevelDescriptionsText не назначен в инспекторе.");
            return;
        }

        int currentLevel = ClubManager.Instance.GetLevel();
        string result = "";

        for (int i = 0; i < _levelDescriptions.Length; i++)
        {
            if (i < currentLevel) // Зачеркнуть открытые уровни
                result += $"<s>{_levelDescriptions[i]}</s>\n\n";
            else
                result += _levelDescriptions[i] + "\n\n";
        }

        _levelDescriptionsText.text = result;
    }
    
    private void LoadUpgradeState()
    {
        int savedLevel = PlayerPrefs.GetInt(UpgradeLevelKey, 1); // по умолчанию 1

        ClubManager.Instance.SetLevel(savedLevel);

        // Включаем объекты открытых уровней
        for (int i = 2; i <= savedLevel; i++)
        {
            EnableLevelObjects(i);
        }

        // Выключаем объекты уровней, которые еще не открыты
        for (int i = savedLevel + 1; i <= 11; i++)
        {
            DisableLevelObjects(i);
        }
    }

    private void DisableLevelObjects(int level)
    {
        GameObject[] levelObjects = GetObjectsForLevel(level);
        if (levelObjects == null) return;

        foreach (var obj in levelObjects)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
}
