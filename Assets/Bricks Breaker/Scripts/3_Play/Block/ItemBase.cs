using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ItemBase : MonoBehaviour {
    [HideInInspector] public BlockGroup blockGroup;
    public SpriteRenderer mySprite; 
    [HideInInspector] public bool isBall = false; 
    [HideInInspector] public bool isDestory = false;

    public virtual void SetData (int itemId) {
        isDestory = false; 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            HitFx(SoundList.sound_play_sfx_ball_comback);
            InDamage(collision);
            // xữ lý với các loại item
           switch(gameObject.GetComponent<Item>().itemType)
            {
                case 1:
                    // bomp
                    CtrBlock.instance.DestroyAllBlock();
                    break;
                case 2:
                    // shost head
                    CtrGame.instance.isGameOver = true;
                    CtrUI.instance._PopupFinishLevel.Open();
                    break;
                case 3:
                    // rocket
                    CtrGame.instance.SetFullRocket(); 
                    break;
                case 4:
                    // sword
                    break;
                default:
                    // code block
                    break;
            } 
        }
        else if (collision.gameObject.CompareTag("InTrigger"))
        {
            HitFx(SoundList.sound_play_sfx_block2_destory);
            InDamage(collision);
            
        }
    }
    private void OnCollisionEnter2D (Collision2D collision) {
        if (isDestory) return;

        
    }

    public virtual void InDamage (Collider2D collision) {
        Destory();
       // CtrBlock.instance.CheckAllClear();
    }

    //바닥 라인에 닿았을경우
    //public void OnTriggerEnter2D (Collider2D collision) {
    //    if (collision.gameObject.CompareTag("InTrigger")) {
    //        if (!CtrGame.instance.isGameOver) {
    //            CtrGame.instance.isGameOver = true;
    //            Debug.Log("InTrigger");
    //            CtrGame.instance.GameOver();
    //        }

    //        Destory();
    //    }
    //}

    //블럭 파괴
    public virtual void Destory () {
        if (isDestory) return;
        isDestory = true;

        //if (!isBall) {
        //    //공이 아닐경우에는 블럭 이펙트 및 블럭 파괴 사운드 재생
            PoolManager.Despawn(PoolManager.Spawn(CtrPool.instance.pFxBlockBoom, transform.position, Quaternion.identity), 1f);
             
        //} 
        //초기화
        Reset();
       // blockGroup.blockBases.Remove(this);
        PoolManager.Despawn(this.gameObject);
    }

    public virtual void Reset () { }
    public virtual void HitFx (string soundName) {
        SoundManager.Instance.PlayEffect(soundName);
    }
}
