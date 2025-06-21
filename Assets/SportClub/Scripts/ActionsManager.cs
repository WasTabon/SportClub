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
    Popup,
    Leak,
    Fake,
    Steal
}

public enum CostType
{
    PR,
    Management,
    Merch,
    Sabotage
}

public class ActionsManager : MonoBehaviour
{
    private ActionType _actionType;
    private CostType _costType;

    public void SetActionTypeLaunch()
    {
        SetActionHandler(ActionType.Launch, Popups.PRPopupSure);
        _costType = CostType.PR;
    }
    public void SetActionTypeOrganize()
    {
        SetActionHandler(ActionType.Organize, Popups.PRPopupSure);
        _costType = CostType.PR;
    }
    public void SetActionTypeClear()
    {
        SetActionHandler(ActionType.Clear, Popups.PRPopupSure);
        _costType = CostType.PR;
    }

    public void SetActionTypeAppoint()
    {
        SetActionHandler(ActionType.Appoint, Popups.ManagementPopupSure);
        _costType = CostType.Management;
    }
    public void SetActionTypeRestructure()
    {
        SetActionHandler(ActionType.Restructure, Popups.ManagementPopupSure);
        _costType = CostType.Management;
    }
    public void SetActionTypeCalm()
    {
        SetActionHandler(ActionType.Calm, Popups.ManagementPopupSure);
        _costType = CostType.Management;
    }
    public void SetActionTypeMerch()
    {
        SetActionHandler(ActionType.Merch, Popups.MerchPopupSure);
        _costType = CostType.Merch;
    }
    public void SetActionTypePopup()
    {
        SetActionHandler(ActionType.Popup, Popups.MerchPopupSure);
        _costType = CostType.Merch;
    }
    public void SetActionTypeLeak()
    {
        SetActionHandler(ActionType.Leak, Popups.SabotagePopupSure);
        _costType = CostType.Sabotage;
    }
    public void SetActionTypeFake()
    {
        SetActionHandler(ActionType.Fake, Popups.SabotagePopupSure);
        _costType = CostType.Sabotage;
    }
    public void SetActionTypeSteal()
    {
        SetActionHandler(ActionType.Steal, Popups.SabotagePopupSure);
        _costType = CostType.Sabotage;
    }

    public void HandleActionType()
    {
        if (_costType == CostType.PR)
        {
            if (ClubManager.Instance.GetMoney() - 50 < 0)
            {
                UIManager.Instance.ShowPopup(Popups.NotEnoughResources);
                return;
            }
            else
            {
                ClubManager.Instance.SetMoney(ClubManager.Instance.GetMoney() - 50);
            }
        }
        else if (_costType == CostType.Management)
        {
            if (ClubManager.Instance.GetHype() - 10 < 0)
            {
                UIManager.Instance.ShowPopup(Popups.NotEnoughResources);
                return;
            }
            else
            {
                ClubManager.Instance.SetHype(ClubManager.Instance.GetHype() - 10);
            }
        }
        else if (_costType == CostType.Merch)
        {
            if (ClubManager.Instance.GetMoney() - 100 < 0)
            {
                UIManager.Instance.ShowPopup(Popups.NotEnoughResources);
                return;
            }
            else
            {
                ClubManager.Instance.SetMoney(ClubManager.Instance.GetMoney() - 100);
            }
        }
        else if (_costType == CostType.Sabotage)
        {
            if (ClubManager.Instance.GetFans() - 5 < 0)
            {
                UIManager.Instance.ShowPopup(Popups.NotEnoughResources);
                return;
            }
            else
            {
                ClubManager.Instance.SetFans(ClubManager.Instance.GetFans() - 5);
            }
        }
        
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
                case ActionType.Leak:
                    ActivateActionBuff(BuffType.Scandal, BuffType.Caught);
                    break;
                case ActionType.Fake:
                    ActivateActionBuff(BuffType.Ratings, BuffType.Investigation);
                    break;
                case ActionType.Steal:
                    ActivateActionBuff(BuffType.Fans, BuffType.Backlash);
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
