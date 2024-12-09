using UnityEngine;

public static class Helpers
{
    public static float Map(float value, float min1, float max1, float min2, float max2, bool isClamp)
    {
        float newValue = min2 + (max2 - min2) * (value - min1) / (max1 - min1);

        if (isClamp)
        {
            newValue = Mathf.Clamp(newValue, min2, max2);
        }

        return newValue;
    }
}
