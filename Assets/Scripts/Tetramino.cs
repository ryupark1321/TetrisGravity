using UnityEngine;
using UnityEngine.Tilemaps;

public enum Tetramino {
    I,
    O,
    T,
    J,
    L,
    S,
    Z,
    None
}

[System.Serializable]
public struct TetraminoData {
    public Tetramino tetramino;
    public Tile tile;
    public Vector2Int[] cells { get; private set; }
    public Vector2Int[,] wallKicks { get; private set; }

    public void Initialize() {
        this.cells = Data.Cells[this.tetramino];
        this.wallKicks = Data.WallKicks[this.tetramino];
    }

    public bool IsTheSameTetramino(TetraminoData other) {
        return other.tetramino == tetramino;
    }
}