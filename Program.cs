using System;

namespace Curit_Løsning
{

    /// <summary>
    /// Først of fremmest, så er det vigtigt at vi er så effektive som muligt.
    /// Vores største gebyr er de rå bordplader. Vi skal sikre os at vi kan lave så mange bordplader
    /// ud fra 1 rå bordplade som overhovedet muligt.
    /// For det tjekker jeg alle 3 dimensioner, på alle vinkler, for at sikre vi laver så mange bordplader som muligt.
    /// </summary>
    class Program
    {
        //Her laver jeg klassen til den Cube som vi vil bruges til at lave objekter af den rå bordplade.
        public class Cube {
            public double x { get; private set; }
            public double y { get; private set; }
            public double z { get; private set; }

            public Cube(double x, double y, double z)
            {
                this.x = Math.Abs(x);
                this.y = Math.Abs(y);
                this.z = Math.Abs(z);
            }
        }
        //targetCube er den bordplade vi skal sælge, det er vores skabelon.
        //rawCube er den rå bordplade vi modtager fra producenten i holland.
        public static Cube targetCube = new Cube(2000, 610, 27);
        public static Cube rawCube = new Cube(6000, 800, 27);

        static void Main(string[] args)
        {
            //Her dette er vores store funktion. Her tjekker vi
            static int calcNumTargetsInRaw(Cube targetCube, Cube rawCube)
            {
                //Hvis en af vores sider er 0 (Hvilket ikke kan lade sig gøre, det er kun hvis der er en der skriver forkert) så er der ingen grund til at fortsætte, fordi X * 0 = 0
                if (targetCube.x == 0 || targetCube.y == 0 || targetCube.z == 0)
                {
                    return 0;
                }

                //Hvis vores skabelon er større end vores rå bordplade, så er der ingen grund til at fortsætte, da den ikke kan passe
                if (targetCube.x > rawCube.x || targetCube.y > rawCube.y || targetCube.z > rawCube.z)
                {
                    return 0;
                }

                //Her tjekker vi hvor mange gange den kan være i en bestemt side
                double numFitX = Math.Floor(rawCube.x / targetCube.x);
                double numFitY = Math.Floor(rawCube.y / targetCube.y);
                double numFitZ = Math.Floor(rawCube.z / targetCube.z);

                //Her laver vi cubes af hvad der er tilbage. Vi skære skabelonen ud af den rå plade, og gemmer de resterende ved at lave 3 seperate cubes.
                //Dette tager nok lidt forklaring, men når man fjerne en cube fra en cube, så laver man 5 mulige nye cubes, men eftersom nogle skære over hinanden, behøver vi blot 3.
                Cube[] leftoverCubes = new Cube[]
                {
                    new Cube(
                        rawCube.x - numFitX * targetCube.x,
                        rawCube.y,
                        rawCube.z
                    ),
                    new Cube(
                        rawCube.x,
                        rawCube.y - numFitY * targetCube.y,
                        rawCube.z
                    ),
                    new Cube(
                        rawCube.x,
                        rawCube.y,
                        rawCube.z - numFitZ * targetCube.z
                    )
                };

                int maxNumFitInAnyLeftover = 0;
                //Her tjekker vi så hvor meget der er til overs ved hjælp af vores function der beregner hvor mange gange vi kan skære i leftover cubes
                foreach (var leftoverCube in leftoverCubes)
                {
                    maxNumFitInAnyLeftover = Math.Max(maxNumFitInAnyLeftover, calcMaxNumFitTargetsInRaw(targetCube, leftoverCube));
                }
                //Så sender vi tallet tilbage så man ved hvor mange færdige bordplader man kan producere ud fra 1 bordplade
                return (int)(numFitX * numFitY * numFitZ) + maxNumFitInAnyLeftover;
            }

            static int calcMaxNumFitTargetsInRaw(Cube targetCube, Cube rawCube)
            {
                //Igen, hvis 1 af dem er 0, så skal vi bare stoppe
                if (rawCube.x <= 0 || rawCube.y <= 0 || rawCube.z <= 0)
                {
                    return 0;
                }
                //Her løber vi alle 6 muligheder igennem. Der er 8 ialt, men der er 2 der spejler sig selv, så dem beregner vi ikke, da de ville yde samme resultat.
                int[] tests = new int[] {
                    calcNumTargetsInRaw(new Cube(targetCube.x, targetCube.y, targetCube.z), rawCube),
                    calcNumTargetsInRaw(new Cube(targetCube.y, targetCube.x, targetCube.z), rawCube),
                    calcNumTargetsInRaw(new Cube(targetCube.z, targetCube.x, targetCube.y), rawCube),
                    calcNumTargetsInRaw(new Cube(targetCube.x, targetCube.z, targetCube.y), rawCube),
                    calcNumTargetsInRaw(new Cube(targetCube.y, targetCube.z, targetCube.x), rawCube),
                    calcNumTargetsInRaw(new Cube(targetCube.z, targetCube.y, targetCube.x), rawCube),
                };
                int maxCut = 0;
                //Så looper vi vores array igennem, og finder det største tal, hvis der er et tal, så har vi fundet endnu en udskæring
                foreach (var item in tests)
                {
                    maxCut = Math.Max(maxCut, item);
                }
                return maxCut;
            }
            Console.WriteLine(calcMaxNumFitTargetsInRaw(targetCube, rawCube));

            Random rnd = new Random();

            //Mængden af Plader vi skal lave
            double desiredAmount = 100;
            //Mængden af Plader vi har lavet
            double producedAmount = 0;
            //Tiden i minutter, det tager at forberede maskinen dagligt
            double setupTime = 20;
            //Produktions tid per bordplade i minutter
            double timePerBoard = 15 + 10;
            //Hvor meget tid der er blevet brugt på produktionen af bordplader i dag
            double totalTimeSpentOnBoards = 0 + setupTime;
            //Hvor mange dage vi har brugt på at producere bordplader, hver 5. dag forventer vi lager levering og skal bruge 1 dag på at holde styr på lageret
            double daysSpentOnBoards = 0;
            //Hvor mange dage der går imellem vi skal holde lagerhåndtering
            double daysBetweenRestock = 5;
            //Hvor mange dage vi har brugt på at opretholde lageret
            double storageDaysSpent = 0;
            //Hvor mange timer den ansatte skal arbejde dagligt
            double workDaySchedule = 7;
            //Hvor meget maskinen koster i timen i kr
            double machinePricePerHour = 500; 
            //Hvor meget den ansatte koster i timen i kr
            double laborPricePerHour = 350;
            //Hvor mange gange maskinen bliver sat op. Den starter på 1, da den skal sættes op om morgenen før vi kan begynde på de første bordplader
            double setupCount = 1;
            //Timepris for produktionen af bordpladen
            double hourCostProduction = (timePerBoard / 60) * (machinePricePerHour + laborPricePerHour);
            //Timepris for opsætning af udstyr
            double hourCostSetup = (setupTime / 60) * (machinePricePerHour + laborPricePerHour);
            //Prisen på en rå bordplade
            double rawBoardPrice = 1200;
            //Mængden af salgsklar bordplader vi kan lave af 1 rå bordplade
            int cutsPossible = calcNumTargetsInRaw(targetCube, rawCube);
            //Mængden af rå bordplader der skal bestilles for færest penge.
            double rawBoardsNeeded = Math.Ceiling(desiredAmount / cutsPossible * 1.1 * 1.03);
            Console.WriteLine(rawBoardsNeeded);
            //Mængden af rå bordplader vi har på lager
            double rawBoardStock = 12;
            //Hvis True, producere vi så mange bordplader som vi kan, baseret på hvor mange rå bordplader vi har på lager
            bool produceByStock = false;
            double boardsBroken = 0;

            //Her beregner vi skal producere ud fra lageret.
            if (produceByStock)
            {
                rawBoardsNeeded = rawBoardStock;
                //hvis det er tilfældet, skal vi bare lave mængden af bordplader vi vil lave, til vores lager * mængden af bordplader vi kan lave fra 1 rå bordplade
                desiredAmount = rawBoardStock * cutsPossible;
            }

            double sellPrice = 5000;

            //Eftersom der er en chance for at boarded kan ødelægges eller det ikke går igennem kvalitets tjek, så skal det kasseres og ikke sælges vidre.
            for (int i = 0; i < desiredAmount; i++)
            {
                totalTimeSpentOnBoards += timePerBoard;
                //Der er 10% chance for at bordpladen går i stykker under savning. Hvis ikke, så bliver det sendt videre til inspektion
                if(rnd.Next(0,101) > 10) 
                { 
                    //Boardet er nu færdigt, og vi tjekker så om boardet er godt nok, det har 3% chance for ikke at bestå inspektionen, hvis den består,
                    //bliver den tilføjet til bunken af færdige bordplader.
                    if(rnd.Next(0,101) > 3) { producedAmount++; }
                    else if (rawBoardsNeeded * cutsPossible > desiredAmount)
                    {
                        desiredAmount++;
                        boardsBroken++;
                    }
                }
                else if (rawBoardsNeeded * cutsPossible > desiredAmount)
                {
                    desiredAmount++;
                    boardsBroken++;
                }
                //Her tjekker vi om vores arbejdsdag er gennenmført. Hvis den er det, så vil saven skulle sættes op igen, dagen efter.
                if (totalTimeSpentOnBoards/60 > workDaySchedule)
                {
                    totalTimeSpentOnBoards = 0;
                    daysSpentOnBoards ++;
                    Console.WriteLine("We have started a new day! We will now setup the machines again, this is the " + setupCount + " day of production!");
                    setupCount++;
                    if(daysSpentOnBoards % daysBetweenRestock == 0)
                    {
                        storageDaysSpent++;
                        Console.WriteLine("We will not produce anything today, we will focus on the storage");
                        daysSpentOnBoards++;
                    }
                }
            }
            //Her laver vi alt matematiken der skal give os et overblik over hvordan produktionen gik.
            double price = (hourCostProduction * desiredAmount) + (hourCostSetup * setupCount) + (rawBoardPrice * rawBoardsNeeded) + (storageDaysSpent * laborPricePerHour / 60);
            double pricePerBoard = price / desiredAmount;
            double totalGain = sellPrice * desiredAmount - price;

            //Her udregner vi hvor meget spild vi får af ødelagte bordplader
            double totalWastedMaterial = (boardsBroken/desiredAmount) * 100;

            Console.WriteLine(desiredAmount - producedAmount + " boards broke or failed check during construction");
            Console.WriteLine("Boards produced: " + Math.Round(producedAmount, 1) + "\nTotal Cost: " + Math.Round(price, 1) + "\nManufacturing price per Board: " + Math.Round(pricePerBoard, 1) + "\nProfit: " + Math.Round(totalGain,1) + "\nProfit per board: " + Math.Round((sellPrice - pricePerBoard),1) + "\nDays Spent in total: " + daysSpentOnBoards + "\nWe wasted " + Math.Round(totalWastedMaterial, 2) + "% of our material"); ;
            Console.ReadLine();
        }
    }
}
