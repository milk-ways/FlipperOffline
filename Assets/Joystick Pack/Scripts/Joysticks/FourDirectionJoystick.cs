using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FourDirectionJoystick : DirectionJoystick
{ 
    protected override float GetDirectionFloat(float value, AxisOptions snapAxis)
    {
        if (value == 0)
            return value;

        float angle = Vector2.Angle(input, Vector2.up);

        if (snapAxis == AxisOptions.Horizontal)
        {
            if (angle < 45f || angle > 135f)
                return 0;
            else
                return (value > 0) ? 1 : -1;
        }
        else if (snapAxis == AxisOptions.Vertical)
        {
            if (angle >= 45f && angle <= 135f)
                return 0;
            else
                return (value > 0) ? 1 : -1;
        }
        return value;
    }
}
