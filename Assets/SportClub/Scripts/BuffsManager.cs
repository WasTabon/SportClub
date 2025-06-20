using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private void Awake()
    {
        Instance = this;
    }

    public void AddBuff(BuffType buffType)
    {
        string description = GetDescriptionByType(buffType);
        Sprite sprite = GetIconByType(buffType);
        
        RectTransform buff = Instantiate(_buff.gameObject, _content).GetComponent<RectTransform>();
        Image icon = buff.GetComponentInChildren<Image>();
        TextMeshProUGUI text = buff.GetComponentInChildren<TextMeshProUGUI>();

        icon.sprite = sprite;
        text.text = description;

        GetPopupByType(buffType);
    }

    private string GetDescriptionByType(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.ViralBuff:
                return "+20% to income";
            case BuffType.NegativeReactionBuff:
                return "-10% to loyalty";
            case BuffType.FansUnited:
                return "+10% to loyalty";
            case BuffType.FansConflitct:
                return "-10% to reputation";
            case BuffType.CleanStreets:
                return "+15% to reputation";
            case BuffType.NewVoice:
                return "+30% to reputation";
            case BuffType.Radical:
                return "-30% to reputation";
            case BuffType.OrderRestored:
                return "+10% to loyalty";
            case BuffType.FansPushBack:
                return "-15% to loyalty";
            case BuffType.CoolerHeads:
                return "+10% to loyalty";
            case BuffType.SeenAsWeek:
                return "+10% to loyalty";
            default:
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
        }
    }
}
