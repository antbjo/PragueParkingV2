using Spectre.Console;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Prague_Parking_V2.Klasser
{
    public class Parkeringshus
    {
        public List<Parkeringsruta> Parkeringsrutor { get; set; } = new();
        public int AntalPlatser { get; set; } = 100;
        Prislista prislista = new Prislista();
        public Parkeringshus()
        {
            var Parkeringsrutor = new List<Parkeringsruta>();
            for (int i = 0; i < AntalPlatser; i++)
            {
                this.Parkeringsrutor.Add(new Parkeringsruta(i + 1, 4));
            }
        }

        public int ParkeraFordon(Fordon fordon)
        {
            Console.Clear();
            foreach (var ruta in Parkeringsrutor)
            {
                int platsStorlek = 0;
                foreach (var parkeratFordon in ruta.ParkeradeFordon)
                {
                    platsStorlek += parkeratFordon.Storlek;

                }
                if (platsStorlek + fordon.Storlek <= ruta.Kapacitet)
                {
                    fordon.StartTid = DateTime.Now;
                    ruta.ParkeradeFordon.Add(fordon);
                    return ruta.Platsnummer;
                }
            }
            AnsiConsole.MarkupLine("[red]PARKERINGSHUSET ÄR FULLT!");
            return 0;
        }

        public void VisaGaraget()
        { 
            while (true)
            {
                Console.Clear();
                var grid = new Grid();

                for (int i = 0; i < 4; i++)
                {
                    grid.AddColumn(new GridColumn { Padding = new Padding(1, 0, 1, 0), Width = 30, Alignment = Justify.Left });
                }

                for (int i = 0; i < Parkeringsrutor.Count; i += 4)
                {
                    string[] parkeringsrutor = new string[4];
                    for (int j = 0; j < 4; j++)
                    {
                        int index = i + j;
                        if (index < Parkeringsrutor.Count)
                        {
                            var ruta = Parkeringsrutor[index];
                            if (ruta.ParkeradeFordon.Count == 0)
                            {
                                parkeringsrutor[j] = $"[yellow]{ruta.Platsnummer:D3}[/]  [green]LEDIG[/]";
                            }
                            else if (ruta.ParkeradeFordon.Count == 1 && ruta.ParkeradeFordon[0].FordonsTyp == "MC")
                            {
                                parkeringsrutor[j] = $"[yellow]{ruta.Platsnummer:D3}[/]  [yellow4]{ruta.ParkeradeFordon[0].RegNr.ToUpper()}[/]";
                            }
                            else
                            {
                                string regNr = "";
                                foreach (var fordon in ruta.ParkeradeFordon)
                                {
                                    regNr += $"{fordon.FordonsTyp}#{fordon.RegNr.ToUpper()}  ";
                                }
                                parkeringsrutor[j] = $"[yellow]{ruta.Platsnummer:D3}[/]  [red]{regNr.Trim()}[/]";
                            }

                        }
                    }
                    grid.AddRow(parkeringsrutor);
                }
                AnsiConsole.MarkupLine("\n[green]GRÖN[/] = LEDIG, [yellow4]GUL[/] = PLATS FÖR MC, [red]RÖD[/] = UPPTAGEN\n");
                AnsiConsole.Write(grid);
                Console.WriteLine();
                List<string> menyVal = [
                            "REDIGERA FORDON",
                            "TILLBAKA TILL HUVUDMENY"
                                ];

                string tvåVal = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .HighlightStyle(new Style(Color.Yellow4))
                    .AddChoices(menyVal)
                    );
                switch (tvåVal)
                {
                    case "REDIGERA FORDON":
                        Fordon fordon = SökFordon();
                        if (fordon != null)
                        {
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
                                    FlyttaFordon(fordon);
                                    break;
                                case "CHECKA UT":
                                    CheckaUt(fordon);
                                    break;
                                case "TILLBAKA TILL HUVUDMENY":
                                    return;
                            }
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]FORDONET HITTADES INTE![/]");
                            Console.ReadLine();
                        }
                        break;
                    case "TILLBAKA TILL HUVUDMENY":
                        return;
                }
            }
        }

        public Fordon SökFordon()
        {
            string sökFordon = AnsiConsole.Ask<string>("ANGE REGISTRERINGSNUMMER: ");

            foreach (var ruta in Parkeringsrutor)
            {
                foreach (var fordon in ruta.ParkeradeFordon)
                {
                    if (fordon.RegNr.Equals(sökFordon, StringComparison.OrdinalIgnoreCase))
                    {
                        return fordon;
                    }
                }
            }
            return null;
        }
        public int HämtaPlats(Fordon fordon)
        {
            foreach (var ruta in Parkeringsrutor)
            {
                if (ruta.ParkeradeFordon.Contains(fordon))
                {
                    return ruta.Platsnummer;
                }
            }
            return 0;
        }

        public void FlyttaFordon(Fordon fordonet)
        {
            Parkeringsruta gammalRuta = null;
            Parkeringsruta nyRuta = null;

            foreach (var ruta in Parkeringsrutor)
            {
                foreach (var fordon in ruta.ParkeradeFordon)
                {
                    if (ruta.ParkeradeFordon.Contains(fordonet))
                    {
                        gammalRuta = ruta;
                        break;
                    }
                }
            }

            if (fordonet == null)
            {
                AnsiConsole.MarkupLine("[red]FORDONET HITTADES INTE![/]");
                return;
            }

            int nyPlats = AnsiConsole.Ask<int>("ANGE PARKERINGSPLATS SOM DU VILL FLYTTA FORDONET TILL (1-100): ");
            Console.Clear();

            if (nyPlats < 1 || nyPlats > Parkeringsrutor.Count)
            {
                AnsiConsole.MarkupLine("[red]OGILTIG PARKERINGSPLATS![/]");
                return;
            }

            nyRuta = Parkeringsrutor[nyPlats - 1];

            if (nyPlats == gammalRuta.Platsnummer)
            {
                AnsiConsole.MarkupLine("[red]FORDONET STÅR REDAN PÅ DEN PLATSEN![/]");
                return;
            }

            int platsStorlek = 0;
            foreach (var parkeratFordon in nyRuta.ParkeradeFordon)
            {
                platsStorlek += parkeratFordon.Storlek;
            }

            if (platsStorlek + fordonet.Storlek > nyRuta.Kapacitet)
            {
                AnsiConsole.MarkupLine("[red]PARKERINGSRUTAN HAR INTE TILLRÄCKLIGT MED PLATS![/]");
                return;
            }

            gammalRuta.ParkeradeFordon.Remove(fordonet);
            nyRuta.ParkeradeFordon.Add(fordonet);

            AnsiConsole.MarkupLine($"FORDONET '[green]{fordonet.RegNr.ToUpper()}[/]' HAR PARKERATS PÅ PLATS: [green]{nyRuta.Platsnummer}[/]");
        }

        public void CheckaUt(Fordon fordonet)
        {
            string checkaUt = fordonet.RegNr;
            Console.Clear();

            string menyVal = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"ÄR DU SÄKER ATT DU VILL CHECKA UT '{fordonet.RegNr.ToUpper()}'?")
                    .HighlightStyle(new Style(Color.Yellow4))
                    .AddChoices("JA", "NEJ")
                );
            switch (menyVal)
            {
                case "JA":
                    fordonet = null;
                    foreach (var ruta in Parkeringsrutor)
                    {
                        foreach (var fordon in ruta.ParkeradeFordon)
                        {
                            if (fordon.RegNr.Equals(checkaUt, StringComparison.OrdinalIgnoreCase))
                            {
                                fordonet = fordon;
                                break;
                            }
                        }
                        if (fordonet != null)
                        {
                            Console.Clear();
                            ruta.ParkeradeFordon.Remove(fordonet);
                            AnsiConsole.MarkupLine($"FORDONET '[yellow4]{checkaUt.ToUpper()}[/]' HAR CHECKAT UT");
                            AnsiConsole.MarkupLine($"PARKERAD TID: {fordonet.HämtaTidParkerad().Hours} TIMMAR OCH {fordonet.HämtaTidParkerad().Minutes % 60} MINUTER");
                            AnsiConsole.MarkupLine($"BELOPP ATT BETALA: {BeräknaPris(fordonet, prislista)} Kč");
                            break;
                        }
                    }
                    if (fordonet == null)
                    {
                        AnsiConsole.MarkupLine("[red]FORDONET HITTADES INTE![/]");
                    }
                    break;

                case "NEJ":
                    return;
            }
        }
        public bool FinnsRegNrRedan(string regNr)
        {
            foreach (var ruta in Parkeringsrutor)
            {
                foreach (var fordon in ruta.ParkeradeFordon)
                {
                    if (fordon.RegNr.Equals(regNr, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public decimal BeräknaPris(Fordon fordon, Prislista prislista)
        {
            decimal timPris = 0;
            if (fordon is Bil)
            {
                timPris = prislista.BilPris;
            }
            else if (fordon is MC)
            {
                timPris = prislista.MCPris;
            }

            TimeSpan tidParkerad = fordon.HämtaTidParkerad();
            if (tidParkerad.TotalMinutes < 10)
            {
                return 0;
            }
            return (decimal)Math.Ceiling(tidParkerad.TotalHours) * fordon.TimPris;
        }

    }
}

