using UnityEngine;
using UnityEngine.EventSystems;

public class RunState : PlayerState
{
    public override void EnterState(BasePlayer player)
    {
        player.animator.SetBool(player.HashRun, true);
    }

    public override void UpdateState()
    {

        HandleInput();
        GameObject camera = player.camera;
        Vector3 cameraForward = camera.transform.forward;
        cameraForward.y = 0;
        cameraForward = cameraForward.normalized;

        float currentInputHor = Input.GetAxis("Horizontal");
        float currentInputVer = Input.GetAxis("Vertical");

        Vector3 moveVer = cameraForward * currentInputVer;
        Vector3 moveHor = camera.transform.right * currentInputHor;

        player.moveDirection = (moveVer + moveHor).normalized;

        player.isMoving = player.moveDirection != Vector3.zero;

        if (player.isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(player.moveDirection);
            player.PlayerTransform.rotation = Quaternion.Slerp(player.PlayerTransform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    public override void FixedUpdateState()
    {
        if (player.isMoving)
        {
            player.rb.MovePosition(player.rb.position + player.moveDirection * player.moveSpd * Time.fixedDeltaTime);
        }
    }

    public override void HandleInput()
    {
    }
}
