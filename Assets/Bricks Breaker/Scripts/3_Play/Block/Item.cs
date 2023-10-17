using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item : ItemBase
{
    public SpriteRenderer shadow;
    public ParticleSystem fxHit;
    public int itemType = 0; // 1: bomp nổ toàn bộ block, 2: ghosthead: game over, 3: roket: reload, 4: sword: kiếm: chưa có


    private float bSpriteCount;
    private float valueB = 0.1f;
    private float valueA = 0.11f;

    private void OnEnable()
    {
        //Reset();
    }

    public override void Reset()
    {
        shadow.DOKill();
        shadow.DOFade(0f, 0f);
        mySprite.DOKill();
        mySprite.DOFade(0f, 0f);

        mySprite.transform.DOKill();
        mySprite.transform.DOScale(1f, 0f);

        transform.DOKill();
        transform.DOLocalMoveY(0.3f, 0f);
    }


    public override void SetData(int itemID)
    {
        //int turn = CtrGame.instance.turnCount; 

        mySprite.sprite = CtrItem.instance.itemSprites[itemID-1];
        itemType = itemID;
        //InAnimation();
        base.SetData(itemID);
    }

    void InAnimation()
    { 
        mySprite.DOFade(1f, 0.2f).SetEase(Ease.OutCubic);
        transform.DOLocalMoveY(0f, 0.2f).SetEase(Ease.OutCubic);
    }

    public override void InDamage(Collider2D collision)
    {
        base.InDamage(collision); 
    }

    public override void HitFx(string soundName)
    {      
        fxHit.Play(); 

        mySprite.DOKill();
        mySprite.transform.DOScale(0.95f, 0.1f).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            mySprite.transform.DOScale(1f, 0.1f).SetEase(Ease.OutCirc);
        });
        //spriteHit.DOKill();
        //spriteHit.DOFade(0f, 0f);
        //spriteHit.DOFade(1f, 0.05f).SetEase(Ease.OutCubic).OnComplete(()=> {
        //    spriteHit.DOFade(0f, 0.1f).SetEase(Ease.Linear);
        //});

        base.HitFx(soundName);
    }

}