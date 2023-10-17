using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CtrGame : CtrBase
{
    static CtrGame _instance;

    public static CtrGame instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CtrGame>();
            }

            return _instance;
        }
    }
    
    [HideInInspector] public bool isStart = false;
    [HideInInspector] public bool isGameOver = false;
    [HideInInspector] public int turnScore;
    [HideInInspector] public int turnCount = 1;
    [HideInInspector] public bool isContinue = false;
    [HideInInspector] public int comboCount = 0;
    [HideInInspector] public bool isAllClear = false;
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip clip;
    public TiltCamera tiltCamera;
    public ButtonRocket buttonRocket;
    public ButtonRocketArcade buttonRocketArcade;
    private int shotSoundCount = 0;
    private bool isLock = false;
    public bool isClassicGame = false;
    [HideInInspector] public bool isCompleteLevel = false;
    [HideInInspector] public int levelNumber = 1;
    public GameObject imageBG;
    public Sprite[] backgroundSprite;
    public PaddleController paddleController;


    //Screen Drag Lock
    public bool IsLock
    {
        get
        {
            if (!isStart)
            {
                return true;
            }

            if (Player.instance.activeBall.Count > 0) return true;
            if (isGameOver) return true;
            return isLock;
        }
        set { isLock = value; }
    }


    private void Awake()
    {
        IsLock = true;

        levelNumber = PlayerPrefs.GetInt("levelNumber");
        //Initialize to set play record once
        PlayManager.Instance.countPlay = 0;
        PlayManager.Instance.countBreakeBrick = 0;
        PlayManager.Instance.countAllClear = 0;
        PlayManager.Instance.countLuckyBonus = 0;
        PlayManager.Instance.countHighestCombo = 0;

        if (isClassicGame)
        {
            if (PlayManager.Instance.isSaveGameStart)
            {
                //When playing from the middle
                PlayManager.Instance.isSaveGameStart = false;

                turnCount = GameData.Save_Turn;
                PlayManager.Instance.score = GameData.Save_Score;

                CtrUI.instance.textTurn.text = turnCount.ToString();
                CtrUI.instance.textScore.text = Utility.ChangeThousandsSeparator(GameData.Save_Score);
                Player.instance.ballCount = turnCount;
                Player.instance.ballMaxCount = turnCount;

                if (turnCount >= buttonRocket.ReloadMaxCount)
                {
                    buttonRocket.ReloadCount = buttonRocket.ReloadMaxCount;
                    buttonRocket.Reload();
                }
                else
                {
                    buttonRocket.ReloadCount = turnCount;
                    buttonRocket.SetFillAmount();
                }
            }
            else
            {
                //First
                PlayManager.Instance.score = 0;
            }
        }
        else
        {
            PlayManager.Instance.score = 0;
            CtrUI.instance.textTurn.text = levelNumber.ToString();
        }
    }

    IEnumerator Start()
    {
        PlayManager.Instance.commonUI._CoinGem.Hide();
        // SoundManager.Instance.PlayBGM(SoundList.sound_play_bgm);
        // set background image
        int indexPic = Random.Range(0, backgroundSprite.Length);
        imageBG.GetComponent<Image>().sprite = backgroundSprite[indexPic];

        Player.instance.SetData();
        yield return new WaitForSeconds(0.01f);
        if (isClassicGame)
        {
            CtrBlock.instance.SpwanBlock(0, turnCount);
        }
        else
        {
            CtrBlock.instance.SpwanBlock(0, levelNumber * 2 + 1);
            CtrBlock.instance.SpwanBlock(1, levelNumber * 2);
            CtrBlock.instance.SpwanBlock(2, levelNumber * 2 - 1);
            if(levelNumber > 5)CtrBlock.instance.SpwanBlock(3, levelNumber * 2 - 2);
            if(levelNumber > 10) CtrBlock.instance.SpwanBlock(4, levelNumber * 2 - 3);
            if(levelNumber > 20) CtrBlock.instance.SpwanBlock(5, levelNumber * 2 - 4);
        }

        yield return new WaitForSeconds(0.5f);
        isStart = true;
        IsLock = false;
    }


    //Next turn
    public void NextTurn()
    {
        isLock = true;
        turnCount += 1;

        CtrUI.instance.SetTurn(turnCount);
        StartCoroutine(NextTurnCo());
    }

    IEnumerator NextTurnCo()
    {
        CtrUI.instance.AddScore(turnScore);
        yield return new WaitForSeconds(0.2f);
        CtrBlock.instance.NextTurn();
    }



    public void NextTurnMoveEnd()
    {
        if (isGameOver) return;

        //All clear check
        if (CtrUI.instance._ComboEffectText.isAllClear)
        {
            isAllClear = true;
            CtrUI.instance._ComboEffectText.isAllClear = false;
        }
        else
        {
            CtrUI.instance._ComboEffectText.allClearCount = 0;
            isAllClear = false;
        }


        CtrUI.instance.NextTurnReady();

        turnScore = 0;
        comboCount = 0;
        IsLock = false;
        Player.instance.guideLine.GuidelineOn();
    }

    //Continue
    public void Continue()
    {
        if (isContinue) return;
        isContinue = true;

        SoundManager.Instance.ResumeBGM();
        CtrUI.instance._PopupContinue.Close();
        StartCoroutine(ContinueCo());
    }

    IEnumerator ContinueCo()
    {
        CtrBlock.instance.DestroyContinueBlock();
        yield return new WaitForSeconds(0.1f);
        Player.instance.ContinuePlayer();
    }

    //GameOver
    public void GameOver()
    {
        Debug.Log("GameOver");
        isGameOver = true;
        PlayManager.Instance.turn = turnCount;
        SoundManager.Instance.PauseBGM();

        if (isContinue)
        {
            PlayManager.Instance.LoadScene(Data.scene_result);
        }
        else
        {
            CtrUI.instance._PopupContinue.Open();
        }
        // phá hủy pađle ở đây
         
    }


    public void ShotSound()
    {
        if (shotSoundCount > 2) return;
        shotSoundCount++;
        audio.volume = Data.VolumeEffect;
        audio.PlayOneShot(clip);

        StartCoroutine(RemoveSoundCo(clip.length));
    }


    IEnumerator RemoveSoundCo(float time)
    {
        yield return new WaitForSeconds(time);
        if (shotSoundCount > 0) shotSoundCount -= 1;
    }

    public void SetFullRocket()
    {
        buttonRocketArcade.ReloadCount = 100;
        buttonRocketArcade.SetFillAmount();
    }
}