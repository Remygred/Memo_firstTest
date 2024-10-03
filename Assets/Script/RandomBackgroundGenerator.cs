using UnityEngine;
using UnityEngine.Tilemaps;

public class OptimizedBackgroundGenerator : MonoBehaviour
{
    public Tilemap tilemap;              // 场景中的 Tilemap 组件
    public Tile[] backgroundTiles;       // 背景图片对应的 Tiles
    public int width = 20;               // 地图宽度
    public int height = 20;              // 地图高度
    public Transform player;             // 玩家对象
    public float distanceThreshold = 15f;    // Tile 移动的距离阈值
    public float preemptiveDistance = 5f;    // 提前检测的距离
    public int bufferSize = 10;              // 预生成范围，超出玩家视野的区域

    private Vector3Int[,] tilePositions;  // 记录每个 Tile 的位置

    void Start()
    {
        // 初始化地图，生成大于当前视野范围的背景
        GenerateInitialBackground();
    }

    void Update()
    {
        // 动态检测玩家的位置并移动远离的 Tile
        MoveTilesBasedOnPlayerPosition();
    }

    // 初始化生成大于视野范围的背景
    void GenerateInitialBackground()
    {
        tilePositions = new Vector3Int[width + bufferSize, height + bufferSize];

        int centerX = (width + bufferSize) / 2;
        int centerY = (height + bufferSize) / 2;

        for (int x = -centerX; x < centerX; x++)
        {
            for (int y = -centerY; y < centerY; y++)
            {
                // 随机选择一个背景 Tile
                int randomIndex = Random.Range(0, backgroundTiles.Length);
                Tile selectedTile = backgroundTiles[randomIndex];

                // 在 Tilemap 上设置 Tile
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                tilePositions[x + centerX, y + centerY] = tilePosition;
                tilemap.SetTile(tilePosition, selectedTile);
            }
        }
    }

    // 动态移动远离玩家的 Tile 到附近的未占用区域
    void MoveTilesBasedOnPlayerPosition()
    {
        Vector3 playerPos = player.position;

        for (int x = 0; x < width + bufferSize; x++)
        {
            for (int y = 0; y < height + bufferSize; y++)
            {
                Vector3Int tilePos = tilePositions[x, y];

                // 计算玩家到当前 Tile 的距离
                float distanceToPlayer = Vector3.Distance(playerPos, tilemap.CellToWorld(tilePos));

                // 如果玩家离该 Tile 距离远，并且 Tile 处于提前检测范围内，提前触发 Tile 移动
                if (distanceToPlayer > (distanceThreshold - preemptiveDistance))
                {
                    // 找到一个新的空闲位置，立即移动 Tile
                    Vector3Int newPosition = FindEmptyTileAroundPlayer();
                    if (newPosition != Vector3Int.zero)
                    {
                        MoveTile(tilePos, newPosition);  // 同步移动
                        tilePositions[x, y] = newPosition;
                    }
                }
            }
        }
    }

    // 同步移动 Tile 到新的位置
    void MoveTile(Vector3Int oldPosition, Vector3Int newPosition)
    {
        Tile tileToMove = (Tile)tilemap.GetTile(oldPosition);

        // 设置 Tile 到新位置
        tilemap.SetTile(newPosition, tileToMove);

        // 清除旧位置的 Tile
        tilemap.SetTile(oldPosition, null);
    }

    // 找到玩家附近的一个空闲 Tile 位置
    Vector3Int FindEmptyTileAroundPlayer()
    {
        Vector3Int playerGridPos = tilemap.WorldToCell(player.position);

        // 定义一个范围在玩家附近查找空闲位置
        int range = 5;
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector3Int checkPos = new Vector3Int(playerGridPos.x + x, playerGridPos.y + y, 0);
                if (!tilemap.HasTile(checkPos))
                {
                    return checkPos;  // 返回找到的空位置
                }
            }
        }

        return Vector3Int.zero;  // 如果没有找到空位置，则返回 Vector3Int.zero
    }
}
