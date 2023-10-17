using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using System;

public class PopupFinishLevel : MonoBehaviour
{

    public TextMeshProUGUI ribbonText;
    public TextMeshProUGUI textScore;
    public TextMeshProUGUI textLevelNumber;
    public CanvasGroup canvasGroup; 
    public GameObject popupBg; 

    public GameObject buttonReplay;
    public GameObject buttonHome;
    public GameObject buttonNext;

    public GameObject imageStar1;
    public GameObject imageStar2;
    public GameObject imageStar3;

    public Sprite starUnselect;
    public Sprite starSelect; 
    public bool isOn = false;

    /// <summary>
    /// Initialize all UI.
    /// </summary>
    public void UIReset()
    {
        canvasGroup.DOFade(0f, 0f); 
        popupBg.transform.DOScale(0f, 0f);

    }

    public void Open()
    {
        if (isOn) return;
        isOn = true;
        Time.timeScale = 0;
        SoundManager.Instance.PlayEffect(SoundList.sound_continue_sfx_default);
        if (CtrGame.instance.isGameOver)
        {
            ribbonText.text = "Level Fail";
            textScore.text = 0.ToString();
            buttonNext.GetComponent<ButtonBase>().enabled = false;
            var tempColor = buttonNext.GetComponent<Image>().color;
            tempColor.a = 0.5f; 
            buttonNext.GetComponent<Image>().color= tempColor;
        }
        else
        {
            int levelPlaying = PlayerPrefs.GetInt("levelNumber");
            int a = PlayerPrefs.GetInt("levelHadUnlock");
            // kiểm tra nếu level đang chơi lớn hơn level đã mở khóa thì set lại giá trị lưu cho level đã mở khóa 
            if (levelPlaying == PlayerPrefs.GetInt("levelHadUnlock"))
            {
                PlayerPrefs.SetInt("levelHadUnlock", levelPlaying + 1);
            }           
            ribbonText.text = "Level Complete";
            textScore.text = Utility.ChangeThousandsSeparator(CtrGame.instance.turnScore);
            // tính số sao
            int star = CalculateStar();
            SetImageStar(star);
            PaddleController.instance.numberOfTouchBall = 0;

            // lưu vào mảng chứa số sao 

            var starArray = PlayerPrefs.GetString("starLevel").Split(new[] { "###" }, StringSplitOptions.None);
            for (int i = 0; i < starArray.Length; i++)
            {
                if(i == levelPlaying - 1)
                    starArray[i] = star.ToString();
            }
            PlayerPrefs.SetString("starLevel", string.Join("###", starArray)); 
        }
        textLevelNumber.text = PlayerPrefs.GetInt("levelNumber").ToString();
        this.gameObject.SetActive(true);
        canvasGroup.DOKill();
       // canvasGroup.DOFade(1f, 0.15f).SetEase(Ease.OutCubic);
        canvasGroup.DOFade(1f, 0.15f).SetEase(Ease.OutCubic).SetUpdate(true);
        popupBg.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack);
        StartCoroutine(ScoreAnimCo());
        canvasGroup.DOKill();
    }

    private int CalculateStar()
    {
        // kiểm tra số lân chạm ball với paddle: nếu < 30 thì 3*, < 60 thì 2*, ngược lại 1*
        int numberOfTouchBetweenBallAndPaddle = PaddleController.instance.numberOfTouchBall;
        if (numberOfTouchBetweenBallAndPaddle <= 30) return 3;
        else if (numberOfTouchBetweenBallAndPaddle <= 60) return 2;
        return 1;
    }

    private void SetImageStar(int star)
    {
        if(star == 1)
        {
            imageStar1.GetComponent<Image>().sprite = starSelect;
        }
        else if (star == 2)
        {
            imageStar1.GetComponent<Image>().sprite = starSelect;
            imageStar2.GetComponent<Image>().sprite = starSelect;
        }
        else if (star == 3)
        {
            imageStar1.GetComponent<Image>().sprite = starSelect;
            imageStar2.GetComponent<Image>().sprite = starSelect;
            imageStar3.GetComponent<Image>().sprite = starSelect;
        }
    }



    IEnumerator ScoreAnimCo()
    {
        int score = 0;
        int targetScore = CtrGame.instance.turnScore;

        float time = 0.5f;
        DOTween.To(() => score, x => score = x, targetScore, time).SetEase(Ease.Linear);

        //SoundManager.Instance.PlayEffectLoop(SoundList.sound_result_sfx_score);

        while (time > 0)
        {
            time -=  Time.unscaledDeltaTime;
            textScore.text = Utility.ChangeThousandsSeparator(score);
            yield return null;
        }

        //SoundManager.Instance.StopEffectLoop();
        textScore.text = Utility.ChangeThousandsSeparator(targetScore);

        yield return new WaitForSeconds(1f); 
    }

    public void Close()
    {
        canvasGroup.DOKill();
        canvasGroup.DOFade(1f, 0.15f).SetEase(Ease.OutCubic).OnComplete(() => { this.gameObject.SetActive(false); });
    }


    bool isClick = false;

    public void Click_Home()
    {
        SoundManager.Instance.PlayEffect(SoundList.sound_common_btn_in);
        PlayManager.Instance.LoadScene(Data.scene_home);
    }

    public void Click_Again()
    {
        SoundManager.Instance.PlayEffect(SoundList.sound_common_btn_in);
        PlayManager.Instance.LoadScene(Data.scene_play_level);
        SoundManager.Instance.PlayEffect(SoundList.sound_play_sfx_in);
    }

    public void Click_Next()
    {
        SoundManager.Instance.PlayEffect(SoundList.sound_common_btn_in);
        int levelPlaying = PlayerPrefs.GetInt("levelNumber");
        PlayerPrefs.SetInt("levelNumber", levelPlaying + 1); 
        PlayManager.Instance.LoadScene(Data.scene_play_level);
        SoundManager.Instance.PlayEffect(SoundList.sound_play_sfx_in); 
    }
}
