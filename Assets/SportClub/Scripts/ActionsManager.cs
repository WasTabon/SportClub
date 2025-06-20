using DG.Tweening;
using UnityEngine;

public enum ActionType
{
    Launch,
    Organize,
    Clear,
    Appoint,
    Restructure,
    Calm
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
            }
            
            //додати бафи для management 
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
