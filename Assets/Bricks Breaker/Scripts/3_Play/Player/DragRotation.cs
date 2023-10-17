using UnityEngine;
using UnityEngine.EventSystems;

public class DragRotation : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {

    bool isInteractable = false;
    public Transform playerCenter;
    public Transform paddle;
    private Vector3 screenPoint;
    private Vector3 offset;
    private bool isShotBall = false;

    #region - DragPanel
    public void OnPointerDown (PointerEventData data) {
        if (playerCenter == null) return;
        if(CtrGame.instance.IsLock) return;

        //if (CtrGame.instance.isGameOver || !CtrGame.instance.isGameStart) return;
        //if (!CtrGame.instance.PlayerReady) return;
        isInteractable = true;

        Vector3 diff = Camera.main.ScreenToWorldPoint(data.position) - playerCenter.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        playerCenter.rotation = Quaternion.Euler(0f, 0f, Mathf.Clamp((rot_z - 90), -83, 83));

        if (!CtrGame.instance.isClassicGame)
        {
            screenPoint = Camera.main.WorldToScreenPoint(paddle.position);
            offset = paddle.position - Camera.main.ScreenToWorldPoint(data.position);
        }
    }

    //Vector2 dragging;
    public void OnDrag (PointerEventData data) {
        if (!CtrGame.instance.isClassicGame && isShotBall && !CtrGame.instance.isGameOver && !CtrGame.instance.isCompleteLevel) {
            Vector3 curScreenPoint = data.position;
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint);// + offset;
            paddle.position = new Vector3(curPosition.x, paddle.position.y, paddle.position.z);
        }
        if (CtrGame.instance.IsLock) return;
        if (playerCenter == null) return;
        //if (CtrGame.instance.isGameOver || !CtrGame.instance.isGameStart) return;
        if (!isInteractable) return;
        Vector3 diff = Camera.main.ScreenToWorldPoint(data.position) - playerCenter.position;
        diff.Normalize(); 
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        playerCenter.rotation = Quaternion.Euler(0f, 0f, Mathf.Clamp((rot_z - 90), -83, 83));
    }

    public void OnPointerUp (PointerEventData data) {
        if(CtrGame.instance.IsLock) return;
        if (playerCenter == null) return;
        //if (CtrGame.instance.isGameOver || !CtrGame.instance.isGameStart) return;
        if (!isInteractable) return;
        isInteractable = false;

        Player.instance.ShotBall();
        isShotBall = true;


    }
    #endregion

}





