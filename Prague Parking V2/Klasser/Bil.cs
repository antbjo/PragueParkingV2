using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Prague_Parking_V2.Klasser
{
    public class Bil : Fordon
    {
        public override int Storlek => 4;

        public override decimal TimPris => 20;

        public override void AngeRegNr(Parkeringshus pHus)
        {
            FordonsTyp = "CAR";

            while (true)
            {
                RegNr = AnsiConsole.Ask<string>("ANGE BILENS REGISTRERINGSNUMMER: ");

                if (!ÄrRegNrKorrekt(RegNr))
                {
                    AnsiConsole.MarkupLine("[red]OGILTIGT REGISTRERINGSNUMMER!");
                    continue;
                }
                if (pHus.FinnsRegNrRedan(RegNr))
                {
                    AnsiConsole.MarkupLine($"[red]ETT FORDON MED REGISTRERINGSNUMMER '[yellow4]{RegNr.ToUpper()}[/]' EXISTERAR REDAN![/]");
                    continue;
                }

                Console.Clear();
                AnsiConsole.MarkupLine($"DU ANGAV REGISTRERINGSNUMMER: '[yellow4]{RegNr.ToUpper()}[/]'");
                string menyVal = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("STÄMMER REGISTRERINGSNUMRET?")
                        .HighlightStyle(new Style(Color.Yellow4))
                        .AddChoices("JA", "NEJ")

                    );
                if (menyVal == "JA")
                {
                    RegNr = RegNr.ToUpper();
                    break;
                }
            }
        }
    }
}
