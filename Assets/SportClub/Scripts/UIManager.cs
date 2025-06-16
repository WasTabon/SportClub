using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public enum Warnings
{
   EnterName,
   EnterEnglishName
}

public enum Popups
{
   NameEnter
}

public class UIManager : MonoBehaviour
{
   public static UIManager Instance;

   [SerializeField] private float _timeWarning;
   
   [Header("Warnings")]
   [SerializeField] private TextMeshProUGUI _warningEnterName;
   [SerializeField] private TextMeshProUGUI _warningEnterEnglishName;

   [Header("Popups")] 
   [SerializeField] private RectTransform _nameEnterPanel;
   
   private List<TextMeshProUGUI> _warnings;
   private List<RectTransform> _popups;

   private void Awake()
   {
      Instance = this;
      _warnings = new List<TextMeshProUGUI>();
      _popups = new List<RectTransform>();
      
      AddToList(_warnings, _warningEnterName, _warningEnterEnglishName);
      AddToList(_popups, _nameEnterPanel);
   }

   private void Start()
   {
      ResetAllWarnings();
      ResetAllPopups();
   }

   public void ShowWarning(Warnings warning)
   {
      ResetAllWarnings();

      switch (warning)
      {
         case Warnings.EnterName:
            FadeWarning(_warningEnterName);
            break;
         case Warnings.EnterEnglishName:
            FadeWarning(_warningEnterEnglishName);
            break;
      }
   }

   public void ShowPopup(Popups popup)
   {
      switch (popup)
      {
         case Popups.NameEnter:
            _nameEnterPanel.DOScale(Vector3.one, 0.5f);
            break;
      }
   }

   private void FadeWarning(TextMeshProUGUI text)
   {
      text.DOFade(1, _timeWarning)
         .OnComplete((() =>
         {  
            DOVirtual.DelayedCall(1f, () =>
            {
               text.DOFade(0, _timeWarning);
            });
         }));
   }
   
   private void AddToList<T>(List<T> targetList, params T[] items)
   {
      targetList.AddRange(items);
   }
   
   private void ResetAllWarnings()
   {
      foreach (TextMeshProUGUI warning in _warnings)
      {
         warning.DOFade(0f, 0f);
      }
   }
   private void ResetAllPopups()
   {
      foreach (RectTransform popup in _popups)
      {
         popup.DOScale(Vector3.zero, 0f);
      }
   }
}
