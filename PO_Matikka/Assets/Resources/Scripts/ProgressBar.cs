using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
    using UnityEditor;
#endif

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{

#if UNITY_EDITOR
[MenuItem("GameObject/UI/MY Progress Bar")]
    public static void AddProgressBar()
    {
        Object o = PrefabUtility.InstantiatePrefab(Resources.Load<GameObject>("UI/MyProgressBar"), 
        Selection.activeGameObject.transform);
    }
#endif

    public float Max = 100;
    public float Min = 0;
    [SerializeField]
    private float current = 10;

    //EASINGIT TÄÄLLLÄ
    [SerializeField]
    EasingFunction.Ease EasingType = EasingFunction.Ease.Linear;
    private EasingFunction.Function EaseFunction;
    public Image FillImage;


    public void setCurrent(int curr) //Current
    {
        current = curr;
        setFIll();
    }
    public void setMaximum(int max) //Maximum
    {
        Max = max;
    }
    public void setMinimum(int min) //Minimum
    {
        Min = min;
    }


    // Start is called before the first frame update
    void Start()
    {
        EaseFunction = EasingFunction.GetEasingFunction(EasingType);
    }
    public void setFraction(float frac) // SET FRACTION
    {
        current = (float) frac*(Max-Min);
        // Debug.Log(current);
        setFIll();
    }
    private void setFIll()
    {
            Vector3 my_scale = FillImage.transform.localScale;
            my_scale.x = EaseFunction(0f, 1f, ((current - Min) / (Max - Min)));
            FillImage.transform.localScale = my_scale;
    }

   
}
