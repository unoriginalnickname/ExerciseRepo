using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Text;

namespace PersonalregisterPractice
{
    //Uppgift 1
    //Vilka klasser bör ingå i programmet?
    ///Svar: Det beror på, men jag har en personklass, en registerklass, en kommandoklass

    //Uppgift 2
    //Vilka attribut och metoder bör ingå i dessa klasser?
    /// Svar: personklass bör ha attribut som namn, lön, address.
    /// registerklass bör innehålla main och vara huvudklassen och ha koll på databasen och inputs
    /// kommandoklassen håller koll på och genererar kommandon för enkel åtkomst

    //Uppgift 3
    //Skriv programmet
    //Försök göra programmet så robust och framtidssäkert som möjligt!
    ///Svar: har försökt göra programmet framtidssäkert genom att använda system.commandline
    public class StaffRegistration
    {
        static List<Person> db = new List<Person>();
        static RootCommand command = Commander.InitializeCommands(db);
        static void Main()
        {
            command.Parse("--help").Invoke(); // print help on startup

            while (true)
            {
                command.Parse(Console.ReadLine().Split()).Invoke();
            }
        }
    }

    public static class Commander
    {
        static public RootCommand root = new RootCommand();

        static Command addPersonCommand = new("addperson", "Adds a person");
        static Command listAllCommand = new("listall", "Lists all people in the registry");
        static Option<List<string>> nameOption = new("--name") { Description = "example usage: --name Adam James Matthews", Required = true, Arity = ArgumentArity.ZeroOrMore, AllowMultipleArgumentsPerToken = true };
        static Option<List<string>> streetOption = new("--street") { Description = "example usage: --street Ninjavägen 1337", Required = false, Arity = ArgumentArity.ZeroOrMore, AllowMultipleArgumentsPerToken = true };
        static Option<List<string>> workdayOption = new("--workdays") { Description = "example usage: --workdays Monday Wednesday Saturday", Required = false, Arity = ArgumentArity.ZeroOrMore, AllowMultipleArgumentsPerToken = true };
        static Option<string> salaryOption = new("--salary") { Description = "example usage: --salary 55000", Required = true };
        static Option<string> pidnOption = new("--personalnumber") { Description = "example usage: 19880414-1337", Required = false };

        static public RootCommand InitializeCommands(List<Person> db)
        {
            addPersonCommand.Add(nameOption);
            addPersonCommand.Add(pidnOption);
            addPersonCommand.Add(streetOption);
            addPersonCommand.Add(salaryOption);
            addPersonCommand.Add(workdayOption);

            addPersonCommand.SetAction(p =>
            {
                db.Add(new Person
                {
                    Name = p.GetValue(nameOption),
                    Salary = p.GetValue(salaryOption),
                    PersonNumber = p.GetValue(pidnOption),
                    Street = p.GetValue(streetOption),
                    Workdays = p.GetValue(workdayOption)
                });
            });

            listAllCommand.SetAction(p =>
            {
                db.ForEach(x => Console.WriteLine(x.ToString()));
            });

            root.Add(addPersonCommand);
            root.Add(listAllCommand);

            return root;
        }
    }
    public class Person
    {
        public List<string> Name { get; set; }
        public List<string> Street { get; set; }
        public string Salary { get; set; }
        public string PersonNumber { get; set; }
        public List<string> Workdays { get; set; }
        public Guid PersonGuid { get; private set; } = Guid.NewGuid();
        public override string ToString()
        {   //To future proof we can always use reflection and get the properties instead of hardcoding, but then formatting becomes an issue
            StringBuilder sb = new StringBuilder();

            sb.Append($"{string.Join(' ', Name)}");
            if (PersonNumber != default)
                sb.Append($", ({PersonNumber})");
            if (Street.Count != 0)
                sb.Append($", Address: {string.Join(' ', Street)}");
            if (Salary != default)
                sb.Append($", Salary: {Salary}");
            if (Workdays.Count != 0)
                sb.Append($", Workdays: " + string.Join(", ", Workdays));
            //sb.Append($", GUID: " + string.Join(", ", PersonGuid.ToString()));
            return sb.ToString();
        }
    };
}