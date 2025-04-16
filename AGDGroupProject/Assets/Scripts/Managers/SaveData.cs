using UnityEngine;
using System.Collections.Generic;

public static class SaveData
{
    // Elke scene heeft zijn eigen opgeslagen positie
    public static Dictionary<string, Vector2> scenePositions = new Dictionary<string, Vector2>();

    // Laat teleportatie pas toe nadat een scene geladen is en delay voorbij is
    public static bool canTeleport = false;
}
