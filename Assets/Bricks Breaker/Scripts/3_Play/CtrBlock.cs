using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class CtrBlock : MonoBehaviour
{
    static CtrBlock _instance;

    public static CtrBlock instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CtrBlock>();
            }

            return _instance;
        }
    }

    public Transform[] transfomY;
    public List<Transform> firstTransform = new List<Transform>();
    [HideInInspector] public List<BlockGroup> blockGroups = new List<BlockGroup>();

    public Sprite[] blockSprites;

    public Sprite GetBlockSprite
    {
        get { return blockSprites[Random.Range(0, blockSprites.Length)]; }
    }

    //Random shuffle slots
    public void ShuffleSlot()
    {
        firstTransform = Utility.ShuffleList(firstTransform);
    }

    //Create a block on the line
    public void SpwanBlock(int numY = 0, int health = 0)
    {
        //Create a single block
        BlockGroup blockGroup = PoolManager
            .Spawn(CtrPool.instance.pBlockGroups, transfomY[numY].position, Quaternion.identity)
            .GetComponent<BlockGroup>();
        blockGroups.Add(blockGroup);
        blockGroup.SetBlockGroup(numY, health);
    }

    //Next turn
    public void NextTurn(float time = 0.2f)
    {
        SoundManager.Instance.PlayEffect(SoundList.sound_play_sfx_block_down);
        StartCoroutine(NextTurnCo(time));
    }

    IEnumerator NextTurnCo(float time)
    {
        for (int i = 0; i < blockGroups.Count; i++)
        {
            blockGroups[i].Move(time);
        }

        yield return new WaitForSeconds(time);
        //Create a single block
        SpwanBlock(0, CtrGame.instance.turnCount);

        //End of turn movement
        CtrGame.instance.NextTurnMoveEnd();
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

    public void DestroyAllBlock()
    {
        for (int i = 0; i < blockGroups.Count; i++)
        {
            while (blockGroups[i].blockBases.Count > 0)
            {
                for (int j = 0; j < blockGroups[i].blockBases.Count; j++)
                {
                    if (blockGroups[i].blockBases[j].isDestory == false)
                        blockGroups[i].blockBases[j].Destory();
                }
            }
        }
        CtrBlock.instance.CheckAllClear();
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
