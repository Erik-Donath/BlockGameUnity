using UnityEngine;

public static class VoxelData {
    // X+ => East
    // X- => West
    // Z+ => North
    // Z- => South
    // Y+ => Up
    // Y- => Down

    public static readonly Vector3[] Vertices = new Vector3[8] {
        new Vector3(0, 0, 0), // 0
        new Vector3(1, 0, 0), // 1
        new Vector3(0, 1, 0), // 2
        new Vector3(1, 1, 0), // 3
        new Vector3(0, 0, 1), // 4
        new Vector3(1, 0, 1), // 5
        new Vector3(0, 1, 1), // 6
        new Vector3(1, 1, 1), // 7
    };

    public static readonly int[,] Indeces = new int[6, 4] {
        { 5, 7, 4, 6 }, // North
        { 0, 2, 1, 3 }, // South

        { 4, 6, 0, 2 }, // West
        { 1, 3, 5, 7 }, // East

        { 2, 6, 3, 7 }, // Up
        { 1, 5, 0, 4 }, // Down
    };

    public static readonly int[] ArrayOrder = new int[6] {
        0, 1, 2, 2, 1, 3
    };
}
