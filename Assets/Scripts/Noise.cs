using UnityEngine;

public static class Noise {
    public static float Get2DPerlinNoise(Vector2 pos, float offset = 0.0f, float scale = 1/32.0f) {
        offset += 0.1f; // Weil Mathf.PerlinNoise f�r Integer aus irgendeinem Grund fehlerhafte Werte zur�ck gibt
        return Mathf.PerlinNoise(pos.x * scale + offset, pos.y * scale + offset);
    }
}
