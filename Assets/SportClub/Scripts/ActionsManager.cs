using DG.Tweening;
using UnityEngine;

public enum ActionType
{
    Launch,
    Organize,
    Clear,
    Appoint,
    Restructure,
    Calm,
    Merch,
    Popup
}

public class ActionsManager : MonoBehaviour
{
    private ActionType _actionType;

    public void SetActionTypeLaunch()
    {
        SetActionHandler(ActionType.Launch, Popups.PRPopupSure);
    }
    public void SetActionTypeOrganize()
    {
        SetActionHandler(ActionType.Organize, Popups.PRPopupSure);
    }
    public void SetActionTypeClear()
    {
        SetActionHandler(ActionType.Clear, Popups.PRPopupSure);
    }

    public void SetActionTypeAppoint()
    {
        SetActionHandler(ActionType.Appoint, Popups.ManagementPopupSure);
    }
    public void SetActionTypeRestructure()
    {
        SetActionHandler(ActionType.Restructure, Popups.ManagementPopupSure);
    }
    public void SetActionTypeCalm()
    {
        SetActionHandler(ActionType.Calm, Popups.ManagementPopupSure);
    }
    public void SetActionTypeMerch()
    {
        SetActionHandler(ActionType.Merch, Popups.MerchPopupSure);
    }
    public void SetActionTypePopup()
    {
        SetActionHandler(ActionType.Popup, Popups.MerchPopupSure);
    }

    public void HandleActionType()
    {
        UIManager.Instance._loadingScreen.DOScale(Vector3.one, 0.15f);
        DOVirtual.DelayedCall(5f, (() =>
        {
            UIManager.Instance._loadingScreen.DOScale(Vector3.zero, 0.15f);

            switch (_actionType)
            {
                case ActionType.Launch:
                    ActivateActionBuff(BuffType.ViralBuff, BuffType.NegativeReactionBuff);
                    break;
                case ActionType.Organize:
                    ActivateActionBuff(BuffType.FansUnited, BuffType.FansConflitct);
                    break;
                case ActionType.Clear:
                    ActivateActionBuff(BuffType.CleanStreets, BuffType.CleanStreets);
                    break;
                case ActionType.Appoint:
                    ActivateActionBuff(BuffType.NewVoice, BuffType.Radical);
                    break;
                case ActionType.Restructure:
                    ActivateActionBuff(BuffType.OrderRestored, BuffType.FansPushBack);
                    break;
                case ActionType.Calm:
                    ActivateActionBuff(BuffType.CoolerHeads, BuffType.SeenAsWeek);
                    break;
                case ActionType.Merch:
                    ActivateActionBuff(BuffType.Sold, BuffType.Cold);
                    break;
                case ActionType.Popup:
                    ActivateActionBuff(BuffType.Payday, BuffType.Vandalized);
                    break;
            }
        }));
    }

    private void SetActionHandler(ActionType actionType, Popups popup)
    {
        _actionType = actionType;
        UIManager.Instance.ShowPopup(popup);
    }
    
    private void ActivateActionBuff(BuffType goodType, BuffType badType)
    {
        int randomBuff = Random.Range(0, 100);
        
        if (randomBuff < 50)
        {
            BuffsManager.Instance.AddBuff(goodType);
        }
        else
        {
            BuffsManager.Instance.AddBuff(badType);
        }
    }
}
