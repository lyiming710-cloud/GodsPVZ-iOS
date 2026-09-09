namespace Template
{
    public class Damage
    {
        public bool AreaDamage() => false;
    }

    public class Device
    {
        public void TakeDamage(Damage damage, Projectile projectile) { }
    }

    public class Zombie
    {
        public void TakeDamage(Damage damage, Projectile projectile) { }
    }

    public class Projectile
    {
        public int ID;
        public Damage damage = null!;

        private void Collision_AudioParticle() { }
        public void DestroyProjectile() { }

        private void Collision_Device(Device device)
        {
            if (ID == 10 || ID == 11 || ID == 15 || (ID >= 26 && ID <= 32))
                damage.AreaDamage();
            else
                device.TakeDamage(damage, this);

            Collision_AudioParticle();
            if (ID != 19 && ID != 23)
                DestroyProjectile();
        }

        private void Collision_Zombie(Zombie zombie)
        {
            if (ID == 10 || ID == 11 || ID == 15 || ID == 26 || ID == 27 || ID == 28 || ID == 31 || ID == 32)
                damage.AreaDamage();
            else
                zombie.TakeDamage(damage, this);

            Collision_AudioParticle();
            if (ID != 19 && ID != 23)
                DestroyProjectile();
        }
    }
}
