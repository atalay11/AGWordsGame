using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerVisual : MonoBehaviour
{
    [SerializeField] private SlicedFilledImage m_slicedFilledImage;

    public void SetFillAmount(float percent)
    {
        m_slicedFilledImage.fillAmount = percent;
    }
}
