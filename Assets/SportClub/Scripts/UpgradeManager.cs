using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
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

    private const string UpgradeLevelKey = "UpgradedLevel";

    private void Start()
    {
        LoadUpgradeState();
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
        //UIManager.Instance.ShowPopup(Popups.LevelUpgraded);
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

    private void LoadUpgradeState()
    {
        int savedLevel = PlayerPrefs.GetInt(UpgradeLevelKey, 1); // по умолчанию 1

        ClubManager.Instance.SetLevel(savedLevel);

        for (int i = 2; i <= savedLevel; i++)
        {
            EnableLevelObjects(i);
        }
    }
}
