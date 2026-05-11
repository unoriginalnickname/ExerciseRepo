using System.Reflection.Metadata.Ecma335;

Console.WriteLine("Huvudmeny - Navigera med 1-9" +
      "\n1 - prisanpassning för åldersgrupper" +
      "\n2 - räkna ut totalpris för grupp" +
      "\n3 - upprepa inmatad text" +
      "\n4 - skriv ut det tredje ordet i en mening" +
      "\n0 - stäng av programmet");


bool runLoop = true;

while (runLoop)
{
    switch (Console.ReadKey().KeyChar)
    {
        default:
            Console.WriteLine("Okänt kommando");
            break;
        case '0':
            runLoop = false;
            Console.WriteLine("Programmet avslutas");
            break;
        case '1':
            AgeControl();
            break;
        case '2':
            PartyPricing();
            break;
        case '3':
            RepeatText();
            break;
        case '4':
            SplitText();
            break;
        case '5':
            break;
        case '6':
            break;
        case '7':
            break;
        case '8':
            break;
        case '9':
            break;
    }
}

void AgeControl()
{
    int ageInt;

    Console.WriteLine("\nAnge ålder för att se prisanpassning:\n");

    int.TryParse(Console.ReadLine(), out ageInt);

    if (ageInt < 20)
        Console.WriteLine("Ungdomspris: 80kr");

    else if (ageInt > 64)
        Console.WriteLine("Pensionärspris: 90kr");

    else
        Console.WriteLine("Standardpris: 120kr");
}
void PartyPricing()
{
    int numPeople, age, totalCost = 0;

    Console.WriteLine("\nAnge hur många som ingår i sällskapet: ");

    int.TryParse(Console.ReadLine(), out numPeople);

    for (int i = 1; i < numPeople + 1; i++)
    {
        Console.WriteLine("Ange ålder på person " + i + ":"); // lol sätt i + 1
        int.TryParse(Console.ReadLine(), out age);
        totalCost += RetrieveCost(age);
    }
    Console.WriteLine(numPeople + "personer\n" + "kostnad: " + totalCost + "kr");
}
int RetrieveCost(int age)
{
    if (age < 20)
        return 80;
    else if (age > 64)
        return 90;
    else
        return 120;
}
void RepeatText()
{
    Console.WriteLine("\nAnge text som ska repeteras 10 gånger: ");
    string textToRepeat = Console.ReadLine();
    for (int i = 1; i < 10 + 1; i++)
    {
        if (i < 10)
            Console.Write(i + ". " + textToRepeat + ", ");
        else
            Console.Write(i + ". " + textToRepeat + ".");
    }
}
void SplitText()
{
    Console.WriteLine("\nAnge en mening med minst 3 ord för att skriva ut det tredje ordet");
    string mening = Console.ReadLine();

    var uppdeladMening = mening.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    if (uppdeladMening.Length < 2)
    {
        Console.WriteLine("Meningen var för kort");
        return;
    }

    Console.WriteLine(uppdeladMening[2]);
}