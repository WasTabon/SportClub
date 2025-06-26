using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using PowerLines.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
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
   NotEnoughResources,
   LevelUpgraded
}

public class UIManager : MonoBehaviour
{
   public static UIManager Instance;

   public AudioClip _popupSound;
   public AudioClip _warningSound;
   public AudioClip _finishDaySound;
   
   public TextMeshProUGUI timeText;
   
   public RectTransform _loadingScreen;

   [SerializeField] private RectTransform _finishDayPanel;
   [SerializeField] private RectTransform _finishDayButton;
   [SerializeField] private TextMeshProUGUI _moneyCount;
   [SerializeField] private TextMeshProUGUI _reputationCount;
   [SerializeField] private TextMeshProUGUI _hypeCount;
   [SerializeField] private TextMeshProUGUI _fansCount;
   [SerializeField] private TextMeshProUGUI _loyaltyCount;
   
   [Header("Text")]
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
   
   private Coroutine _blinkingColonCoroutine;
   private bool _showColon = true;

   private TimeSpan _currentTime = new TimeSpan(10, 0, 0);
   
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
      _finishDayPanel.DOScale(Vector3.zero, 0f);
      
      _blinkingColonCoroutine = StartCoroutine(BlinkingColon());
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
   
   public void ClosePanelFinishDay(RectTransform panel)
   {
      ResetTimeToMorning();
      
      // Очистка текстов
      _moneyCount.text = "";
      _reputationCount.text = "";
      _hypeCount.text = "";
      _fansCount.text = "";
      _loyaltyCount.text = "";

      // Сброс кнопки
      CanvasGroup buttonGroup = _finishDayButton.GetComponent<CanvasGroup>();
      if (buttonGroup != null)
      {
         buttonGroup.DOFade(0f, 0.2f);
      }

      // Анимация закрытия панели: scale + вращение
      Sequence closeSeq = DOTween.Sequence();
      closeSeq.Append(panel.DORotate(new Vector3(0, 0, -10f), 0.2f));
      closeSeq.Join(panel.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack));
      closeSeq.OnComplete(() =>
      {
         panel.rotation = Quaternion.identity;
      });
   }

   private IEnumerator BlinkingColon()
   {
      while (true)
      {
         _showColon = !_showColon;

         string separator = _showColon ? ":" : " ";
         SetTimeText(_currentTime);

         yield return new WaitForSeconds(0.5f);
      }
   }
   
   public void AnimateTimeAdvance(TimeSpan advance)
   {
      TimeSpan startTime = _currentTime;
      TimeSpan endTime = _currentTime + advance;

      if (endTime.Hours >= 23)
      {
         endTime = new TimeSpan(23, 0, 0);
      }

      DOTween.To(() => 0f, t =>
      {
         TimeSpan current = TimeSpan.FromMinutes(Mathf.Lerp((float)startTime.TotalMinutes, (float)endTime.TotalMinutes, t));
         SetTimeText(current);
      }, 1f, 1.2f).SetEase(Ease.Linear).OnComplete(() =>
      {
         _currentTime = endTime;
         if (_currentTime.Hours >= 23)
         {
            TabsHandler.Instance.FinishDay();
         }
      });
   }
   
   public void ResetTimeToMorning()
   {
      TimeSpan start = _currentTime;
      TimeSpan target = new TimeSpan(10, 0, 0);

      float startMinutes = (float)start.TotalMinutes;
      float targetMinutes = (float)target.TotalMinutes;

      if (targetMinutes <= startMinutes)
         targetMinutes += 1440f;

      DOTween.To(() => 0f, t =>
      {
         float currentMins = Mathf.Lerp(startMinutes, targetMinutes, t);
         TimeSpan current = TimeSpan.FromMinutes(currentMins % 1440);
         SetTimeText(current);
      }, 1f, 2f).SetEase(Ease.InOutCubic).OnComplete(() =>
      {
         SetTimeText(new TimeSpan(10, 0, 0));
      });
   }
   
   private void SetTimeText(TimeSpan time)
   {
      _currentTime = time;
      string separator = _showColon ? ":" : " ";
      _timeText.text = $"{_currentTime.Hours:D2}{separator}{_currentTime.Minutes:D2}";
   }
   
   public void OpenPanel(RectTransform panel)
   {
      panel.DOScale(Vector3.one, _timeOpenPanel);
   }

   public void ShowPanelFinishDay()
   {
      _finishDayPanel.DOScale(Vector3.one, 0.15f);
      _finishDayPanel.rotation = Quaternion.Euler(0, 0, 10);
    _finishDayPanel.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
    _finishDayPanel.DORotate(Vector3.zero, 0.3f).SetEase(Ease.OutQuad);

    List<(TextMeshProUGUI text, Func<int> getter, Action<int> setter, string label, int min, int max)> stats = new()
    {
        (_moneyCount, ClubManager.Instance.GetMoney, ClubManager.Instance.SetMoney, "$", 50, 200),
        (_reputationCount, ClubManager.Instance.GetReputation, ClubManager.Instance.SetReputation, "Reputation", 5, 10),
        (_hypeCount, ClubManager.Instance.GetHype, ClubManager.Instance.SetHype, "Hype", 5, 10),
        (_fansCount, ClubManager.Instance.GetFans, ClubManager.Instance.SetFans, "Fans", 5, 10),
        (_loyaltyCount, ClubManager.Instance.GetLoyality, ClubManager.Instance.SetLoyality, "Loyalty", 5, 10),
    };

    foreach (var entry in stats)
    {
        entry.text.transform.localScale = Vector3.zero;
        entry.text.text = "";
    }

    _finishDayButton.localScale = Vector3.zero;
    CanvasGroup buttonGroup = _finishDayButton.GetComponent<CanvasGroup>();
    if (buttonGroup == null) buttonGroup = _finishDayButton.gameObject.AddComponent<CanvasGroup>();
    buttonGroup.alpha = 0;

    Sequence seq = DOTween.Sequence();
    float appearTime = 0.3f;
    float countTime = 0.8f;

    for (int i = 0; i < stats.Count; i++)
    {
        var (text, getter, setter, label, min, max) = stats[i];
        int bonus = UnityEngine.Random.Range(min, max + 1);
        int oldValue = getter();
        int newValue = oldValue + bonus;

        seq.Append(text.transform.DOScale(Vector3.one, appearTime).SetEase(Ease.OutBack));
        seq.AppendCallback(() =>
        {
            AnimateNumberWithPlus(text, bonus, label, countTime);
        });
        seq.AppendInterval(countTime);
        seq.AppendCallback(() =>
        {
            setter(newValue);
        });
    }

    seq.AppendCallback(() =>
    {
        // кнопка: одновременно scale, fade и легкий прыжок
        _finishDayButton.localScale = Vector3.one * 0.7f;
        buttonGroup.DOFade(1f, 0.3f);
        _finishDayButton.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutBack);
        _finishDayButton.DOPunchScale(Vector3.one * 0.15f, 0.4f, 10, 1);
        MusicController.Instance.PlaySpecificSound(_finishDaySound);
    });
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
      int currentValue = 0;
      
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
      {
         HandleWarning(warningText);
         MusicController.Instance.PlaySpecificSound(_warningSound);
      }
   }

   public void ShowPopup(Popups popup)
   {
      if (_popups.TryGetValue(popup, out var popupPanel))
      {
         HandlePopup(popupPanel);
         MusicController.Instance.PlaySpecificSound(_popupSound);
      }
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
