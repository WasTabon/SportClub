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
    }
    public void SetActionTypeOrganize()
    {
        _actionType = ActionType.Organize;
    }
    public void SetActionTypeClear()
    {
        _actionType = ActionType.Clear;
    }

    public void HandleActionType()
    {
        if (_actionType == ActionType.Launch)
        {
            
        }
    }
}
