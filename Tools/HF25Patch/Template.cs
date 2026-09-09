namespace Template
{
    public enum Camp
    {
        nullCamp = 0,
        plant = 1,
        zombie = 2,
    }

    public class Damage
    {
        public bool AreaDamage() => false;
    }

    public class Projectile
    {
        public int ID;
        public Camp camp;
        public Damage damage = null!;
        public float fZ;
        public float fZ_shadow;
        public int hitType;

        private bool CollisionDetect_Device(bool sameCamp) => false;
        private bool CollisionDetect_Plant(bool sameCamp) => false;
        private bool CollisionDetect_Zombie(bool sameCamp) => false;
        private void Collision_AudioParticle() { }
        public void DestroyProjectile() { }

        private void CollisionDetect()
        {
            switch (hitType)
            {
                case 0:
                    CollisionDetect_Ground();
                    return;

                case 1:
                    if (camp == Camp.plant)
                    {
                        if (CollisionDetect_Zombie(false) || CollisionDetect_Device(false))
                            return;
                    }
                    else if (camp == Camp.zombie)
                    {
                        if (CollisionDetect_Zombie(false) || CollisionDetect_Plant(false) || CollisionDetect_Device(false))
                            return;
                    }
                    CollisionDetect_Ground();
                    return;

                case 2:
                    if (camp == Camp.plant)
                    {
                        if (CollisionDetect_Zombie(true) || CollisionDetect_Plant(true) || CollisionDetect_Device(true))
                            return;
                    }
                    else if (camp == Camp.zombie)
                    {
                        if (CollisionDetect_Zombie(true) || CollisionDetect_Device(true))
                            return;
                    }
                    else
                    {
                        return;
                    }
                    CollisionDetect_Ground();
                    return;

                case 3:
                    if (CollisionDetect_Zombie(false) || CollisionDetect_Plant(false) || CollisionDetect_Device(false))
                        return;
                    if (CollisionDetect_Zombie(true) || CollisionDetect_Plant(true) || CollisionDetect_Device(true))
                        return;
                    CollisionDetect_Ground();
                    return;

                default:
                    return;
            }
        }

        private void CollisionDetect_Ground()
        {
            float threshold = (ID == 29 || ID == 30) ? 240f : 0f;
            float diff = fZ - fZ_shadow;
            if (!(threshold >= diff))
                return;

            if ((ID >= 9 && ID <= 11) || ID == 15 || (ID >= 26 && ID <= 32))
                damage.AreaDamage();

            Collision_AudioParticle();
            DestroyProjectile();
        }
    }
}
