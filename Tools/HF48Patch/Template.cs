using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public class Object
    {
        public static implicit operator bool(Object? value) => !ReferenceEquals(value, null);
    }

    public class Component : Object
    {
        public GameObject gameObject => null!;
        public Transform transform => null!;
    }

    public class Behaviour : Component { }
    public class MonoBehaviour : Behaviour { }

    public class GameObject : Object
    {
        public Transform transform => null!;
        public void SetActive(bool value) { }
    }

    public class Transform : Component
    {
        public Vector3 position { get; set; }
    }

    public class Camera : Behaviour
    {
        public static Camera main => null!;
    }

    public class AudioClip : Object { }
    public class AudioSource : Behaviour { }

    public static class Time
    {
        public static float deltaTime => 0f;
        public static float timeScale { get; set; }
    }

    public static class Debug
    {
        public static void Log(object message) { }
    }

    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
        public Vector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public static Vector3 zero => default;
    }
}

namespace Template
{
    public enum Armor2Type
    {
        Ladder = 4,
    }

    public class Row
    {
        public int EnemyType;
    }

    public class Map
    {
        public List<Row> rows = null!;
        public int GetMapX() => 0;
    }

    public class BoardConfig
    {
        public Vector3 GetGridPosition(int GridX, int GridY) => default;
        public Vector3 GetZombiePosition(int GridX, int GridY) => default;
    }

    public class EnemyPath
    {
        public int gridX;
        public int gridY;
        public Vector3 position;
        public float waitingTime;
        public bool isEnd;

        public EnemyPath(int gridX, int gridY, float waitingTime, BoardConfig boardConfig)
        {
            this.waitingTime = waitingTime;
            this.gridX = gridX;
            this.gridY = gridY;
            isEnd = false;
            if (boardConfig != null)
                position = boardConfig.GetZombiePosition(gridX, gridY);
            else
                position = Vector3.zero;
        }
    }

    public class Enemy
    {
        public int id;
        public int row;
        public int gridX;
        public Vector3 position;
        public int wave;
        public float waitingTime;
    }

    public class Zombie : MonoBehaviour
    {
        public int ID;
        public Armor2Type armor2Type;
        public List<EnemyPath> prePath = null!;
        public List<EnemyPath> path = null!;
        public bool IsDisabled() => false;
    }

    public class Window_T : MonoBehaviour
    {
        public static Window_T PopupNewWindow(int t, Transform parent) => null!;
    }

    public static class ResourceManager
    {
        public static List<AudioClip> boardClips = null!;
    }

    public static class GlobalStaticVars
    {
        public static float AudioVolume() => 0f;
        public static AudioSource CreateAudioAtPoint(AudioClip clip, Vector3 position, float volume) => null!;
    }

    public class Board : MonoBehaviour
    {
        public BoardConfig boardConfig = null!;
        public Map map = null!;
        public bool isFailed;
        public bool gameStart;
        public bool gamePause;
        public GameObject WindowsUI = null!;
        public GameObject pause = null!;
        public ZombieManager zombieManager = null!;
        public EnemyManager enemyManager = null!;
    }

    public class EnemyManager : MonoBehaviour
    {
        public Board board = null!;

        public Zombie? DispatcheZombie(Enemy theEnemy) => null!;

        public void DispatcheLadderWave()
        {
            for (int rowIndex = 0; rowIndex < board.map.rows.Count; rowIndex++)
            {
                Row row = board.map.rows[rowIndex];
                if (row.EnemyType != 1)
                    continue;

                int mapX = board.map.GetMapX();
                Vector3 grid = board.boardConfig.GetGridPosition(mapX, rowIndex);
                Enemy enemy = new Enemy();
                enemy.row = rowIndex;
                enemy.position = new Vector3(grid.x + 57f, grid.y + 28f, grid.z);
                enemy.id = 16;
                enemy.waitingTime = 0.02f;
                DispatcheZombie(enemy);
            }
        }

        public void DispatcheSPHWave()
        {
            for (int rowIndex = 0; rowIndex < board.map.rows.Count; rowIndex++)
            {
                Row row = board.map.rows[rowIndex];
                if (row.EnemyType != 1)
                    continue;

                int mapX = board.map.GetMapX();
                int spawnGridX = mapX + 1 + rowIndex;
                Vector3 grid = board.boardConfig.GetGridPosition(spawnGridX, rowIndex);
                Enemy enemy = new Enemy();
                enemy.row = rowIndex;
                enemy.position = new Vector3(grid.x + 57f, grid.y + 28f, grid.z);
                enemy.id = 17;
                enemy.waitingTime = 0.02f;

                Zombie? zombie = DispatcheZombie(enemy);
                if (zombie)
                {
                    int homeGridX = board.map.GetMapX() - 1;
                    EnemyPath path = new EnemyPath(homeGridX, rowIndex, 999f, board.boardConfig);
                    zombie.prePath.Insert(0, path);
                }
            }
        }
    }

    public class ZombieManager : MonoBehaviour
    {
        public Board board = null!;
        public List<Zombie> zombieList = null!;
        public float ladderCountDown;
        public bool ladderTrigger;
        public int ladderTimes;

        private void Update()
        {
            Update_Ladder();
        }

        private void Update_Ladder()
        {
            if (!board.gameStart || board.gamePause)
                return;

            if (ladderTrigger && ladderCountDown > 0f)
            {
                ladderCountDown -= Time.deltaTime;
                if (ladderCountDown <= 0f)
                {
                    if (ladderTimes == 0)
                    {
                        Window_T.PopupNewWindow(5, board.WindowsUI.transform);
                        Board currentBoard = board;
                        if (!currentBoard.isFailed)
                        {
                            AudioClip clip = ResourceManager.boardClips[11];
                            Vector3 position = Camera.main.transform.position;
                            float volume = GlobalStaticVars.AudioVolume();
                            GlobalStaticVars.CreateAudioAtPoint(clip, position, volume);
                            currentBoard.pause.SetActive(true);
                            Debug.Log("GamePause");
                            currentBoard.gamePause = true;
                            Time.timeScale = 0f;
                        }
                    }

                    board.enemyManager.DispatcheLadderWave();
                    ladderCountDown = Math.Max(15f - ladderTimes, 5f);
                    ladderTimes++;
                    if (ladderTimes == 10)
                        board.enemyManager.DispatcheSPHWave();
                }
            }

            ladderTrigger = false;
        }

        public void TriggerLadder()
        {
            if (ladderTrigger)
                return;

            foreach (Zombie zombie in zombieList)
            {
                if (zombie.ID != 16 || zombie.armor2Type != (Armor2Type)4 || zombie.IsDisabled())
                    continue;
                if (zombie.path != null && zombie.path.Count > 0)
                    return;
            }

            ladderTrigger = true;
        }
    }
}
