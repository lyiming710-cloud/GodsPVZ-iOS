using UnityEngine;

namespace UnityEngine
{
    public class Object
    {
        public static implicit operator bool(Object? value) => value is not null;
    }

    public class Component : Object { }
    public class Behaviour : Component { }
    public class MonoBehaviour : Behaviour { }

    public class Camera : Behaviour
    {
        public static Camera main => null!;
        public float orthographicSize => 0f;
    }

    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
    }

    public static class Random
    {
        public static int Range(int minInclusive, int maxExclusive) => minInclusive;
    }
}

namespace Template
{
    public class Zombie : MonoBehaviour
    {
        public int enemyPoint;
    }

    public class Project : MonoBehaviour
    {
        public void SunSet(int value) { }
        public void SetEndPosition<T>(T host) { }
    }

    public class Board : MonoBehaviour
    {
        public bool isFinished;
        public bool TestWinTargetZombie() => false;
        public void GameFinished() { }
    }

    public class ProjectManager : MonoBehaviour
    {
        public Board board = null!;
        public Project CreateProject(int movementTracks, int id, Vector3 position) => null!;

        public Project DropLootPiece(Zombie zombie, Vector3 position)
        {
            float maxX = Camera.main.orthographicSize * 920f / 540f;
            float minX = -maxX;
            if (position.x > maxX)
                position.x = maxX;
            if (minX > position.x)
                position.x = minX;

            if (board && !board.isFinished && board.TestWinTargetZombie())
            {
                board.GameFinished();
                Project award = CreateProject(2, 4, position);
                award.SetEndPosition(zombie);
                return null!;
            }

            int enemyPoint = zombie.enemyPoint;
            int roll = UnityEngine.Random.Range(0, 10000);
            int id;
            if (roll < enemyPoint)
            {
                id = 3;
            }
            else
            {
                roll -= enemyPoint;
                if (roll < enemyPoint * 30)
                {
                    id = 2;
                }
                else
                {
                    roll -= enemyPoint * 30;
                    if (roll < enemyPoint * 100)
                    {
                        Project sun25 = CreateProject(2, 0, position);
                        sun25.SunSet(25);
                        sun25.SetEndPosition(zombie);
                        Project sun50 = CreateProject(2, 0, position);
                        sun50.SunSet(50);
                        sun50.SetEndPosition(zombie);
                        Project sun100 = CreateProject(2, 0, position);
                        sun100.SunSet(100);
                        sun100.SetEndPosition(zombie);
                        return sun100;
                    }

                    roll -= enemyPoint * 100;
                    if (roll < enemyPoint * 150)
                    {
                        id = 1;
                    }
                    else
                    {
                        roll -= enemyPoint * 150;
                        if (roll >= enemyPoint * 800)
                            return null!;
                        id = 8;
                    }
                }
            }

            Project project = CreateProject(2, id, position);
            if (project)
                project.SetEndPosition(zombie);
            return project;
        }
    }
}
