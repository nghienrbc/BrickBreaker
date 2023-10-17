using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    static PaddleController _instance;

    public static PaddleController instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PaddleController>();
            }

            return _instance;
        }
    }
    public int numberOfTouchBall = 0;
    public int numberOfTimeToReloadRocket = 0;
    public ButtonRocketArcade _ButtonRocketArcade;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            numberOfTouchBall += 1;
            //if(numberOfTouchBall % numberOfTimeToReloadRocket == 0)  
            _ButtonRocketArcade.CheckRocketCoolTime();
        }
    }
}
