using System.Collections.Generic;

namespace Template
{
    public class Board
    {
        public ZombieManager zombieManager = null!;
    }

    public class ZombieManager
    {
        public List<Zombie> zombieList = null!;
    }

    public class Zombie
    {
        public bool isStant;
        public bool immune_wakeUp;
        public bool hide;
        public Board board = null!;

        private bool Path_Finding() => false;
        private void TranToWalk() { }

        private void ZC_ArmoredFlagWakeUpZombies()
        {
            if (isStant && !immune_wakeUp && !hide)
            {
                Path_Finding();
                TranToWalk();
            }

            foreach (Zombie zombie in board.zombieManager.zombieList)
            {
                if (!zombie.immune_wakeUp && !zombie.hide && !zombie.IsDisabled() && zombie.isStant)
                {
                    zombie.Path_Finding();
                    zombie.TranToWalk();
                }
            }
        }

        private bool IsDisabled() => false;
    }
}
