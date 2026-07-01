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
        float valuesMultFactor = 0;

        for(int i = 0; i < values.Length; i++){
            valuesMultFactor += (values[i] * weights[i % weights.Length]);
        }
        
        //Retorna um valor entre 0 e 1;
        return Mathf.Sin((seed * SeedWeight + valuesMultFactor) + 1f * 0.5f);
    }
}
