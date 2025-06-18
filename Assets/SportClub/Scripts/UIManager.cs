using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

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
   PRPopupSure
}

public class UIManager : MonoBehaviour
{
   public static UIManager Instance;

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
   }

   public void ClosePanel(RectTransform panel)
   {
      panel.DOScale(Vector3.zero, _timeClosePanel);
   }

   public void OpenPanel(RectTransform panel)
   {
      panel.DOScale(Vector3.one, _timeOpenPanel);
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
      rectTransform.DOScale(Vector3.one, _timePopup)
         .OnComplete(() =>
         {
            
         });
   }

   private void ResetAllWarnings()
   {
      foreach (var warning in _warnings.Values)
         warning.DOFade(0f, 0f);
   }

   private void ResetAllPopups()
   {
      foreach (var popup in _popups.Values)
         popup.DOScale(Vector3.zero, 0f);
   }
}
