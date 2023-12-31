using UnityEngine;
using UnityEngine.Tilemaps;

public class Next : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Vector3Int spawnPosition;
    public int currentStartIndex = 0;
    public Vector2Int boardSize = new Vector2Int(6, 16);

    private void Awake() {
        tilemap = GetComponentInChildren<Tilemap>();
    }

    public void Set(TetraminoData[] datas) {
        Clear();
        Vector3Int position = spawnPosition;
        for (int i = 0; i < datas.Length; i++) {
            TetraminoData data = datas[(currentStartIndex + i)%datas.Length];
            for (int j = 0; j < data.cells.Length; j++) {
                Vector3Int tilePosition = (Vector3Int) data.cells[j] + position;
                tilemap.SetTile(tilePosition, data.tile);
            }
            position.y -= 3;
        }
    }

    private void Clear() {
        tilemap.ClearAllTiles();
    }
}
