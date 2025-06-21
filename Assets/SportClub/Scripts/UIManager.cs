using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public struct WarningEntry
{
   public Warnings Type;
   public TextMeshProUGUI Text;
}

[Serializable]
public struct PopupEntry
{
   public Popups Type;
   public RectTransform Panel;
}

public enum Warnings
{
   EnterName,
   EnterEnglishName
}

public enum Popups
{
   NameEnter,
   PRPopupSure,
   BuffViral,
   BuffNegativeReaction,
   BuffUnited,
   BuffConflict,
   BuffCleanSteets,
   ManagementPopupSure,
   NewVoice,
   Radical,
   OrderRestored,
   FansPushBack,
   CoolerHeads,
   SeenAsWeek,
   MerchPopupSure,
   Sold,
   Cold,
   Payday,
   Vandalized,
   SabotagePopupSure,
   Scandal,
   Caught,
   Ratings,
   Investigation,
   Fans,
   Backlash,
   NotEnoughResources
}

public class UIManager : MonoBehaviour
{
   public static UIManager Instance;

   public RectTransform _loadingScreen;

   [SerializeField] private TextMeshProUGUI _hypeText;
   [SerializeField] private TextMeshProUGUI _reputationText;
   [SerializeField] private TextMeshProUGUI _moneyText;
   [SerializeField] private TextMeshProUGUI _fansText;
   [SerializeField] private TextMeshProUGUI _loyalityText;
   [SerializeField] private TextMeshProUGUI _levelText;
   [SerializeField] private TextMeshProUGUI _timeText;
   
   [SerializeField] private Image _hypeFillImage;
   [SerializeField] private Image _reputationFillImage;
   
   [SerializeField] private float _timeOpenPanel;
   [SerializeField] private float _timeClosePanel;
   
   [SerializeField] private float _timeWarning;
   [SerializeField] private float _timePopup;

   [SerializeField] private TextMeshProUGUI _namePanelText;
   [SerializeField] private TextMeshProUGUI _nameClubTabText;
   
   [Header("Warnings")]
   [SerializeField] private WarningEntry[] _warningEntries;

   [Header("Popups")] 
   [SerializeField] private PopupEntry[] _popupEntries;

   private Dictionary<Warnings, TextMeshProUGUI> _warnings;
   private Dictionary<Popups, RectTransform> _popups;

   private int _prevHype = -1;
   private int _prevReputation = -1;
   private int _prevMoney = -1;
   
   private void Awake()
   {
      Instance = this;
      _warnings = new Dictionary<Warnings, TextMeshProUGUI>();
      _popups = new Dictionary<Popups, RectTransform>();

      foreach (var entry in _warningEntries)
      {
         if (!_warnings.ContainsKey(entry.Type))
            _warnings.Add(entry.Type, entry.Text);
      }

      foreach (var entry in _popupEntries)
      {
         if (!_popups.ContainsKey(entry.Type))
            _popups.Add(entry.Type, entry.Panel);
      }
   }

   private void Start()
   {
      ResetAllWarnings();
      ResetAllPopups();
      
      _loadingScreen.DOScale(Vector3.zero, 0f);
   }
   private void Update()
   {
      foreach (var popup in _popups.Values)
      {
         if (!popup.gameObject.activeSelf && popup.localScale != Vector3.zero)
         {
            popup.localScale = Vector3.zero;
         }
      }

      int hype = ClubManager.Instance.GetHype();
      if (hype != _prevHype)
      {
         AnimateNumberPlain(_hypeText, hype, "/100");
         _prevHype = hype;
      }

      int reputation = ClubManager.Instance.GetReputation();
      if (reputation != _prevReputation)
      {
         AnimateNumberPlain(_reputationText, reputation, "/100");
         _prevReputation = reputation;
      }

      int money = ClubManager.Instance.GetMoney();
      if (money != _prevMoney)
      {
         AnimateNumberPlain(_moneyText, money, "$");
         _prevMoney = money;
      }

      _levelText.text = $"{ClubManager.Instance.GetLevel()} Level";
      _fansText.text = $"{ClubManager.Instance.GetFans()} Fans";
      _loyalityText.text = $"{ClubManager.Instance.GetLoyality()}/100 Loyalty";

      _hypeFillImage.DOFillAmount(hype / 100f, 0.3f);
      _reputationFillImage.DOFillAmount(reputation / 100f, 0.3f);
   }

   public void ClosePanel(RectTransform panel)
   {
      panel.DOScale(Vector3.zero, _timeClosePanel);
   }

   public void OpenPanel(RectTransform panel)
   {
      panel.DOScale(Vector3.one, _timeOpenPanel);
   }

   public void ShowPanelFinishDay()
   {
      
   }
   
   public void AnimateNumberWithPlus(TextMeshProUGUI text, int targetValue, string label, float duration = 1f)
   {
      int currentValue = 0;

      DOTween.To(() => currentValue, x =>
      {
         currentValue = x;
         text.text = $"+{currentValue} {label}";
      }, targetValue, duration).SetEase(Ease.OutQuad);
   }

   public void AnimateNumberPlain(TextMeshProUGUI text, int targetValue, string label, float duration = 1f)
   {
      // Пытаемся получить текущее число из текста
      int currentValue = 0;

      // Извлекаем только число из строки (например, "45 Fans" → 45)
      string numericPart = new string(text.text.TakeWhile(c => char.IsDigit(c) || c == '-').ToArray());
      int.TryParse(numericPart, out currentValue);

      DOTween.To(() => currentValue, x =>
      {
         currentValue = x;
         text.text = $"{currentValue} {label}";
      }, targetValue, duration).SetEase(Ease.OutQuad);
   }
    
   public void ShowWarning(Warnings warning)
   {
      ResetAllWarnings();
      if (_warnings.TryGetValue(warning, out var warningText))
         HandleWarning(warningText);
   }

   public void ShowPopup(Popups popup)
   {
      if (_popups.TryGetValue(popup, out var popupPanel))
         HandlePopup(popupPanel);
   }

   public void SetNameOnPanel(string name)
   {
      _namePanelText.text = $"Great name \n{name}!";
      _nameClubTabText.text = $"Fan Club \"{name}\"";
   }
    
   private void HandleWarning(TextMeshProUGUI text)
   {
      text.DOFade(1, _timeWarning)
         .OnComplete(() =>
         {
            DOVirtual.DelayedCall(1f, () =>
            {
               text.DOFade(0, _timeWarning);
            });
         });
   }

   private void HandlePopup(RectTransform rectTransform)
   {
      if (!rectTransform.gameObject.activeSelf)
         rectTransform.gameObject.SetActive(true);
      
      rectTransform.DOScale(Vector3.one, _timePopup)
         .OnComplete(() =>
         {
            
         });
   }

   private void ResetAllWarnings()
   {
      foreach (var warning in _warnings.Values)
      {
         if (!warning.gameObject.activeSelf)
            Debug.LogError($"Warning {warning.gameObject.name} is disabled", warning.gameObject);
         warning.DOFade(0f, 0f);
      }
   }

   private void ResetAllPopups()
   {
      foreach (var popup in _popups.Values)
      {
         if (!popup.gameObject.activeSelf)
         {
            Debug.LogError($"Popup {popup.gameObject.name} is disabled", popup.gameObject);
         }
         popup.DOScale(Vector3.zero, 0f);
      }
   }
}
