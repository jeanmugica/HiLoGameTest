using System;

public class ConsoleInputOutputService : IInputOutputService
{
    public void Write(string message)
    {
        Console.Write(message);
    }

    public void WriteLine(string message)
    {
        Console.WriteLine(message);
    }

    public string ReadLine()
    {
        return Console.ReadLine();
    }

    public int ReadValidNumber(string prompt, int minValue = int.MinValue)
    {
        int number;
        do
        {
            Write(prompt);
        } while (!int.TryParse(ReadLine(), out number) || number < minValue);

        return number;
    }

    public bool PromptUser(string message)
    {
        Write(message);
        return ReadLine().Trim().ToLower() == "yes";
    }
}
