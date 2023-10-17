using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Advertisements;

public class PanelLevelSkinList : MonoBehaviour {
      
    public TextMeshProUGUI textName;  
    public Image star1;
    public Image star2;
    public Image star3;
    public Image imageLock;
    public Sprite starSelect;

    public LevelSkinData levelSkinData; 

    
    public void SetList () {
        //List initialization 

        GetComponent<Button>().onClick.AddListener(() => { Click_List(); });

        //Ball name setting
        textName.text = levelSkinData.LevelName;

        //Unlock check
        //if (levelSkinData.id == 0) {
        //levelSkinData.isUnlock = true;
        // } else {
        //    levelSkinData.isUnlock = PlayerPrefs2.GetBool(string.Format("ball {0}", levelSkinData.id));
        //}
        if (levelSkinData.isUnlock)
        {
            imageLock.sprite = levelSkinData.levelSprite;
            imageLock.transform.localScale = new Vector3(1.64f, 1f, 1f);

            // set star
            SetImageStar(levelSkinData.star);
        }
    }

    private void SetImageStar(int star)
    {
        if (star == 1)
        {
            star1.GetComponent<Image>().sprite = starSelect;
        }
        else if (star == 2)
        {
            star1.GetComponent<Image>().sprite = starSelect;
            star2.GetComponent<Image>().sprite = starSelect;
        }
        else if (star == 3)
        {
            star1.GetComponent<Image>().sprite = starSelect;
            star2.GetComponent<Image>().sprite = starSelect;
            star3.GetComponent<Image>().sprite = starSelect;
        }
    }

    //Click list
    public void Click_List () {
        if (levelSkinData.isUnlock) {
            //언락 
            CtrHome ctrHome = PlayManager.Instance.currentBase as CtrHome;
            int levelNumber = int.Parse(levelSkinData.LevelName);
            PlayerPrefs.SetInt("levelNumber", levelNumber);
            //PlayerPrefs.SetInt("levelHadUnlock", levelNumber);
            ctrHome.ClickPlay(Data.game_type_arcade);
            SoundManager.Instance.PlayEffect(SoundList.sound_skin_btn_equip);
        } else {
            //언락 아님 
        }
    } 

    /// <summary>
    /// Unlock Ballskin with ID
    /// </summary>
    void BallUnlockByID () {
        PlayerPrefs2.SetBool(string.Format("ball {0}", levelSkinData.id), true);
    }
}
