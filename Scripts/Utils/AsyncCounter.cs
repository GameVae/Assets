using System.Threading;

public sealed class AsyncCounter
{
    private Thread timer;
    private double refTime;

    public double RefTime
    {
        get { return refTime; }
    }

    public AsyncCounter() { }

    ~AsyncCounter()
    {
        timer?.Abort();
    }

    /// <summary>
    /// Start countdown
    /// </summary>
    /// <param name="totalTime">Start time in Seconds</param>
    public void Start(double totalTime)
    {
        refTime = totalTime;
        timer = new Thread(Counter);
        timer.Start();
    }

    private void Counter()
    {
        while (Thread.CurrentThread.IsAlive)
        {
            Thread.Sleep(200);
            refTime -= 0.2d;
        }
    }
}
