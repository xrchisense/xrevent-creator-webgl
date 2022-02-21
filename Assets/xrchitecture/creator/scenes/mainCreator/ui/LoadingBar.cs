using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LoadingBar : MonoBehaviour
{

    public int current;
    public int maximum;
    public Image bar;

    void Start()
    {

    }

    void Update()
    {
        GetCurrentFill();
    }

    void GetCurrentFill()
    {
        float fillAmount = (float)current / (float)maximum;
        bar.fillAmount = fillAmount;
    }
}
