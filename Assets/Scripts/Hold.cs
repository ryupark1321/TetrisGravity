using UnityEngine;
using UnityEngine.Tilemaps;

public class Hold : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Vector3Int spawnPosition;

    private void Awake() {
        tilemap = GetComponentInChildren<Tilemap>();
    }

    public void Set(TetraminoData data) {
        Clear();
        for (int i = 0; i < data.cells.Length; i++) {
            Vector3Int tilePosition = (Vector3Int) data.cells[i] + spawnPosition;
            tilemap.SetTile(tilePosition, data.tile);
        }
    }

    public void Clear() {
        tilemap.ClearAllTiles();
    }
}
