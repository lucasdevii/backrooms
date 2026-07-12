using System;
using UnityEngine;

public static class Noise
{
    private static readonly float[] weights =
    {
        912312.42f,
        531231.87f,
        123987.54f,
        874512.91f,
        315789.23f,
        623876.35f,
        875345.76f,
        325759.19f
    };
    private const float SeedWeight = 9265917.32f;

    public static float DefaultNoise(ulong seed, params float[] values)
    {
        ulong hash = seed;

        foreach (float value in values)
        {
            hash ^= (ulong)(value * 1000f);
            hash *= 0x9E3779B97F4A7C15UL;
        }

        return (hash & 0xFFFFFFFF) / (float)uint.MaxValue;
    }
}
