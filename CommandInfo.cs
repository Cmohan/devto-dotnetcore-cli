public class CommandInfo
{
    public string name {get; set;}
    public string description {get; set;}
    public string builderFunction {get; set;}

    public override string ToString()
    {
        return $"Name: {name}\nDescription: {description}\nBuilder Function: {builderFunction}\n";
    }
}