using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CtrItem : MonoBehaviour
{
    static CtrItem _instance;

    public static CtrItem instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CtrItem>();
            }

            return _instance;
        }
    }
     
    public List<Transform> firstTransform = new List<Transform>();
    [HideInInspector] public List<BlockGroup> blockGroups = new List<BlockGroup>();

    public Sprite[] itemSprites;

    public Sprite GetBlockSprite
    {
        get { return itemSprites[Random.Range(0, itemSprites.Length)]; }
    }

    //Random shuffle slots
    public void ShuffleSlot()
    {
        firstTransform = Utility.ShuffleList(firstTransform);
    }

    //Create a block on the line
    public void SpwanItem(Vector3 blockPos, int numY = 0, int health = 0)
    {
        //Create a single block
        BlockGroup blockGroup = PoolManager
            .Spawn(CtrPool.instance.pBlockGroups, blockPos, Quaternion.identity)
            .GetComponent<BlockGroup>();
        blockGroups.Add(blockGroup);
        blockGroup.SetBlockGroup(numY, health);
    }

    
    //Delete the bottom 4 lines after reporting an ad
    public void DestroyContinueBlock()
    {
        SoundManager.Instance.PlayEffect(SoundList.sound_play_sfx_revival);
        blockGroups[0].Destory(true);
        blockGroups[0].Destory(true);
        blockGroups[0].Destory(true);
        blockGroups[0].Destory(true);
    }

    public void CheckAllClear()
    {
        if (CtrGame.instance.isClassicGame)
        {
            for (int i = 0; i < blockGroups.Count; i++)
            { 
                if (blockGroups[i].blockBases.Count > 0)
                {
                    return;
                }
            }
            CtrUI.instance.AllClear();
        }             
        else
        {
            for (int i = 0; i < blockGroups.Count; i++)
            {
                // nếu là game acrade thì không cần kiểm tra blockball có còn hay không
                for (int j = 0; j < blockGroups[i].blockBases.Count; j++)
                {
                    if (blockGroups[i].blockBases[j].GetComponent<Block>())
                    {
                        return;
                    }
                }
            }
            CtrUI.instance.AllClear();
            // nếu đã clear hết thì level complete, hiển thị UI PanelFinishLevel
            CtrGame.instance.isCompleteLevel = true;
            //
        }

    }
}
