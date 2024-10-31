using System;
using System.Collections.Generic;
using System.Threading;

namespace BuckshotRoulette_ShellTracker {

    internal class ShellTracker {

        static int liveShells = 0;  // Keeps track of white shells in shotgun
        static int blankShells = 0; // Keeps track of blank shells in shotgun

        static void Main(string[] args) {

            bool trackAgain = true;     // Keeps tracking if user wants to continue
            bool allShellsIn = false;   // Doesn't ask for shell count if already inputted

            while (trackAgain) {

                bool userReset = false;  // If true, doesn't ask whether user wants to continue or not

                // Controls the input of the live shells
                while ((liveShells <= 0 || liveShells > 5) && !allShellsIn) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("LIVE"); Console.ResetColor();
                    Console.Write(" SHELLS: ");
                    char shellNumberInput = Console.ReadKey().KeyChar;

                    if (Char.IsNumber(shellNumberInput)) {
                        int convertedInputNumber = Convert.ToInt32(shellNumberInput.ToString());
                        if (convertedInputNumber <= 0) {
                            ColoredMessage("\nNumber must be higher than 0.", ConsoleColor.Red);
                            ClearScreen(true);
                        } else if (convertedInputNumber > 5) {
                            ColoredMessage("\nNumber can't be higher than 5.", ConsoleColor.Red);
                            ClearScreen(true);
                        } else {
                            liveShells = convertedInputNumber;
                        }
                    } else {
                        ClearScreen(false);
                    }
                }

                // Controls the input of the blank shells
                while ((blankShells <= 0 || blankShells > 5) && !allShellsIn) {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("\nBLANK"); Console.ResetColor();
                    Console.Write(" SHELLS: ");
                    char shellNumberInput = Console.ReadKey().KeyChar;

                    if (Char.IsNumber(shellNumberInput)) {
                        int convertedInputNumber = Convert.ToInt32(shellNumberInput.ToString());
                        if (convertedInputNumber <= 0) {
                            ColoredMessage("\nNumber must be higher than 0.", ConsoleColor.Red);
                            ClearScreen(true);
                        } else if (convertedInputNumber > 5) {
                            ColoredMessage("\nNumber can't be higher than 5.", ConsoleColor.Red);
                            ClearScreen(true);
                        } else {
                            blankShells = convertedInputNumber;
                        }
                    } else {
                        ClearScreen(false);
                    }
                }

                allShellsIn = true;
                bool userContinue = true;
                int burnerShellTurns = -1;
                string burnerShellText = "";

                // While a shell is in, shows this menu
                while (liveShells > 0 || blankShells > 0) {

                    ShellStatus();
                    Console.WriteLine("\n[L] Live Shell Out");
                    Console.WriteLine("[B] Blank Shell Out");
                    Console.WriteLine("[S] Polarized Shell");
                    Console.WriteLine("[P] Burner Phone");
                    Console.WriteLine("[R] End tracking\n");

                    if (!burnerShellText.Equals("") && burnerShellTurns != -1) {
                        if (burnerShellTurns == 1) {
                            burnerShellText = "NOW";
                            burnerShellTurns = 0;
                        } else {
                            burnerShellTurns--;
                        }
                        Console.WriteLine($"{burnerShellText} > {burnerShellTurns}");
                    }

                    char shellMenuOption = Console.ReadKey().KeyChar; // Option Input
                    switch (shellMenuOption) {
                        case 'L': case 'l': // Live shell goes out
                            if (liveShells > 0) { liveShells--; }
                            break;

                        case 'B': case 'b': // Blank shell goes out
                            if (blankShells > 0) { blankShells--; }
                            break;

                        case 'S': case 's': // Shell was polarized
                            ShellStatus();
                            Console.WriteLine("\n[L] Live Polarized Shell");
                            Console.WriteLine("[B] Blank Polarized Shell");
                            char typeShellPolarized = Console.ReadKey().KeyChar;
                            switch (typeShellPolarized) {
                                case 'L': case 'l': // Shell after polarization was live
                                    if (blankShells > 0) blankShells--;
                                    break;
                                case 'B': case 'b': // Shell after polarization was blank
                                    if (liveShells > 0) liveShells--;
                                    break;
                            }
                            break;

                        case 'P': case 'p': // Burner phone info
                            if (liveShells + blankShells > 3) {
                                ShellStatus();
                                Console.WriteLine("\n- BURNER PHONE -");
                                Console.WriteLine("[L] Warns Live Shell");
                                Console.WriteLine("[B] Warns Blank Shell");
                                char burnerShellType = Console.ReadKey().KeyChar;
                                switch (burnerShellType) {
                                    case 'L': case 'l':
                                        burnerShellText = "LIVE";
                                        break;
                                    case 'B': case 'b':
                                        burnerShellText = "BLANK";
                                        break;
                                }

                                bool properInput = false;
                                while (!properInput) {
                                    ShellStatus();
                                    Console.WriteLine("\n- BURNER PHONE -");
                                    Console.WriteLine("[Nº] Distance to Shell");
                                    char charBurnerShellTurns = Console.ReadKey().KeyChar;

                                    if (Char.IsNumber(charBurnerShellTurns)) {
                                        int convertedNumber = Convert.ToInt32(charBurnerShellTurns.ToString());
                                        if (convertedNumber <= (liveShells + blankShells)) {
                                            burnerShellTurns = Convert.ToInt32(charBurnerShellTurns.ToString());
                                            properInput = true;
                                        } else {
                                            ColoredMessage("Shell out of range.", ConsoleColor.Red);
                                        }
                                        burnerShellTurns = Convert.ToInt32(charBurnerShellTurns.ToString());
                                    } else {
                                        ColoredMessage("Value was not a number.", ConsoleColor.Red);
                                    }
                                }

                            }
                            break;

                        case 'R': case 'r': // User chooses to reset
                            liveShells = blankShells = 0;
                            userReset = true;
                            break;

                        default:
                            ColoredMessage("Option unavailable.", ConsoleColor.Red);
                            System.Environment.Exit(0);
                            break;
                    }
                }

                if (!userReset) {
                    ShellStatus();
                    Console.WriteLine("\n[Y] Continue tracking");
                    Console.WriteLine("[N] Exit program\n");
                    Console.Write("> ");
                    char mainMenuOption = Console.ReadKey().KeyChar;
                    switch (mainMenuOption) {
                        case 'Y': case 'y':
                            allShellsIn = false;
                            ClearScreen(false);
                            break;
                        case 'N': case 'n':
                            trackAgain = false;
                            break;
                    }
                } else { 
                    allShellsIn = false;
                    ClearScreen(false);
                }

            }

        }

        /// <summary>
        /// Shows shell status
        /// </summary>
        static void ShellStatus() {
            ClearScreen(false);
            ColoredMessage("LIVE", ConsoleColor.Red);
            Console.Write($" SHELLS:  {liveShells}\n");
            ColoredMessage("BLANK", ConsoleColor.Cyan);
            Console.Write($" SHELLS: {blankShells}\n");
        }

        /// <summary>
        /// Shows a message on-screen with the specified color
        /// </summary>
        static void ColoredMessage(string msg, ConsoleColor color) {
            Console.ForegroundColor = color;
            Console.Write(msg);
            Console.ResetColor();
        }

        /// <summary>
        /// Clears the screen after 1.5 seconds
        /// </summary>
        static void ClearScreen(bool timerEnabled) {
            if (timerEnabled) Thread.Sleep(1500);
            Console.Clear();
        }

    }

}
