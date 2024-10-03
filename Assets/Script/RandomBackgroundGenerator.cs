using UnityEngine;
using UnityEngine.Tilemaps;

public class OptimizedBackgroundGenerator : MonoBehaviour
{
    public Tilemap tilemap;              // �����е� Tilemap ���
    public Tile[] backgroundTiles;       // ����ͼƬ��Ӧ�� Tiles
    public int width = 20;               // ��ͼ���
    public int height = 20;              // ��ͼ�߶�
    public Transform player;             // ��Ҷ���
    public float distanceThreshold = 15f;    // Tile �ƶ��ľ�����ֵ
    public float preemptiveDistance = 5f;    // ��ǰ���ľ���
    public int bufferSize = 10;              // Ԥ���ɷ�Χ�����������Ұ������

    private Vector3Int[,] tilePositions;  // ��¼ÿ�� Tile ��λ��

    void Start()
    {
        // ��ʼ����ͼ�����ɴ��ڵ�ǰ��Ұ��Χ�ı���
        GenerateInitialBackground();
    }

    void Update()
    {
        // ��̬�����ҵ�λ�ò��ƶ�Զ��� Tile
        MoveTilesBasedOnPlayerPosition();
    }

    // ��ʼ�����ɴ�����Ұ��Χ�ı���
    void GenerateInitialBackground()
    {
        tilePositions = new Vector3Int[width + bufferSize, height + bufferSize];

        int centerX = (width + bufferSize) / 2;
        int centerY = (height + bufferSize) / 2;

        for (int x = -centerX; x < centerX; x++)
        {
            for (int y = -centerY; y < centerY; y++)
            {
                // ���ѡ��һ������ Tile
                int randomIndex = Random.Range(0, backgroundTiles.Length);
                Tile selectedTile = backgroundTiles[randomIndex];

                // �� Tilemap ������ Tile
                Vector3Int tilePosition = new Vector3Int(x, y, 0);
                tilePositions[x + centerX, y + centerY] = tilePosition;
                tilemap.SetTile(tilePosition, selectedTile);
            }
        }
    }

    // ��̬�ƶ�Զ����ҵ� Tile ��������δռ������
    void MoveTilesBasedOnPlayerPosition()
    {
        Vector3 playerPos = player.position;

        for (int x = 0; x < width + bufferSize; x++)
        {
            for (int y = 0; y < height + bufferSize; y++)
            {
                Vector3Int tilePos = tilePositions[x, y];

                // ������ҵ���ǰ Tile �ľ���
                float distanceToPlayer = Vector3.Distance(playerPos, tilemap.CellToWorld(tilePos));

                // ��������� Tile ����Զ������ Tile ������ǰ��ⷶΧ�ڣ���ǰ���� Tile �ƶ�
                if (distanceToPlayer > (distanceThreshold - preemptiveDistance))
                {
                    // �ҵ�һ���µĿ���λ�ã������ƶ� Tile
                    Vector3Int newPosition = FindEmptyTileAroundPlayer();
                    if (newPosition != Vector3Int.zero)
                    {
                        MoveTile(tilePos, newPosition);  // ͬ���ƶ�
                        tilePositions[x, y] = newPosition;
                    }
                }
            }
        }
    }

    // ͬ���ƶ� Tile ���µ�λ��
    void MoveTile(Vector3Int oldPosition, Vector3Int newPosition)
    {
        Tile tileToMove = (Tile)tilemap.GetTile(oldPosition);

        // ���� Tile ����λ��
        tilemap.SetTile(newPosition, tileToMove);

        // �����λ�õ� Tile
        tilemap.SetTile(oldPosition, null);
    }

    // �ҵ���Ҹ�����һ������ Tile λ��
    Vector3Int FindEmptyTileAroundPlayer()
    {
        Vector3Int playerGridPos = tilemap.WorldToCell(player.position);

        // ����һ����Χ����Ҹ������ҿ���λ��
        int range = 5;
        for (int x = -range; x <= range; x++)
        {
            for (int y = -range; y <= range; y++)
            {
                Vector3Int checkPos = new Vector3Int(playerGridPos.x + x, playerGridPos.y + y, 0);
                if (!tilemap.HasTile(checkPos))
                {
                    return checkPos;  // �����ҵ��Ŀ�λ��
                }
            }
        }

        return Vector3Int.zero;  // ���û���ҵ���λ�ã��򷵻� Vector3Int.zero
    }
}
