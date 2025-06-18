using DG.Tweening;
using UnityEngine;

public enum ActionType
{
    Launch,
    Organize,
    Clear
}

public class ActionsManager : MonoBehaviour
{
    private ActionType _actionType;

    public void SetActionTypeLaunch()
    {
        _actionType = ActionType.Launch;
        UIManager.Instance.ShowPopup(Popups.PRPopupSure);
    }
    public void SetActionTypeOrganize()
    {
        _actionType = ActionType.Organize;
        UIManager.Instance.ShowPopup(Popups.PRPopupSure);
    }
    public void SetActionTypeClear()
    {
        _actionType = ActionType.Clear;
        UIManager.Instance.ShowPopup(Popups.PRPopupSure);
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
            }
        }));
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
