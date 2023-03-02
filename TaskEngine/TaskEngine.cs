using System;
using System.Threading;
using System.Threading.Tasks;

class TaskEngine
{
    private CancellationTokenSource cts;
    private Task[] tasks;

    public TaskEngine()
    {
        cts = new CancellationTokenSource();
        StartAsync().Wait();
    }

    public async Task StartAsync()
    {
        // Configure your parallel tasks here
        tasks = new Task[]
        {
            Task.Factory.StartNew(() => DoTask1()),
            Task.Factory.StartNew(() => DoTask2())
        };

        // Start polling the tasks and return the polling task
        await PollTasksAsync(TimeSpan.FromSeconds(5), cts.Token);
    }

    public void Stop()
    {
        // Cancel the polling and wait for the tasks to complete
        cts.Cancel();
        Task.WaitAll(tasks);
    }

    private async Task PollTasksAsync(TimeSpan interval, CancellationToken token)
    {
        do
        {
            foreach (var task in tasks)
            {
                if (task.IsCompleted)
                {
                    // Handle task completion here
                }
                else if (task.IsFaulted)
                {
                    // Handle task error here
                }
                else
                {
                    // Handle task still running here
                }
                task.Wait();
            }
            DoTask1();
            DoTask2();
            await Task.Delay(interval, token);
        }
        while (!token.IsCancellationRequested);
    }

    private void DoTask1()
    {
        Console.WriteLine("DoTask1");
    }

    private void DoTask2()
    {
        // Implement your task logic here
        Console.WriteLine("DoTask2");
    }
}