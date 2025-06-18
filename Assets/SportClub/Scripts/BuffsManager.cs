using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum BuffType
{
    ViralBuff,
    NegativeReactionBuff
}

public class BuffsManager : MonoBehaviour
{
    public static BuffsManager Instance;
    
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _buff;

    [SerializeField] private Sprite _viralIcon;

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
                return _viralIcon;
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
        }
    }
}
