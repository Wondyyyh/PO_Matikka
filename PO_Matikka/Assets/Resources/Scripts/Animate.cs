using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{

    public EasingFunction.Ease EasingType;

    private EasingFunction.Function EasingFun;

    [Range(0,100)]
    public int value = 0;

    // Start is called before the first frame update
    void Start()
    {
        EasingFun = EasingFunction.GetEasingFunction(EasingType);
    }

    // Update is called once per frame
    void Update()
    {

        //kutsutaan functiota ja asetetaan range sen mukaan
        float tval = EasingFun(0, 100, value / 100f);

        Debug.Log(tval);

    }
}
