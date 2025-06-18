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
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _buff;

    [SerializeField] private Sprite _viralIcon;

    public void AddBuff(BuffType buffType)
    {
        string description = GetDescriptionByType(buffType);
        Sprite sprite = GetIconByType(buffType);
        
        RectTransform buff = Instantiate(_buff.gameObject, _content).GetComponent<RectTransform>();
        Image icon = buff.GetComponentInChildren<Image>();
        TextMeshProUGUI text = buff.GetComponentInChildren<TextMeshProUGUI>();

        icon.sprite = sprite;
        text.text = description;
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
}
