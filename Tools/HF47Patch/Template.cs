using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine
{
    public class Object
    {
        public static implicit operator bool(Object? value) => !ReferenceEquals(value, null);
        public static T Instantiate<T>(T original) where T : Object => null!;
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
        public Vector3 localScale { get; set; }
    }

    public static class Time
    {
        public static float deltaTime => 0f;
    }

    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
        public Vector3(float x, float y, float z) { this.x = x; this.y = y; this.z = z; }
        public static Vector3 zero => default;
    }
}

namespace Template
{
    public enum ParticleState
    {
        None,
        BucketDrop,
        CherryBoom,
        ConeDrop,
        FinalWave,
    }

    public class EnemyPath { }

    public class Enemy
    {
        public int id;
        public int row;
        public Vector3 position;
        public float waitingTime;
        public List<EnemyPath> ClonePath() => null!;
    }

    public class Wave
    {
        public List<Enemy> enemies = null!;
        public bool isFlagWave;
    }

    public class Flag
    {
        public List<Wave> waves = null!;
    }

    public class Row
    {
        public int EnemyType;
    }

    public class Map
    {
        public List<Row> rows = null!;
        public float cameraSize;
        public int GetMapX() => 0;
    }

    public class BoardConfig
    {
        public Vector3 GetGridPosition(int GridX, int GridY) => default;
    }

    public class Save
    {
        public int adventureLevel;
        public bool[] almanac_ZombieLock = null!;
    }

    public class SavesManager
    {
        public Save playerSave = null!;
    }

    public static class GlobalStaticVars
    {
        public class LawnApp
        {
            public SavesManager savesManager = null!;
        }
        public static LawnApp gLawnApp = null!;
    }

    public class Zombie : MonoBehaviour
    {
        public int gridY;
        public float waitingTime;
        public List<EnemyPath> prePath = null!;
        public int wave;
        public float healthPoint;
        public float armor1Point;
        public float armor2Point;
        public void TeleportTo(Vector3 position, float fZ, bool changeUI) { }
    }

    public class ZombieManager : MonoBehaviour
    {
        public Zombie CreateNewZombie(int zombieID) => null!;
    }

    public class FlagMeter : MonoBehaviour
    {
        public void UpdateMeter(int theFlag, int theWave) { }
    }

    public class Board : MonoBehaviour
    {
        public BoardConfig boardConfig = null!;
        public Map map = null!;
        public bool gameStart;
        public ZombieManager zombieManager = null!;
        public FlagMeter flagMeter = null!;
    }

    public static class ResourceManager
    {
        public static GameObject prefab_HugeWave = null!;
    }

    public class ParticlesManager : MonoBehaviour
    {
        public static ParticlesManager instance => null!;
        public GameObject CreatNewParticle(ParticleState particleState) => null!;
    }

    public class EnemyManager : MonoBehaviour
    {
        public List<Flag> flags = null!;
        public int wavesNum;
        public int theFlag;
        public int theWave;
        private float waveHealth;
        private float nextWaveTime;
        private float nextTestTime;
        private float nextFlagWaveTime;
        public bool finish;
        private float testWavelength;
        private float shortWavelength;
        private float longWavelength;
        public Board board = null!;

        private bool TextWaveHealth() => false;
        private void PlayBoardAudio(int ID) { }

        public void DispatcheWave(Wave theWave)
        {
            waveHealth = 0f;

            if (theWave.enemies != null)
            {
                foreach (Enemy enemy in theWave.enemies)
                    DispatcheZombie(enemy);
            }

            if (!theWave.isFlagWave)
                return;

            for (int rowIndex = 0; rowIndex < board.map.rows.Count; rowIndex++)
            {
                Row row = board.map.rows[rowIndex];
                if (row.EnemyType != 1)
                    continue;

                int mapX = board.map.GetMapX();
                Vector3 grid = board.boardConfig.GetGridPosition(mapX, rowIndex);
                Enemy enemy = new Enemy();
                enemy.row = rowIndex;
                enemy.id = 1;
                enemy.waitingTime = 0.02f;
                enemy.position = new Vector3(grid.x + 57f, grid.y + 28f, grid.z);

                int adventureLevel = GlobalStaticVars.gLawnApp.savesManager.playerSave.adventureLevel;
                if (adventureLevel > 10 && (adventureLevel > 15 || (rowIndex & 1) == 0))
                    enemy.id = 10;

                DispatcheZombie(enemy);
            }
        }

