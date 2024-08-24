public class VFXPlayed : RewindableAction
{
    private VFXPlayback vFXPlayback;

    public static void VFXPlay(VFXPlayback vFXPlayback)
    {
        new VFXPlayed(vFXPlayback);
    }

    public VFXPlayed(VFXPlayback vFXPlayback)
    {
        this.vFXPlayback = vFXPlayback;
        Execute();
    }

    public override void Undo()
    {
        vFXPlayback.StopEffect();
    }
}
