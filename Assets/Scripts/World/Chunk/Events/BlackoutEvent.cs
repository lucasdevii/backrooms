using System;
using System.Collections; 
using UnityEngine; 

public static class BlackoutEvent 
{ 
    private static float chanceOfBlackout = 0.007f; 
    private static float timerForTrying = 10f; 
    private static int minTimeWithBlackout = 5;
    private static int maxTimeWithBlackout = 60;

    public static IEnumerator RollBlackoutChance() 
    { 
        
        while (true)
        {
            yield return new WaitForSeconds(timerForTrying);

            bool isBlackout = UnityEngine.Random.value < chanceOfBlackout;

            if (isBlackout)
            {
                int timerWithBlackout = UnityEngine.Random.Range(
                    minTimeWithBlackout,
                    maxTimeWithBlackout + 1
                );

                WorldManager.Instance.SetBlackout(true);

                yield return new WaitForSeconds(timerWithBlackout);
            }
            else
            {
                WorldManager.Instance.SetBlackout(false);
            }
        }
        
    } 
}