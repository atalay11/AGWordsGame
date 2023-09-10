using UnityEngine;

public class MobileInput : InputPlatform
{
    public override bool GetPointer() => Input.touchCount > 0;

    public override bool GetPointerDown()
    {
        if (GetPointer())
            return Input.GetTouch(0).phase == TouchPhase.Began;
        else
            return false;
    }

    public override bool GetPointerUp()
    {
        if (GetPointer())
            return Input.GetTouch(0).phase == TouchPhase.Ended;
        else
            return false;
    }

    public override Vector2 GetPointerPosition() => Input.mousePosition;
}
