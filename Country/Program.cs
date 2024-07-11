
using Country.Concrets;
using Country.Services.Command;
using Country.Services.Query;
using System.ComponentModel;

bool exit = false;

while (!exit)
{
    Console.WriteLine("-----MENU-----");
    Console.WriteLine("0.Exit");
    Console.WriteLine("1.Display Countries");
    Console.WriteLine("2.Add Country");
    Console.WriteLine("3.Update Country");
    Console.WriteLine("4.Delete Country");
    Console.WriteLine("5.Display Citites");
    Console.WriteLine("6.Add City");
    Console.WriteLine("7.Update City");
    Console.WriteLine("8.Delete City");

    Console.Write("Write your choice: ");
    bool canCoverte = int.TryParse(Console.ReadLine(), out int choice);

    if (canCoverte)
    {
        switch ((Operations)choice)
        {
            case Operations.Exit:
                return;
            case Operations.DisplayCountries:
                Display.DisplayCountries();
                break;
            case Operations.AddCountry:
                Adding.AddCountry();
                break;
            case Operations.UpdateCountry:
                Display.DisplayCountries();
                Update.UpdateCountry();
                break;
            case Operations.DeleteCountry:
                Display.DisplayCountries();
                Delete.DeleteCountry();
                break;
            case Operations.DisplayCities:
                Display.DisplayCities();
                break;
            case Operations.AddCity:
                Display.DisplayCountries();
                Adding.AddCity();
                break;
            case Operations.UpdateCity:
                Display.DisplayCities();
                Update.UpdateCity();
                break;
            case Operations.DeleteCity:
                Display.DisplayCities();
                Delete.DeleteCity();
                break;
            default:
                Console.WriteLine("There is not a option like that");
                break;
        }
    }
    else
        Messages.InvalidInput();
}