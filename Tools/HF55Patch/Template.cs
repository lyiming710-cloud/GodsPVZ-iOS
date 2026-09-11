using UnityEngine;

namespace UnityEngine
{
    public class Object { }
    public class Component : Object { }
    public class Behaviour : Component { }
    public class MonoBehaviour : Behaviour { }
    public class Transform : Component
    {
        public Vector3 position => default;
    }
    public class GameObject : Object
    {
        public Transform transform => null!;
    }
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
    }
}

namespace Template
{
    public class Project : MonoBehaviour { }

    public class ProjectManager : MonoBehaviour
    {
        public Project DropLootPiece(Zombie zombie, Vector3 position) => null!;
    }

    public class Board : MonoBehaviour
    {
        public ProjectManager projectManager = null!;
    }

    public class Zombie : MonoBehaviour
    {
        public GameObject shadow = null!;
        public bool isOnBoard;
        public Board board = null!;

        public Vector3 GetHaedPosition() => default;

        public void DropLootPiece()
        {
            Transform transform = shadow.transform;
            Vector3 position = transform.position;
            if (isOnBoard)
            {
                Vector3 haedPosition = GetHaedPosition();
                Board board = this.board;
                Project project = board.projectManager.DropLootPiece(this, haedPosition);
            }
        }
    }
}
