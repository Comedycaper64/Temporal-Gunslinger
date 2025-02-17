using System;

public interface IFireStarter
{
    event EventHandler OnFireStarted;

    public bool GetIsAflame();
    public void SetIsAflame(bool aflame);
}
