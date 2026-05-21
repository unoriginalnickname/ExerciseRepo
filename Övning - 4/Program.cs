using Övning___4.Commands;

try
{
    CommandVault vault = new CommandVault();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Unexpected error: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
    Console.ResetColor();
}