public interface IInputOutputService
{
    void Write(string message);
    void WriteLine(string message);
    string ReadLine();
    int ReadValidNumber(string prompt, int minValue = int.MinValue);
    bool PromptUser(string message);
}
