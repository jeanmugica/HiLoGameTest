public class Player
{
    public string Name { get; set; }
    public int Attempts { get; set; }
    public double TotalTime { get; set; }

    public Player(string name)
    {
        Name = name;
        Attempts = 0;
        TotalTime = 0;
    }
}
