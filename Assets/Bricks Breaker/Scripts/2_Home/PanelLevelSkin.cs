using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//public enum CostType {
//    Default,
//    Coin,
//    Gem,
//    Ads
//}

[System.Serializable]
public class LevelSkinData {
    [HideInInspector]
    public int id;
    [HideInInspector]
    public bool isUnlock;
    public Sprite levelSprite;
    public string LevelName;  
    public int star;
}

public class PanelLevelSkin : PanelBase {

    public LevelSkinData[] LevelDatas;
    public GameObject pListSlot;
    public Transform listTransform;
    public PanelLevelSkinList selectLevelSkinList;
    public List<PanelLevelSkinList> panelLevelLists = new List<PanelLevelSkinList>();
    public Sprite[] ballSprites;

    /// <summary>
    /// Ball Skin Data Settings
    ///Set each list with the ball data.
    /// </summary>
    public override void SetData () {
        panelLevelLists.Clear();
    int levelHadUnlock = PlayerPrefs.GetInt("levelHadUnlock");
        var starArray = PlayerPrefs.GetString("starLevel").Split(new[] { "###" }, StringSplitOptions.None);

        if (levelHadUnlock == 0)
        {
            levelHadUnlock = 1;
            PlayerPrefs.SetInt("levelHadUnlock", levelHadUnlock);
        }
        for (int i = 0; i < LevelDatas.Length; i++) {
            LevelDatas[i].id = i;
            LevelDatas[i].isUnlock = i < levelHadUnlock;
            LevelDatas[i].levelSprite = ballSprites[i];
            if (starArray.Length > 0)
                LevelDatas[i].star = int.Parse(starArray[i]);

            GameObject obj = Instantiate(pListSlot);
            obj.transform.SetParent(listTransform, false);

            PanelLevelSkinList skinList = obj.GetComponent<PanelLevelSkinList>();
            skinList.levelSkinData = LevelDatas[i];
            skinList.SetList();

            panelLevelLists.Add(skinList);
        } 
       // panelLevelLists[GameData.SelectBallNum].Select();
    }
     

    /// <summary>
    /// Get the Ballskin Sprite by name.
    /// </summary>
    public Sprite GetBallSprite (string name) {
        Debug.Log(name);

        foreach (Sprite sprite in ballSprites) {

            if (name == sprite.name) {
                return sprite;
            }
        }
        return null;
    }
}


