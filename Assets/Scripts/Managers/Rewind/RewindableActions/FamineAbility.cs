public class FamineAbility : RewindableAction
{
    private PlayerFamineAbility playerFamineAbility;

    public static void FamineAbilityUsed(PlayerFamineAbility playerFamineAbility)
    {
        new FamineAbility(playerFamineAbility);
    }

    public FamineAbility(PlayerFamineAbility playerFamineAbility)
    {
        this.playerFamineAbility = playerFamineAbility;
        Execute();
    }

    public override void Undo()
    {
        playerFamineAbility.UndoAbility();
    }
}
