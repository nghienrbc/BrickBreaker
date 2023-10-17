using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonRocketArcade : MonoBehaviour {

    bool isRocketReady = false;
    float reloadCount = 0;

#if UNITY_EDITOR
    float reloadMaxCount = 5;
#else
    float reloadMaxCount = 5;
#endif

    public Image rocketFiilAmount;  
    public ParticleSystem fxRocketOn;
    public Image imageRocket;

    public float ReloadCount {
        get {
            return reloadCount;
        }
        set {
            reloadCount = value;
        }
    }

    public float ReloadMaxCount {
        get {
            return reloadMaxCount;
        }
        set {
            reloadMaxCount = value;
        }
    } 

    public void Click_Rocket () {
        if (!isRocketReady) return;
        isRocketReady = false;

        SoundManager.Instance.PlayEffect(SoundList.sound_rocket_sfx_ready); 

        reloadCount = reloadMaxCount;         

        Click_Rocket_Area();
    }


    public void CheckRocketCoolTime () {
        if (isRocketReady) return;

        //Progress settings
        reloadCount += 1;
        SetFillAmount();
    }

    public void SetFillAmount () {
        rocketFiilAmount.DOFillAmount(reloadCount / reloadMaxCount, 0.15f).SetEase(Ease.InOutCubic);

        if (reloadCount >= reloadMaxCount) {
            SoundManager.Instance.PlayEffect(SoundList.sound_rocket_sfx_cool);
            Reload();
        } 
    }

    public void Reload () {
        //Reload completed
        reloadCount = reloadMaxCount;
        rocketFiilAmount.transform.GetChild(0).gameObject.SetActive(true);
        imageRocket.DOFade(1f, 0f);
        isRocketReady = true;
        fxRocketOn.Play();
    }


    public void Cancel () { 
        //Turn on the rocket button
        gameObject.SetActive(true);

        //Reload
        Reload();
    }


    public void Click_Rocket_Area() {

        SoundManager.Instance.PlayEffect(SoundList.sound_rocket_sfx_launch); 
        //Button and Progress Settings
        rocketFiilAmount.transform.GetChild(0).gameObject.SetActive(false);
        imageRocket.DOFade(0.5f, 0f);

        //Cool time start setting
        rocketFiilAmount.fillAmount = 0;
        reloadCount = 0;


        SetFillAmount();


        //Turn on the button
        gameObject.SetActive(true); 

        //Rocket launch 

        GameObject roket = PoolManager.Spawn(CtrPool.instance.pRoket, PaddleController.instance.transform.position, Quaternion.identity);
        roket.GetComponent<Roket>().SetRoket(); 

    }
}
