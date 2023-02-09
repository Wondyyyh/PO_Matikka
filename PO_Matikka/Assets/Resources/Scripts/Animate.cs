using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{

    public GameObject progressbar1;
    public GameObject progressbar2;
    public GameObject progressbar3;
    public GameObject progressbar4;
    public GameObject progressbar5;
    public GameObject progressbar6;

    public EasingFunction.Ease EasingType;

    private EasingFunction.Function EasingFun;

    [Range(0,100)]
    public float animTime = 5.0f;
    private float accu = 0.0f;

    private bool IsAnimating = false;

    // Start is called before the first frame update
    void Start()
    {
        EasingFun = EasingFunction.GetEasingFunction(EasingType);
    }

    void AnimateIt()
    {
        accu += Time.deltaTime;

        if (accu > animTime)
        {
            IsAnimating = false;
            return;
        }

        float t = accu / animTime;

        // Debug.Log(t);
        progressbar1.GetComponent<ProgressBar>().setFraction(t);
        progressbar2.GetComponent<ProgressBar>().setFraction(t);
        progressbar3.GetComponent<ProgressBar>().setFraction(t);
        progressbar4.GetComponent<ProgressBar>().setFraction(t);
        progressbar5.GetComponent<ProgressBar>().setFraction(t);
        progressbar6.GetComponent<ProgressBar>().setFraction(t);


    }

    // Update is called once per frame
    void Update()
    {

        // kutsutaan functiota ja asetetaan range sen mukaan
        // float tval = EasingFun(0, 100, accu / 100f);

        // Debug.Log(tval);

        if (IsAnimating)
        {
            AnimateIt();
        }

        else
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                IsAnimating = true;
                accu = 0.0f;
            }
        }

    }
}
