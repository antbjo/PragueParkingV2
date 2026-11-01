using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Prague_Parking_V2.Klasser
{
    public class Meny
    {
        private Parkeringshus pHus;
        private FileHandler fil;
        private Config config;
        private Prislista prislista = new Prislista();

        public Meny()
        {
            prislista.LäsPrislista();
            fil = new FileHandler();
            config = fil.LäsConfig();
            pHus = fil.LäsParkeringsHus();
            if (pHus == null)
            {
                pHus = new Parkeringshus();
            }
        }

        public void SkrivUtHuvudMeny()
        {
            Console.Clear();
            AnsiConsole.Write(new FigletText("PRAGUE PARKING").Color(Color.Yellow4));
            List<string> meny = [
                "REGISTRERA FORDON",
                "SÖK FORDON",
                "VISA PARKERINGSPLATSER",
                "AVSLUTA PROGRAM"
            ];

            string menyVal = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .HighlightStyle(new Style(Color.Yellow4))
                .AddChoices(meny)
                );

            switch (menyVal)
            {
                case "REGISTRERA FORDON":
                    RegistreraFordon();
                    break;

                case "SÖK FORDON":
                    Fordon fordon = pHus.SökFordon();
                    if (fordon != null)
                    {
                        Console.Clear();
                        TimeSpan tid = fordon.HämtaTidParkerad();
                        int pPlats = pHus.HämtaPlats(fordon);
                        AnsiConsole.MarkupLine($"FORDONET MED REGISTRERINGSNUMMER '[yellow4]{fordon.RegNr.ToUpper()}[/]' STÅR PARKERAD PÅ PLATS [yellow4]{pPlats}[/].");
                        AnsiConsole.MarkupLine($"PARKERINGSTID: {tid.Minutes} MINUTER OCH {tid.Seconds % 60} SEKUNDER");
                        AnsiConsole.MarkupLine($"BELOPP: {pHus.BeräknaPris(fordon, prislista)} Kč");

                        List<string> valMeny = [
                            "FLYTTA",
                            "CHECKA UT",
                            "TILLBAKA TILL HUVUDMENY"
                        ];

                        string nyaVal = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                            .HighlightStyle(new Style(Color.Yellow4))
                            .AddChoices(valMeny)
                            );
                        switch (nyaVal)
                        {
                            case "FLYTTA":
                                pHus.FlyttaFordon(fordon);
                                break;
                            case "CHECKA UT":
                                pHus.CheckaUt(fordon);
                                break;
                            case "TILLBAKA TILL HUVUDMENY":
                                SkrivUtHuvudMeny();
                                break;
                        }

                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]FORDONET HITTADES INTE![/]");
                    }
                    Console.ReadLine();
                    SkrivUtHuvudMeny();
                    break;

                case "VISA PARKERINGSPLATSER":
                    pHus.VisaGaraget();
                    //pHus.VisaParkeringsPlatser();
                    SkrivUtHuvudMeny();
                    break;

                case "AVSLUTA PROGRAM":
                    AvslutaProgram();
                    break;
            }
        }
        public void RegistreraFordon()
        {
            Console.Clear();
            List<string> fordonsMeny = [
                "BIL",
                "MC"
                ];

            string menyVal = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("--- VÄLJ FORDONSTYP ---")
                .HighlightStyle(new Style(Color.Yellow4))
                .AddChoices(fordonsMeny)
                );

            switch (menyVal)
            {
                case "BIL":
                    Bil bil = new Bil();
                    bil.AngeRegNr(pHus);
                    int pPlats = pHus.ParkeraFordon(bil);
                    AnsiConsole.MarkupLine($"[yellow4]{bil.SättIhopRegNrOchFordonstyp()}[/] HAR PARKERATS PÅ PLATS [yellow4]{pPlats}[/]");
                    RegistreraNyttFordon();
                    break;

                case "MC":
                    MC mc = new MC();
                    mc.AngeRegNr(pHus);
                    pPlats = pHus.ParkeraFordon(mc);
                    AnsiConsole.MarkupLine($"[yellow4]{mc.SättIhopRegNrOchFordonstyp()}[/] HAR PARKERATS PÅ PLATS [yellow4]{pPlats}[/]");
                    RegistreraNyttFordon();
                    break;
            }
        }

        public void RegistreraNyttFordon()
        {
            string registreraNyttFordon = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                        .Title("\nVILL DU REGISTRERA ETT NYTT FORDON?")
                        .HighlightStyle(new Style(Color.Yellow4))
                        .AddChoices("JA", "NEJ")
                        );
            switch (registreraNyttFordon)
            {
                case "JA":
                    RegistreraFordon();
                    break;

                case "NEJ":
                    SkrivUtHuvudMeny();
                    break;
            }
        }
        public void TillbaksTillMeny()
        {
            AnsiConsole.MarkupLine("\nTRYCK PÅ VALFRI TANGENT FÖR ATT KOMMA TILLBAKA TILL HUVUDMENYN...");
            Console.ReadKey();
        }
        public void AvslutaProgram()
        {
            string menyVal = AnsiConsole.Prompt<string>(
                new SelectionPrompt<string>()
                .Title("VILL DU AVSLUTA PROGRAMMET?")
                .HighlightStyle(new Style(Color.Yellow4))
                .AddChoices("JA")
                .AddChoices("NEJ")
                .DefaultValue("NEJ")
                );
            switch (menyVal)
            {
                case "JA":
                    fil.SparaParkeringsHus(pHus);
                    Environment.Exit(0);
                    break;

                case "NEJ":
                    SkrivUtHuvudMeny();
                    break;
            }
        }
    }
}
