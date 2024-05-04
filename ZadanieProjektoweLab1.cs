using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Xml.Serialization;

[DataContract]
public class Zadanie
{
    [DataMember]
    public int Id { get; set; }
    [DataMember]
    public string Nazwa { get; set; }
    [DataMember]
    public string Opis { get; set; }
    [DataMember]
    public DateTime DataZakonczenia { get; set; }
    [DataMember]
    public bool CzyWykonane { get; set; }

    public Zadanie(int id, string nazwa, string opis, DateTime dataZakonczenia)
    {
        Id = id;
        Nazwa = nazwa;
        Opis = opis;
        DataZakonczenia = dataZakonczenia;
        CzyWykonane = false;
    }
}

public class ManagerZadan
{
    private List<Zadanie> listaZadan;

    public ManagerZadan()
    {
        listaZadan = new List<Zadanie>();
    }

    public void DodajZadanie(Zadanie zadanie)
    {
        listaZadan.Add(zadanie);
    }

    public void UsunZadanie(int id)
    {
        Zadanie zadanie = listaZadan.Find(z => z.Id == id);
        if (zadanie != null)
        {
            listaZadan.Remove(zadanie);
        }
        else
        {
            Console.WriteLine("Zadanie o podanym identyfikatorze nie istnieje.");
        }
    }

    public void WyswietlZadania()
    {
        foreach (var zadanie in listaZadan)
        {
            Console.WriteLine($"Id: {zadanie.Id}, Nazwa: {zadanie.Nazwa}, Opis: {zadanie.Opis}, Data zakończenia: {zadanie.DataZakonczenia}, Wykonane: {zadanie.CzyWykonane}");
        }
    }

    public void ZapiszDoPliku(string sciezka, string format)
    {
        try
        {
            FileStream stream = new FileStream(sciezka, FileMode.Create);
            switch (format.ToLower())
            {
                case "xml":
                    XmlSerializer serializerXml = new XmlSerializer(typeof(List<Zadanie>));
                    serializerXml.Serialize(stream, listaZadan);
                    break;
                case "json":
                    DataContractJsonSerializer serializerJson = new DataContractJsonSerializer(typeof(List<Zadanie>));
                    serializerJson.WriteObject(stream, listaZadan);
                    break;
                default:
                    Console.WriteLine("Nieobsługiwany format.");
                    break;
            }
            stream.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas zapisu do pliku: {ex.Message}");
        }
    }

    public void WczytajZPliku(string sciezka, string format)
    {
        try
        {
            FileStream stream = new FileStream(sciezka, FileMode.Open);
            switch (format.ToLower())
            {
                case "xml":
                    XmlSerializer deserializerXml = new XmlSerializer(typeof(List<Zadanie>));
                    listaZadan = (List<Zadanie>)deserializerXml.Deserialize(stream);
                    break;
                case "json":
                    DataContractJsonSerializer deserializerJson = new DataContractJsonSerializer(typeof(List<Zadanie>));
                    listaZadan = (List<Zadanie>)deserializerJson.ReadObject(stream);
                    break;
                default:
                    Console.WriteLine("Nieobsługiwany format.");
                    break;
            }
            stream.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Błąd podczas wczytywania z pliku: {ex.Message}");
        }
    }
}

class Program
{
    static void Main()
    {
        ManagerZadan manager = new ManagerZadan();

        while (true)
        {
            Console.WriteLine("Wybierz opcję:");
            Console.WriteLine("1. Dodaj zadanie");
            Console.WriteLine("2. Usuń zadanie");
            Console.WriteLine("3. Wyświetl zadania");
            Console.WriteLine("4. Zapisz listę zadań do pliku");
            Console.WriteLine("5. Wczytaj listę zadań z pliku");
            Console.WriteLine("6. Wyjdź");

            string opcja = Console.ReadLine();

            switch (opcja)
            {
                case "1":
                    Console.WriteLine("Podaj nazwę zadania:");
                    string nazwa = Console.ReadLine();
                    Console.WriteLine("Podaj opis zadania:");
                    string opis = Console.ReadLine();
                    Console.WriteLine("Podaj datę zakończenia zadania (w formacie dd.MM.yyyy):");
                    DateTime dataZakonczenia = DateTime.ParseExact(Console.ReadLine(), "dd.MM.yyyy", null);
                    Zadanie noweZadanie = new Zadanie(manager.listaZadan.Count + 1, nazwa, opis, dataZakonczenia);
                    manager.DodajZadanie(noweZadanie);
                    Console.WriteLine("Zadanie zostało dodane.");
                    break;
                case "2":
                    Console.WriteLine("Podaj Id zadania do usunięcia:");
                    int id;
                    if (int.TryParse(Console.ReadLine(), out id))
                    {
                        manager.UsunZadanie(id);
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowy format Id.");
                    }
                    break;
                case "3":
                    manager.WyswietlZadania();
                    break;
                case "4":
                    Console.WriteLine("Podaj nazwę pliku do zapisu:");
                    string sciezkaZapisu = Console.ReadLine();
                    Console.WriteLine("Wybierz format zapisu (xml/json):");
                    string formatZapisu = Console.ReadLine();
                    manager.ZapiszDoPliku(sciezkaZapisu, formatZapisu);
                    Console.WriteLine("Lista zadań została zapisana do pliku.");
                    break;
                case "5":
                    Console.WriteLine("Podaj nazwę pliku do wczytania:");
                    string sciezkaWczytania = Console.ReadLine();
                    Console.WriteLine("Wybierz format wczytywania (xml/json):");
                    string formatWczytywania = Console.ReadLine();
                    manager.WczytajZPliku(sciezkaWczytania, formatWczytywania);
                    Console.WriteLine("Lista zadań została wczytana z pliku.");
                    break;
                case "6":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Nieprawidłowa opcja.");
                    break;
            }
        }
    }
}
