using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    public int goldCount = 0;

    private void Update()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            TryMineNearbyOres();
        }
    }

    void TryMineNearbyOres()
    {
        GoldOre[] allOres = FindObjectsByType<GoldOre>(FindObjectsSortMode.None);

        foreach (GoldOre ore in allOres)
        {
            if (ore.IsPlayerInRange())
            {
                bool mined = ore.MineOre();
                if (mined)
                {
                    goldCount += ore.goldValue;
                    Debug.Log("You mined " + ore.goldValue + " gold! Total: " + goldCount);
                }

                break; // Only mine one ore per press
            }
        }
    }

}
