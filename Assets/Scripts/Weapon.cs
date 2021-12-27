public interface Weapon
{
    public string Name { get; } //ENCAPSULATION

    public bool IsActive { get; }

    public void Activate();

    public void Deactivate();

    public void Attack();
}
