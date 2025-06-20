using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum BuffType
{
    //PR
    ViralBuff,
    NegativeReactionBuff,
    FansUnited,
    FansConflitct,
    CleanStreets,
    //Management
    NewVoice,
    Radical,
    OrderRestored,
    FansPushBack,
    CoolerHeads,
    SeenAsWeek
}

public class BuffsManager : MonoBehaviour
{
    public static BuffsManager Instance;
    
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _buff;

    [SerializeField] private Sprite _viralIcon;
    [SerializeField] private Sprite _negativeReactionIcon;
    [SerializeField] private Sprite _fansUnitedIcon;
    [SerializeField] private Sprite _fansConflictIcon;
    [SerializeField] private Sprite _cleanStreetsIcon;
    
    [SerializeField] private Sprite incomeIcon;
    [SerializeField] private Sprite reputationIcon;
    [SerializeField] private Sprite loyaltyIcon;
    [SerializeField] private Sprite hypeIcon;

    private void Awake()
    {
        Instance = this;
    }

    public void AddBuff(BuffType buffType)
    {
        string description = GetDescriptionByType(buffType, out Sprite sprite);
        
        RectTransform buff = Instantiate(_buff.gameObject, _content).GetComponent<RectTransform>();
        Image icon = buff.GetComponentInChildren<Image>();
        TextMeshProUGUI text = buff.GetComponentInChildren<TextMeshProUGUI>();

        icon.sprite = sprite;
        text.text = description;

        GetPopupByType(buffType);
    }

    private int GeneratePercent() => Random.Range(5, 31);

    private string More(string statName, out Sprite icon, Sprite statIcon)
    {
        int percent = GeneratePercent();
        icon = statIcon;
        return $"+{percent}% to {statName}";
    }

    private string Less(string statName, out Sprite icon, Sprite statIcon)
    {
        int percent = GeneratePercent();
        icon = statIcon;
        return $"-{percent}% to {statName}";
    }

    private string GetDescriptionByType(BuffType buffType, out Sprite icon)
    {
        switch (buffType)
        {
            case BuffType.ViralBuff:
                return More("income", out icon, incomeIcon);
            case BuffType.NegativeReactionBuff:
                return Less("loyalty", out icon, loyaltyIcon);
            case BuffType.FansUnited:
                return More("loyalty", out icon, loyaltyIcon);
            case BuffType.FansConflitct:
                return Less("hype", out icon, reputationIcon);
            case BuffType.CleanStreets:
                return More("reputation", out icon, reputationIcon);
            case BuffType.NewVoice:
                return More("income", out icon, reputationIcon);
            case BuffType.Radical:
                return Less("reputation", out icon, reputationIcon);
            case BuffType.OrderRestored:
                return More("loyalty", out icon, loyaltyIcon);
            case BuffType.FansPushBack:
                return Less("hype", out icon, loyaltyIcon);
            case BuffType.CoolerHeads:
                return More("loyalty", out icon, loyaltyIcon);
            case BuffType.SeenAsWeek:
                return More("income", out icon, loyaltyIcon);
            default:
                icon = null;
                return "";
        }
    }

    private Sprite GetIconByType(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.ViralBuff:
                return _viralIcon;
            case BuffType.NegativeReactionBuff:
                return _negativeReactionIcon;
            case BuffType.FansUnited:
                return _fansUnitedIcon;
            case BuffType.FansConflitct:
                return _fansConflictIcon;
            default:
                return null;
        }
    }

    private void GetPopupByType(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.ViralBuff:
                UIManager.Instance.ShowPopup(Popups.BuffViral);
                break;
            case BuffType.NegativeReactionBuff:
                UIManager.Instance.ShowPopup(Popups.BuffNegativeReaction);
                break;
            case BuffType.FansUnited:
                UIManager.Instance.ShowPopup(Popups.BuffUnited);
                break;
            case BuffType.FansConflitct:
                UIManager.Instance.ShowPopup(Popups.BuffConflict);
                break;
            case BuffType.CleanStreets:
                UIManager.Instance.ShowPopup(Popups.BuffCleanSteets);
                break;
            case BuffType.NewVoice:
                UIManager.Instance.ShowPopup(Popups.NewVoice);
                break;
            case BuffType.Radical:
                UIManager.Instance.ShowPopup(Popups.Radical);
                break;
            case BuffType.OrderRestored:
                UIManager.Instance.ShowPopup(Popups.OrderRestored);
                break;
            case BuffType.FansPushBack:
                UIManager.Instance.ShowPopup(Popups.FansPushBack);
                break;
            case BuffType.CoolerHeads:
                UIManager.Instance.ShowPopup(Popups.CoolerHeads);
                break;
            case BuffType.SeenAsWeek:
                UIManager.Instance.ShowPopup(Popups.SeenAsWeek);
                break;
        }
    }
}
