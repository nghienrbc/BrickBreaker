using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BlockBall : BlockBase
{


    public ParticleSystem fxLoop;
    public ParticleSystem fxGet;

    private void OnEnable()
    {
        Reset();
    }

    public override void Reset()
    {
        //mySprite.transform.DOKill();
        //mySprite.transform.DOScale(0f, 0f);
    }

    public override void SetData(int health, int itemId = 0)
    {
        GetComponent<CircleCollider2D>().enabled = true;
        //mySprite.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        transform.DOLocalMoveY(0f, 0f);
        fxLoop.gameObject.SetActive(true);
        base.SetData(health, itemId);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDestory) return;

        if (collision.CompareTag("Ball"))
        {
#if UNITY_IOS
            if (PlayManager.Instance.isHaptic) 
            {
                //iOSHapticFeedback.Instance.Trigger(iOSHapticFeedback.iOSFeedbackType.SelectionChange);
                //0 : small 1 : light 2 : midium 3 : heavy 4 : success 5 : warring 6 : falure 7 : onoff 
            }
#endif
            blockGroup.blockBases.Remove(this);
            CtrBlock.instance.CheckAllClear();
            fxGet.Play();
            GetComponent<CircleCollider2D>().enabled = false;
            PoolManager.Despawn(PoolManager.Spawn(CtrPool.instance.pFxBallGet, transform.position, Quaternion.identity),
                1f);
            fxLoop.gameObject.SetActive(false);
            SoundManager.Instance.PlayEffect(SoundList.sound_play_sfx_addball);

            if (CtrGame.instance.isClassicGame)
            {                
                Player.instance.addBallBlock.Add(this);
                transform.SetParent(CtrGame.instance.transform);
                float value = (transform.position.y - Player.instance.nextPosition.y) * 0.1f;
                transform.DOMoveY(-4.213f, value).SetEase(Ease.InCubic).OnComplete(() => { });
            }
            else
            {
                isDestory = true;
                int levelNumber = PlayerPrefs.GetInt("levelNumber");
                // khi ball chạm vào thì biến blockball thành 1 ball mới và rơi xuống để paddle tiếp tục xài
                Ball ball = PoolManager.Spawn(Player.instance.selectBall, transform.position, Quaternion.identity).GetComponent<Ball>();
                Player.instance.activeBall.Add(ball);
                ball.SetDataInArcadeMode(levelNumber > 3 ? (int)levelNumber/3 : 1); //damage = levelNumber
                Destory();
                mySprite.sprite = null;
            }
        }
    }
}
