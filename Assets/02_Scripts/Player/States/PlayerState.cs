using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class PlayerState
{
    BasePlayer player = null;
    PlayerState(BasePlayer player)
    {
        this.player = player;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public abstract void HandleInput();
    public virtual void OnCollisionEnterState(BasePlayer player, Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Walkable"))
        {
            player.TransitionToState(player.IdleState);
        }
    }

}