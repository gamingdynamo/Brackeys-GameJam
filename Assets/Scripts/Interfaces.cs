
public interface IDamageable
{
    void damage(int damage, bool friendly);
}
public interface IUpgradeable
{
    void increasespeed();
    void increasedmg();
    void increasehp();
    void increaserange();
}
public interface IInteractable
{
    void returnupgradables();
}
