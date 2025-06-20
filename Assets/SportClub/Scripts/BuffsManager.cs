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
    SeenAsWeek,
    //Mech
    Sold,
    Cold,
    Payday,
    Vandalized,
    //Sabotage
    Scandal,
    Caught,
    Ratings,
    Investigation,
    Fans,
    Backlash
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
                return More("fans", out icon, loyaltyIcon);
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
                return Less("income", out icon, loyaltyIcon);
            case BuffType.Sold:
                return More("income", out icon, loyaltyIcon);
            case BuffType.Cold:
                return Less("income", out icon, loyaltyIcon);
            case BuffType.Payday:
                return More("hype", out icon, loyaltyIcon);
            case BuffType.Vandalized:
                return Less("reputation", out icon, loyaltyIcon);
            case BuffType.Scandal:
                return More("loyalty", out icon, loyaltyIcon);
            case BuffType.Caught:
                return Less("reputation", out icon, loyaltyIcon);
            case BuffType.Ratings:
                return More("fans", out icon, loyaltyIcon);
            case BuffType.Investigation:
                return Less("income", out icon, loyaltyIcon);
            case BuffType.Fans:
                return More("reputation", out icon, loyaltyIcon);
            case BuffType.Backlash:
                return Less("hype", out icon, loyaltyIcon);
            default:
                icon = null;
                return "";
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
            case BuffType.Sold:
                UIManager.Instance.ShowPopup(Popups.Sold);
                break;
            case BuffType.Cold:
                UIManager.Instance.ShowPopup(Popups.Cold);
                break;
            case BuffType.Payday:
                UIManager.Instance.ShowPopup(Popups.Payday);
                break;
            case BuffType.Vandalized:
                UIManager.Instance.ShowPopup(Popups.Vandalized);
                break;
            case BuffType.Scandal:
                UIManager.Instance.ShowPopup(Popups.Scandal);
                break;
            case BuffType.Caught:
                UIManager.Instance.ShowPopup(Popups.Caught);
                break;
            case BuffType.Ratings:
                UIManager.Instance.ShowPopup(Popups.Ratings);
                break;
            case BuffType.Investigation:
                UIManager.Instance.ShowPopup(Popups.Investigation);
                break;
            case BuffType.Fans:
                UIManager.Instance.ShowPopup(Popups.Fans);
                break;
            case BuffType.Backlash:
                UIManager.Instance.ShowPopup(Popups.Backlash);
                break;
        }
    }
}
