using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridLayout3D : MonoBehaviour
{
    public static void SetLayout(List<Transform> elements, float constaintHeight, float constraintWidth, int maxColumnSize, float space)
    {
        // always adds padding to align in the mid center
        // layout is positioned at XY plane

        double elementHeight = 1;
        double elementWidth = 1;

        int numberOfElementsAtRow = (int)Mathf.Ceil((float)elements.Count / maxColumnSize);

        double rawWholeSize = numberOfElementsAtRow * elementHeight + (numberOfElementsAtRow - 1) * space;
        double columnWholeSize = maxColumnSize * elementWidth + (maxColumnSize - 1) * space;

        double scaleOfElementsIfRow = constaintHeight / rawWholeSize;
        double scaleOfElementsIfColumn = constraintWidth / columnWholeSize;

        // scale less to void floating point errors
        const double scaleErrorMargin = 0.05f;
        var scaleOfElements = Math.Min(scaleOfElementsIfColumn, scaleOfElementsIfRow);
        scaleOfElements -= scaleErrorMargin;

        // recalculate after scale

        elementHeight *= scaleOfElements;
        elementWidth *= scaleOfElements;

        rawWholeSize = numberOfElementsAtRow * elementHeight + (numberOfElementsAtRow - 1) * space;
        columnWholeSize = maxColumnSize * elementWidth + (maxColumnSize - 1) * space;

        // calculate padding
        double topPadding = (constaintHeight - rawWholeSize) * 0.5f;
        double leftPadding = (constraintWidth - columnWholeSize) * 0.5f;

        double elementSpacing = scaleOfElements + space;

        double startXpos = -(((constraintWidth) - elementWidth) * 0.5f) + leftPadding;
        double lastXpos = startXpos;
        double lastYpos = (((constaintHeight) - elementHeight) * 0.5f) - topPadding;

        int curColumnSize = 0;
        foreach (var child in elements)
        {
            child.localScale *= (float)scaleOfElements;
            child.position = new Vector3((float)lastXpos, (float)lastYpos, 0f);
            lastXpos += elementSpacing;

            curColumnSize += 1;
            if (curColumnSize == maxColumnSize)
            {
                lastYpos -= elementSpacing;
                lastXpos = startXpos;
                curColumnSize = 0;
            }
        }
    }
}
