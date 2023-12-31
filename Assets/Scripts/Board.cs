using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }    
    public TetraminoData held;
    public Hold hold;
    public Next next;
    public TetraminoData[] tetraminos;
    public Vector3Int spawnPosition;
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public TetraminoData[] nextPieces;

    public RectInt Bounds {
        get {
            Vector2Int position = new Vector2Int(-this.boardSize.x/2, -this.boardSize.y/2);
            return new RectInt(position, this.boardSize);
        }
    }

    private void Awake() {
        tilemap = GetComponentInChildren<Tilemap>();
        activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < this.tetraminos.Length; i++) {
            this.tetraminos[i].Initialize();
        }

        nextPieces = new TetraminoData[5];
        for (int i = 0; i < nextPieces.Length; i++) {
            int random = Random.Range(0, this.tetraminos.Length - 1);
            nextPieces[i] = this.tetraminos[random];
        }
    }

    private void Start() {
        SpawnPiece();
    }

    public void SpawnPiece() {
        int random = Random.Range(0, this.tetraminos.Length - 1);
        TetraminoData data = this.tetraminos[random];

        this.activePiece.Initialize(this, this.spawnPosition, nextPieces[next.currentStartIndex]);
        nextPieces[next.currentStartIndex++] = data;
        next.currentStartIndex %= nextPieces.Length;
        next.Set(nextPieces);

        if (IsValidPosition(this.activePiece, this.spawnPosition)) {
            Set(this.activePiece);
        } else {
            GameOver();
        }
    }

    private void SpawnPiece(TetraminoData data) {
        this.activePiece.Initialize(this, this.spawnPosition, data);

        if (IsValidPosition(this.activePiece, this.spawnPosition)) {
            Set(this.activePiece);
        } else {
            GameOver();
        }
    }

    private void GameOver() {
        this.tilemap.ClearAllTiles();
    }

    public void Hold() {
        hold.Set(activePiece.data);
        Clear(activePiece);
        bool wasUnset = (held.tetramino == Tetramino.None);
        TetraminoData heldData = held;
        held = activePiece.data;
        if (wasUnset) {
            SpawnPiece();
        } else {
            SpawnPiece(heldData);
        }
    }

    public void Set(Piece piece){
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, piece.data.tile);
        }
    }

    public void Clear(Piece piece){
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition, null);
        }
    }

    public bool IsValidPosition(Piece piece, Vector3Int position) {
        for (int i = 0; i < piece.cells.Length; i++) {
            Vector3Int tilePosition = piece.cells[i] + position;

            if (!Bounds.Contains((Vector2Int)tilePosition)) {
                return false;
            }

            if (this.tilemap.HasTile(tilePosition)) {
                return false;
            }
        }
        return true;
    }
    
    public void ClearLines() {
        int r = this.Bounds.yMin;
        while (r < this.Bounds.yMax) {
            if (IsLineFull(r)) {
                ClearLine(r);
            } else {
                r++;
            }
        }
    }

    private void ClearLine(int r) {
        for (int c = this.Bounds.xMin; c < this.Bounds.xMax; c++) {
            Vector3Int pos = new Vector3Int(c, r, 0);
            this.tilemap.SetTile(pos, null);
        }

        while (r < this.Bounds.yMax) { 
            for (int c = this.Bounds.xMin; c < this.Bounds.xMax; c++) {
                Vector3Int pos = new Vector3Int(c, r, 0);
                Vector3Int upPos = new Vector3Int(c, r + 1, 0);
                this.tilemap.SetTile(pos, this.tilemap.GetTile(upPos));
            }
            r++;
        }
    }

    private bool IsLineFull(int r) {
        for (int c = this.Bounds.xMin; c < this.Bounds.xMax; c++) {
            Vector3Int pos = new Vector3Int(c, r, 0);
            
            if (!this.tilemap.HasTile(pos)) {
                return false;
            }
        }
        return true;
    }
}

