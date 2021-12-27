public interface Weapon
{
    public string Name { get; }

    public bool IsActive { get; }

    public void Activate();

    public void Deactivate();

    public void Attack();
}
