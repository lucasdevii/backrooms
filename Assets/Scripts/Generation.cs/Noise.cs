using System;
using UnityEngine;

public static class Noise
{
    public static float DefaultNoise(ulong seed, int x, int y)
    {
        return (float)Math.Sin(
            seed * 9823123.42 +
            x * 21384152.95 +
            y * 7519727.333
        );
    }
}