        public Zombie? DispatcheZombie(Enemy theEnemy)
        {
            Zombie zombie = board.zombieManager.CreateNewZombie(theEnemy.id);
            if (!zombie)
                return null;

            zombie.TeleportTo(theEnemy.position, 0f, false);
            zombie.gridY = theEnemy.row;
            zombie.waitingTime = theEnemy.waitingTime;
            zombie.prePath = theEnemy.ClonePath();
            zombie.wave = theWave;
            waveHealth += zombie.healthPoint;
            waveHealth += zombie.armor1Point;
            waveHealth += zombie.armor2Point * 0.2f;

            bool[] zombieLocks = GlobalStaticVars.gLawnApp.savesManager.playerSave.almanac_ZombieLock;
            zombieLocks[theEnemy.id] = true;
            return zombie;
        }

        private void TimeUpdate()
        {
            if (!board.gameStart)
                return;

            if (nextTestTime > 0f && !finish)
            {
                float previousTestTime = nextTestTime;
                nextTestTime -= Time.deltaTime;
                if (!(nextTestTime > 0f))
                {
                    float threshold = shortWavelength - testWavelength;
                    nextTestTime = 0f;
                    if (threshold < nextWaveTime)
                    {
                        if (!TextWaveHealth())
                        {
                            nextTestTime = previousTestTime;
                        }
                        else
                        {
                            nextTestTime = testWavelength;
                            if (threshold < nextWaveTime)
                                nextWaveTime = threshold;
                        }
                    }
                }
            }

            if (!(nextFlagWaveTime > 0f))
            {
                if (!(nextWaveTime > 0f) || finish)
                    return;

                nextWaveTime -= Time.deltaTime;
                if (nextWaveTime > 0f)
                    return;

                nextWaveTime = 0f;
                if (theWave == 0)
                {
                    board.flagMeter.gameObject.SetActive(true);
                    PlayBoardAudio(0);
                }

                if (theWave % 10 == 9)
                {
                    nextFlagWaveTime = 7.5f;
                    GameObject hugeWave = Object.Instantiate<GameObject>(ResourceManager.prefab_HugeWave);
                    Transform hugeTransform = hugeWave.transform;
                    Vector3 hugeScale = hugeTransform.localScale;
                    float hugeFactor = board.map.cameraSize / 540f;
                    hugeScale.x *= hugeFactor;
                    hugeScale.y *= hugeFactor;
                    hugeScale.z *= hugeFactor;
                    hugeTransform.localScale = hugeScale;
                    PlayBoardAudio(3);
                    if (theWave == wavesNum - 1)
                        nextTestTime = 0f;
                    return;
                }

                Flag flag = flags[theFlag];
                Wave wave = flag.waves[theWave % 10];
                DispatcheWave(wave);
                board.flagMeter.UpdateMeter(theFlag, theWave);

                if (theWave == wavesNum - 1)
                {
                    nextTestTime = 0f;
                    finish = true;
                    return;
                }

                theWave++;
                nextWaveTime = longWavelength;
                nextTestTime = testWavelength;
                if (theWave % 10 == 9)
                    nextWaveTime = longWavelength * 2f;
                return;
            }

            nextFlagWaveTime -= Time.deltaTime;
            if (nextFlagWaveTime > 0f)
                return;

            board.flagMeter.UpdateMeter(theFlag, theWave);
            Flag flagWave = flags[theFlag];
            Wave dispatched = flagWave.waves[theWave % 10];
            DispatcheWave(dispatched);

            if (theWave == wavesNum - 1)
            {
                GameObject finalWave = ParticlesManager.instance.CreatNewParticle(ParticleState.FinalWave);
                Transform finalTransform = finalWave.transform;
                Vector3 finalScale = finalTransform.localScale;
                float finalFactor = board.map.cameraSize / 540f;
                finalScale.x *= finalFactor;
                finalScale.y *= finalFactor;
                finalScale.z *= finalFactor;
                finalTransform.localScale = finalScale;
                finalWave.transform.position = Vector3.zero;
                PlayBoardAudio(2);
                PlayBoardAudio(8);
                nextTestTime = 0f;
                finish = true;
                return;
            }

            PlayBoardAudio(8);
            theWave++;
            theFlag++;
            nextWaveTime = longWavelength;
            nextTestTime = testWavelength;
            if (theWave % 10 == 9)
                nextWaveTime = longWavelength * 2f;
        }
    }
}
