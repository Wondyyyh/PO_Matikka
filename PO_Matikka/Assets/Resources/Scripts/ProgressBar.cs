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

    public int Max = 100;
    public int Min = 0;

    [SerializeField]
    private int current = 10;

    //EASINGIT TÄÄLLLÄ
    [SerializeField]
    EasingFunction.Ease EasingType = EasingFunction.Ease.Linear;

    public Image FillImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void setCurrent(int curr)
    {
        current = curr;
        setFIll();
    }
    public void setMaximum(int max)
    {
        max = Max;
    }
    public void setMinimum(int min)
    {
        min = Min;
    }

    private void setFIll()
    {
        if (FillImage != null)
        {
            //FillImage.transform.localscale.x = current;
            Vector3 my_scale = FillImage.transform.localScale;
            my_scale.x = (current - Min) / (float)(Max - Min);
            FillImage.transform.localScale = my_scale;

        }
    }

    // Update is called once per frame
    void Update()
    {
       setFIll();
    }
}
