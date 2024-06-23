using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IPlayerState
{
    void EnterState(PlayerCtrl player);
    void UpdateState(PlayerCtrl player);
    void ExitState(PlayerCtrl player);

}