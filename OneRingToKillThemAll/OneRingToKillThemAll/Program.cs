using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Media;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing;

namespace Jeu_Combat_Groupe_F
{
    class Program
    {
        static void Main(string[] args)
        {

            #region Declaration and Initialization of variables

            //enable the full screen at the beginning of the game
            Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);

            //1st music plays (ambient music menu)
            SoundPlayer musicPlayer = new SoundPlayer("Adventure Meme.wav");
            musicPlayer.Load();
            musicPlayer.PlayLooping();

            //--------------------Lists and Dictionnary for the different characters (names, PV, ATK, Spe and Spe description), the potions and the Active Burn list for the Gundalph-------------------------------------------------

            //CLASSES
            List<string> listCharAvailable = new List<string>
            {
                "Rawrhirrim",
                "Erwan",
                "Gymlit",
                "Degolas",
                "Gundalph",
                "Striper"
            };

            //HP
            Dictionary<string, int> dicHealthChar = new Dictionary<string, int>
            {
                { listCharAvailable[0], 7 },
                { listCharAvailable[1], 8 },
                { listCharAvailable[2], 10 },
                { listCharAvailable[3], 9 },
                { listCharAvailable[4], 9 },
                { listCharAvailable[5], 8 }
            };

            //BASE ATK
            Dictionary<string, int> dicATKChar = new Dictionary<string, int>
            {
                { listCharAvailable[0], 3 },
                { listCharAvailable[1], 1 },
                { listCharAvailable[2], 2 },
                { listCharAvailable[3], 1 },
                { listCharAvailable[4], 1 },
                { listCharAvailable[5], 2 }
            };

            //SPE NAMES
            Dictionary<string, string> dicSpeChar = new Dictionary<string, string>
            {
                { listCharAvailable[0], "Ring's Fury" },
                { listCharAvailable[1], "Lembus (elvish bread)" },
                { listCharAvailable[2], "Ring of Power" },
                { listCharAvailable[3], "Nuzgûl's curse" },
                { listCharAvailable[4], "BUUUURN" },
                { listCharAvailable[5], "Mines of Muria" }
            };

            //POTIONS
            List<string> listPotions = new List<string>
            {
                "Second Breakfast",
                "Shting (the elven short-sword)"
            };

            //HALF NAMES
            List<string> listHalves = new List<string>
            {
                "Muria",
                "Endwin River",
                "Meowrdor"
            };

            //SPE DESCRIPTIONS
            Dictionary<string, string> dicSpeDescription = new Dictionary<string, string>
            {
                {"Ring's Fury", "returns all daGundalphs suffered during the turn"},
                {"Lembus (elvish bread)", "regain 2 of Health"},
                {"Ring of Power", "lose 1 of Health but gains 1 of ATK for the turn"},
                {"Nuzgûl's curse", "steal one Health for himself from the enemy" },
                {"BUUUURN", "inflict 1 of daGundalph during 3 turn (can be combined)" },
                {"Mines of Muria", "inflict between 0 to 4 of ATK (random)"},

            };

            //for the mage's burn
            List<int> allBurnActive = new List<int>();

            //used to determine the current state of the game, the current turn (for burn) and for the tryCatch
            int half = 1;
            bool isBeginningOfNewHalf = true;
            int countTurn = 1;
            int errorTryCatch = 0;

            //for the Player basics
            string playerName = "";
            int playerChoiceChar = 0;
            int pvPlayer = 0;
            int atkPlayer = 0;
            string spePlayer = "";
            string charPlayer = "";
            //for the potions of the player
            string playerActivePotion = "";
            int playerChoicePotion = 0;
            bool isShting = false;
            //dealing with player's actions
            int playerChoiceAction = 0;
            Tuple<int, int, string> sumActionTurnPlayer = new Tuple<int, int, string>(0, 0, "");

            //for the AI and its cleverness
            List<string> allNameAI = new List<string>()
            {
                "Balgor",
                "Surumen",
                "Soron"
            };
            string charAI = "";
            int pvAI = 10;
            int atkAI = 0;
            string speAI = "";

            Random randomAI = new Random();
            int actionAI = 0;
            int preferedActionAI = 0;
            Tuple<int, int, string> sumActionTurnAI = new Tuple<int, int, string>(0, 0, "");
            int difficultyLevelAI = 0;

            int nbDefPlayerHalf = 0;
            int nbATKPlayerHalf = 0;
            int nbSpePlayerHalf = 0;

            #endregion

//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            
            #region Texts Beginning of the game

            PrintTitle();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("                                                                                                                  Welcome in the Shire, dear adventurer ! I'm Gollurn.\n\n");

            //asking the player his name
            Console.WriteLine("Gollurn -- What is your name ? ");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            playerName = Console.ReadLine();
            Console.ResetColor();
            Console.Write("\n");

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Gollurn -- Nice to meet you, precious !");
            Console.WriteLine("\n");

            #endregion

//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            
            #region Player's Choice of Character

            //print all characters available with their details
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Wait(0.5f);
            Console.WriteLine("1 -- Rawrhirrim : 7 of Health, 3 of ATK, Special is 'Ring's Fury' : returns all daGundalphs suffered during the turn");
            Console.WriteLine("2 -- Erwan : 8 of Health, 1 of ATK, Special is 'Lembus (elvish bread)' : regain 2 of Health");
            Console.WriteLine("3 -- Gymlit : 10 of Health, 2 of ATK, Special is 'Ring of Power' : lose 1 of Health but gains 1 of ATK for the turn");
            Console.WriteLine("4 -- Degolas : 9 of Health, 1 of ATK, Special is 'Nuzgûl's curse' : steal one Health for himself from the enemy");
            Console.WriteLine("5 -- Gundalph : 9 of Health, 1 of ATK, Special is 'BUUUURN' : inflict 1 of daGundalph during 3 turn (can be combined)");
            Console.WriteLine("6 -- Striper : 8 of Health, 2 of ATK, Special is 'Mines of Muria' : inflict between 0 to 4 of ATK (random)\n\n");
            Wait(0.5f);
            Console.ResetColor();

            //asking the player his choice of character
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("Gollurn -- Please choose your destiny : ");
            Console.ResetColor();

            //in order to avoid a wrong character entry (a non-int) from the player
            do
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    errorTryCatch = 0;
                    playerChoiceChar = int.Parse(Console.ReadLine());
                    Console.ResetColor();

                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    errorTryCatch = 1;
                    Console.ResetColor();
                    Console.Write("\n");

                    //Console.WriteLine(ex.Message); //that's to debug
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Gollurn -- Huh, what's that, precious ? Nasty little humansies always say rubbish things. Tell us !");
                    Console.Write("Gollurn -- Please choose your destiny : ");
                    Console.ResetColor();
                }

                //in order to avoid a wrong character entry (an int out of bounds) from the player
                if ((playerChoiceChar < 1 || playerChoiceChar > 6) && errorTryCatch == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    errorTryCatch = 1;
                    Console.ResetColor();

                    Console.Write("\n");

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Gollurn -- You can't be \"nothing\", precious ! *coughs* Tell us !");
                    Console.Write("Gollurn -- Please choose your destiny : ");
                    Console.ResetColor();
                }
            }

            while (errorTryCatch == 1);

            //associate the corresponding character and its details (PV, SPE, ATK)
            charPlayer = ChooseCharacter(playerChoiceChar);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("\nGollurn -- {0}, you now play as {1} !\n\n", playerName, charPlayer);
            Console.ResetColor();
            spePlayer = dicSpeChar[charPlayer];
            pvPlayer = dicHealthChar[charPlayer];
            atkPlayer = dicATKChar[charPlayer];

            #endregion

//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            
            #region Difficulty Level of the AI

            //choice by the player
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Wait(0.5f);
            Console.WriteLine("Level of difficulty :\n");
            Console.WriteLine("1 -- Easy stroll in the Shire");         //pure random
            Console.WriteLine("2 -- You shall not pass !");             //acts accordingly the previous actions of the Player (Puddle Earth) and his character (Meowrdor)
            Console.WriteLine("3 -- RUN, YOU FOOLS");                   //A better version of "You shall not pass!" and the AI has a chance in Meowrdor 
            Wait(0.5f);
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("\nGollurn -- Your choice : ");
            Console.ResetColor();

            //in order to avoid a wrong difficulty entry (a non-int) from the player
            do
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    errorTryCatch = 0;
                    difficultyLevelAI = int.Parse(Console.ReadLine());
                    Console.ResetColor();
                }
                catch (Exception ex)
                {
                    errorTryCatch = 1;
                    //Console.WriteLine(ex.Message); //That is for debug
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Gollurn -- No, no, no... You can't fool us ! We knows you has it, precious. Tell us, tell us the level !\n");
                    Console.Write("Gollurn -- Your choice : ");
                    Console.ResetColor();
                }

                //in order to avoid a wrong difficulty entry (an int out of bounds) from the player
                if ((difficultyLevelAI < 1 || difficultyLevelAI > 3) && errorTryCatch == 0)
                {

                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    errorTryCatch = 1;
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Gollurn -- Fool of a Took ! Choose or walk away, precious !\n");
                    Console.Write("Gollurn -- Your choice : ");
                    Console.ResetColor();
                }
            }
            while (errorTryCatch == 1);
            Wait(0.5f);

            #endregion

//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            
            #region Story texts before Half 1

            //FIRST HISTORY TEXT
            Console.WriteLine("");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("You have been invited to Rivandel, a well known elvish place, to discuss about the futur of Puddle Earth.");
            Console.WriteLine("Indeed, the strongest weapons of our all time ennemy has been found and we now need all to gather &t agree on what should we do next !");
            Console.WriteLine("");
            Wait(0.5f);
            Console.WriteLine("That's why you choose to accept this invitation and see this matter with your own eyes !");
            Console.WriteLine("");
            Wait(0.5f);
            Console.WriteLine("In your journey, you pass through Izengarden, Surumen's homeland.");
            Console.WriteLine("All is destroyed, the young trees and the oldest ones too have peerished...");
            Console.WriteLine("");
            Wait(0.5f);
            Console.WriteLine("The sadness grows inside your heart and now your are sure of Surumen's betrail !");
            Console.WriteLine("You aim to avenge all that have died & travel as fast as your might to let your allies know of the situation. That's how the company was born.");
            Console.WriteLine("");
            Wait(0.5f);
            Console.WriteLine("During the council, it has been shown that we effectively are in possession of The one Ring of Power...");
            Console.WriteLine("... and it has been decided to destroy it, for destroying it will also be the Lord of Darkness's demise.");
            Console.WriteLine("");
            Wait(0.5f);
            Console.WriteLine("You travel very discretly and have found yourself ahead of the \"Mines of Muria\", an ancient dwarf place, now filled with evil creatures of all kinds.");
            Console.WriteLine("After a few days into the mines, one of your friends (a clumsy Took) dropped heavy items on the ground -- which awoke a Balgor...");
            Console.WriteLine("... a very ancient creature of past ages...");
            Console.WriteLine("");
            Wait(0.5f);

            //to let the player read to his own rythm
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("(Press any key to continue...)");
            Console.ResetColor();

            Console.ReadKey();
            Console.WriteLine("");
            Console.Clear();

            PrintTitle();

            #endregion

//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            
            #region Game Loop
            while (pvPlayer > 0 && pvAI > 0)
            {
                #region Texts and choice of AI character at the start of a new half
                if (isBeginningOfNewHalf)
                {

                    PrintHalf(listHalves[half - 1]);
                    Wait(0.5f);

                    //reinitialisation of the burn effect (non-cumulable over the different halves)
                    allBurnActive.Clear();

                    //choice of the AI character
                    Tuple<string, int> choixcharAI = new Tuple<string, int>("", 0);
                    choixcharAI = ChoiceCharAI(difficultyLevelAI, charPlayer, half, nbATKPlayerHalf, nbDefPlayerHalf, nbSpePlayerHalf);
                    charAI = choixcharAI.Item1;
                    preferedActionAI = choixcharAI.Item2;

                    speAI = dicSpeChar[charAI];
                    pvAI = dicHealthChar[charAI];
                    atkAI = dicATKChar[charAI];

                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("Gollurn -- You are going to fight a {0} (SPE : {1}), controled by a {2} (I hope you like things crispy).\n\n", charAI, dicSpeDescription[speAI], allNameAI[half - 1]);
                    Console.ResetColor();
                    #endregion

                    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    
                    #region Halves Music

                    if (half == 1)
                    {
                        SoundPlayer boss = new SoundPlayer("Boss.wav");
                        boss.Load();
                        boss.PlayLooping();

                    }
                    else if (half == 2)
                    {
                        SoundPlayer dungeonBoss = new SoundPlayer("Dungeon Boss.wav");
                        dungeonBoss.Load();
                        dungeonBoss.PlayLooping();


                    }
                    else if (half == 3)
                    {
                        SoundPlayer noiseAttack = new SoundPlayer("Noise Attack.wav");
                        noiseAttack.Load();
                        noiseAttack.PlayLooping();

                        pvPlayer = dicHealthChar[charPlayer];
                    }

                    #endregion

                    //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                    
                    #region Choice of the potion during the second half and Texts of half 2
                    if (half == 2)
                    {
                        Wait(0.5f);
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("Gollurn -- A brilliant victory that was, precious ! But it looks like you just killed the big boss's pet. Quick ! You find some potions on the ground - your pocketsies are full but can still take one (because we said so). Choose wisely !");
                        Console.WriteLine("Gollurn -- Of course, you can keep the potion for later, don't have to use it all if you don't need to just yet. We are no monsters you know !\n");
                        Console.ResetColor();

                        Console.WriteLine("\n");
                        Wait(0.5f);
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("Available potions :\n\n");

                        //print available potions
                        Console.WriteLine("1 -- Second Breakfast : give you back all of your health !\n");
                        Console.WriteLine("2 -- Shting (the elven short-sword) : attack 2 points !\n");
                        Wait(0.5f);
                        Console.ResetColor();

                        //choice of potion
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("Your choice : ");
                        Console.ResetColor();

                        //in order to avoid a wrong potion entry (a non-int) from the player
                        do
                        {
                            try
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                errorTryCatch = 0;
                                playerChoicePotion = int.Parse(Console.ReadLine());
                                Console.ResetColor();
                            }
                            catch (Exception ex)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                errorTryCatch = 1;
                                Console.ResetColor();

                                //console.WriteLine(ex.Message);
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                Console.WriteLine("Gollurn -- AAAAAAH ! *coughs* \n");
                                Console.Write("Your choice : ");
                                Console.ResetColor();
                            }

                            //in order to avoid a wrong potion entry (an int out of bounds) from the player
                            if ((playerChoicePotion < 1 || playerChoicePotion > 2) && errorTryCatch == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                errorTryCatch = 1;
                                Console.ResetColor();

                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                Console.WriteLine("Gollurn -- You..don't want a potion, precious ? Rubbish little humansies again... if you don't choose, we takes it! \n");
                                Console.Write("Your choice : ");
                                Console.ResetColor();
                            }
                        }
                        while (errorTryCatch == 1);

                        //update of the potion
                        playerActivePotion = ChoosePotion(playerChoicePotion);
                        listPotions.Remove(ChoosePotion(playerChoicePotion));

                        //story of half 2
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("");
                        Console.WriteLine("{0} wants to follow the Endwin river but a white magician appears ! :o", playerName);
                        Console.WriteLine("");
                        Wait(0.5f);
                        Console.WriteLine("Surumen : \"You little fool... what were you thinking by doing this quest which don't concern you at all !\"");
                        Console.WriteLine("Surumen : \"Give it to me. It would be wise, my friend.\"");
                        Console.WriteLine("");
                        Wait(0.5f);
                        Console.WriteLine("{0} : \"Never ! I know you have betrayed us to serve your own purpose !\"", playerName);
                        Console.WriteLine("{0} : \"I saw what you have done along your territory, you have gone out of the right way a long time ago now.\"", playerName);
                        Console.WriteLine("{0} : \"Let me do what you'll never be able to do !\"", playerName);
                        Console.WriteLine("");
                        Wait(0.5f);
                        Console.WriteLine("Surumen : \"No one can be against the lord of drakness and his will.\"");
                        Console.WriteLine("Surumen : \"And it's way so fool now I'm with him !\"");
                        Console.WriteLine("");
                        Wait(0.5f);
                        Console.WriteLine("Surumen : \"Now you will taste a little piece of this great power and die !!!\"");
                        Console.WriteLine("");
                        Wait(0.5f);
                        Console.ResetColor();

                        //to let the player read to his own rythm
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write("(Press any key to continue...)");
                        Console.ResetColor();
                    }

                    //used for the damages of Shting
                    if (playerActivePotion == "Shting (the elven short-sword)")
                        isShting = true;

                    isBeginningOfNewHalf = false;
                }
                #endregion

                //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                
                #region Player and AI's details
                //AI's name choice
                string nameAI = allNameAI[half - 1];

                Wait(0.5f);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("    {0} [{1}] |  {2} [{3}]", nameAI, charAI, playerName, charPlayer);
                Console.WriteLine("    {0} PV, {1}  ATK | {2} PV, {3} ATK", pvAI, atkAI, pvPlayer, atkPlayer);
                Console.WriteLine("    SPE : {0} |  SPE : {1}\n\n", speAI, spePlayer);
                Console.Write("\n");

                #endregion

                #region Player and AI's choices of action

                //player choice
                Wait(0.5f);
                Console.WriteLine("Choose an action :");
                Console.WriteLine("1 - Attack the enemy !");
                Console.WriteLine("2 - Defend yourself from evil !");
                Console.ResetColor();
                Wait(0.5f);

                //to avoid the cumulation of over 3 active burn, we restrict the use of the special of Gundalph
                if (charPlayer == "Gundalph" && allBurnActive.Count == 3)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Gollurn -- You're too hot, precious ! Do something else !");
                    Console.ResetColor();

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("3 - {0}", spePlayer);
                    Console.ResetColor();
                }

                //printing the options for potions if it still exists
                if (half >= 2 && playerActivePotion != "null" && playerActivePotion != "empty")
                {
                    Wait(0.5f);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("4 - {0}", playerActivePotion);
                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("\nGollurn - Your choice : ");
                Console.ResetColor();

                //in order to avoid a wrong action entry (a non-int) from the player
                do
                {
                    try
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        errorTryCatch = 0;
                        playerChoiceAction = int.Parse(Console.ReadLine());
                        Console.ResetColor();
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        errorTryCatch = 1;
                        Console.ResetColor();

                        //console.WriteLine(ex.Message);
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("Gollurn -- My precious must be deaf (or not know how to read). Try again ! \n");
                        Console.Write("Gollurn - Your choice : ");
                        Console.ResetColor();
                    }

                    //in order to avoid a wrong action entry (an int out of bounds) from the player
                    if ((playerChoiceAction < 1 || (playerChoiceAction > 3 && (playerActivePotion == "null" || playerActivePotion == "empty")) || playerChoiceAction > 4 || (playerChoiceAction == 4 && half == 1)) && errorTryCatch == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        errorTryCatch = 1;
                        Console.ResetColor();

                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("*coughs* Semwise Gamgeet -- PO-TAY-TOES ! Boil 'em, mash 'em, stick 'em in a stew (can they hear us, precious ?)... Lovely big golden chips with a nice piece of fried fish.");
                        Console.WriteLine("Gollurn -- Keep your nasty little chips & tell us !\n");
                        Console.Write("Gollurn - Your choice : ");
                        Console.ResetColor();
                    }
                }
                while (errorTryCatch == 1);

                //AI choice of action if the difficulty chosen is over 1 and the half over 1
                Wait(0.5f);
                if (preferedActionAI == 1)
                {
                    actionAI = FakeRandom(1);
                }
                else if (preferedActionAI == 2)
                {
                    actionAI = FakeRandom(2);
                }
                else if (preferedActionAI == 3)
                {
                    actionAI = FakeRandom(3);
                }
                else
                {
                    actionAI = randomAI.Next(1, 4);
                }

                Random choixRand = new Random();

                //AI choice of action and character for the third difficulty 
                if (choixRand.Next(0, 4) < 3 && half == 3 && difficultyLevelAI == 3)
                {
                    if (playerChoiceAction == 2 && charPlayer != "Gundalph")
                    {
                        if (charAI == "Gundalph" && allBurnActive.Count != 3)
                            actionAI = 3;
                        else
                            actionAI = 1;
                    }
                    else if (playerChoiceAction == 2 && charPlayer == "Gundalph")
                    {
                        actionAI = 2;
                    }
                    else
                    {
                        switch (charPlayer)
                        {
                            case "Erwan":
                                if (allBurnActive.Count != 3)
                                    actionAI = 3;
                                else
                                    actionAI = 1;
                                break;

                            case "Rawrhirrim":
                                actionAI = 2;
                                break;

                            case "Gymlit":
                                if (playerChoiceAction == 1)
                                    actionAI = 1;
                                else
                                    actionAI = 3;
                                break;

                            case "Degolas":
                                if (playerChoiceAction == 1)
                                    actionAI = 3;
                                else
                                    actionAI = 1;
                                break;

                            case "Gundalph":
                                if (allBurnActive.Count == 3)
                                    actionAI = 3;
                                else
                                    actionAI = 1;
                                break;

                            case "Striper":
                                if (playerChoiceAction == 1)
                                    actionAI = 1;
                                else
                                    actionAI = 3;
                                break;
                        }
                    }

                }

                //for the special attack when AI is a Gundalph (no cumulation over 3)
                if (charAI == "Gundalph" && allBurnActive.Count == 3)
                    actionAI = randomAI.Next(1, 3);

                #endregion

                //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                #region Player and AI's effective actions
                Tuple<int, int, int, int> nbActionPlayer = new Tuple<int, int, int, int>(0, 0, 0, 0);
                Tuple<int, int, int, int> nbActionAI = new Tuple<int, int, int, int>(0, 0, 0, 0);

                //printing the player's action to the screen
                nbActionPlayer = TextActions(playerChoiceAction, playerName, playerActivePotion, charPlayer, allBurnActive);

                //used for cleverness of the AI
                nbATKPlayerHalf += nbActionPlayer.Item1;
                nbDefPlayerHalf += nbActionPlayer.Item2;
                nbSpePlayerHalf += nbActionPlayer.Item3;

                playerChoiceAction = nbActionPlayer.Item4;

                //SFX for the player
                if (playerChoiceAction == 3)
                {

                    var reader = new NAudio.Wave.Mp3FileReader("Epic Special Attack Player.mp3");   //ready for lecture
                    var waveOut = new NAudio.Wave.WaveOutEvent();
                    waveOut.Init(reader);                                                           //init of the reader
                    waveOut.Play();                                                                 //play the sound

                }

                Wait(1);

                //printing the AI's action to the screen
                nbActionAI = TextActions(actionAI, "AI", playerActivePotion, charAI, allBurnActive);
                actionAI = nbActionAI.Item4;

                //SFX for the AI
                if (actionAI == 3)
                {
                    var reader = new NAudio.Wave.Mp3FileReader("IA Hit Impact.mp3");
                    var waveOut = new NAudio.Wave.WaveOutEvent();
                    waveOut.Init(reader);
                    waveOut.Play();

                }

                //will be used to sum every action on the two entities
                int finalImpactOnAI = 0;
                int finalImpactOnPlayer = 0;

                //holds first the impact of the Player's action on himself and second on the AI
                sumActionTurnPlayer = actionPlayer(playerChoiceAction, charPlayer, playerActivePotion, dicATKChar);
                //holds first the impact of AI's action on itself and second on the Player
                sumActionTurnAI = actionPlayer(actionAI, charAI, playerActivePotion, dicATKChar);

                //we recover the state of the potion at the 2nd and 3rd half
                if (half >= 2)
                {
                    playerActivePotion = sumActionTurnPlayer.Item3;
                }

                //the final impact on each entity
                finalImpactOnPlayer += sumActionTurnPlayer.Item1 + sumActionTurnAI.Item2;
                finalImpactOnAI += sumActionTurnPlayer.Item2 + sumActionTurnAI.Item1;

                #endregion

                //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                #region Defense vs Ring of Power
                //when Ring of Power is used, it can pierce through defenses
                if ((playerChoiceAction == 2) && (charAI == "Gymlit" && actionAI == 3))
                {
                    finalImpactOnPlayer += 1;
                }
                if ((actionAI == 2) && (charPlayer == "Gymlit" && playerChoiceAction == 3))
                {
                    finalImpactOnAI += 1;
                }

                #endregion

                //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                #region BUUUURN

                //we remove the oldest burn (if its 3 turns is over)
                if ((allBurnActive.Count >= 1) && (allBurnActive[0] + 3 == countTurn))
                    allBurnActive.RemoveAt(0);

                //we add the damages to the burned entity and add new burns (if wanted)
                if (charAI == "Gundalph")
                {

                    if (actionAI == 3)
                        allBurnActive.Add(countTurn);
                    finalImpactOnPlayer -= allBurnActive.Count;

                }
                if (charPlayer == "Gundalph")
                {

                    if (playerChoiceAction == 3 && allBurnActive.Count != 3)
                        allBurnActive.Add(countTurn);
                    finalImpactOnAI -= allBurnActive.Count;
                }

                #endregion

                //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                #region Ring's Fury

                //if Ring's Fury is used, the entity returns all damages to the ennemy
                if ((playerChoiceAction == 3 && charPlayer == "Rawrhirrim"))
                {
                    finalImpactOnAI += sumActionTurnAI.Item2;
                    finalImpactOnAI -= allBurnActive.Count;
                }
                if ((actionAI == 3 && charAI == "Rawrhirrim"))
                {
                    finalImpactOnPlayer += sumActionTurnPlayer.Item2;
                    finalImpactOnPlayer -= allBurnActive.Count;
                }


                if (playerChoiceAction == 2 && actionAI != 3)
                    finalImpactOnPlayer = 0;
                if (actionAI == 2 && playerChoiceAction != 3)
                    finalImpactOnAI = 0;

                #endregion

                //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                #region Defense (except if Ring of Power is used)

                if ((playerChoiceAction == 2) && (actionAI == 3) && (charAI != "Gymlit"))
                    finalImpactOnPlayer = 0;
                if ((actionAI == 2) && (playerChoiceAction == 3) && (charPlayer != "Gymlit"))
                    finalImpactOnAI = 0;

                //if shting is used, we pierce through the defense
                if (playerChoiceAction == 4 && isShting)
                {
                    finalImpactOnAI -= 2;
                    isShting = false;
                }

                #endregion

                //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                //we update the healths
                pvAI += finalImpactOnAI - 5; //add -10 to cheat
                pvPlayer += finalImpactOnPlayer;

                //without going further than the limit set at the beginning
                if (pvAI > dicHealthChar[charAI])
                    pvAI = dicHealthChar[charAI];
                if (pvPlayer > dicHealthChar[charPlayer])
                    pvPlayer = dicHealthChar[charPlayer];

                countTurn++;

                //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

                #region Possible ending
                //to avoid displaying negative values
                if (pvPlayer < 0)
                    pvPlayer = 0;
                if (pvAI < 0)
                    pvAI = 0;

                //if one of the health is down to 0, we print the final messages 
                if (pvAI * pvPlayer == 0)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Wait(0.5f);
                    Console.WriteLine("    {0} [{1}]                            {2} [{3}]", nameAI, charAI, playerName, charPlayer);
                    Console.WriteLine("    {0} PV, {1} ATK                     {2} PV, {3} ATK\n\n", pvAI, atkAI, pvPlayer, atkPlayer);
                    Wait(0.5f);
                    Console.ResetColor();

                    Tuple<int, bool> tupleEnd = new Tuple<int, bool>(0, false);
                    tupleEnd = EndGame(pvPlayer, pvAI, half, args);

                    half = tupleEnd.Item1;
                    isBeginningOfNewHalf = tupleEnd.Item2;

                    //if the player won and wants/can proceed to the next half
                    if (isBeginningOfNewHalf)
                        pvAI = 20;

                }
                #endregion
            }
            #endregion

//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            
            #region Functions

            //return the impact of the entity on itself and on the other entity
            static Tuple<int, int, string> actionPlayer(int action, string player, string potion, Dictionary<string, int> dicATKChar)
            {
                int impactOnMe = 0;
                int impactOnHim = 0;
                int errorTryCatch = 0;

                //if a potion is used 
                if (action == 4 && potion != "empty" && potion != "null")
                {
                    if (potion == "Second Breakfast")
                        impactOnMe += 10;
                    potion = "empty";
                }
                //if the player wants to choose the potion but it has already been used
                else if (action == 4 && potion == "empty")
                {
                    do
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("Gollurn - The potion is empty !");
                        Console.ResetColor();

                        try
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            errorTryCatch = 0;
                            action = int.Parse(Console.ReadLine());
                            Console.ResetColor();
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            errorTryCatch = 1;
                            Console.ResetColor();

                            //Console.WriteLine(ex.Message);
                            Console.Write("Gollurn -- Pick a number precious, not a letter... ");
                        }
                        if ((action < 1 || action > 3) && errorTryCatch == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            errorTryCatch = 1;
                            Console.ResetColor();

                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Gollurn -- Your potion is empty, just choose something else, precious ! *coughs*");
                            Console.ResetColor();
                        }
                    }
                    while (errorTryCatch == 1);
                    actionPlayer(action, player, potion, dicATKChar);
                }
                //if the player wants to choose the potion but it hasn't been unlocked
                else if (action == 4 && potion == "null")
                {
                    do
                    {
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("Gollurn - the potion is empty");
                        Console.ResetColor();

                        try
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            errorTryCatch = 0;
                            action = int.Parse(Console.ReadLine());
                            Console.ResetColor();
                        }
                        catch (Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            errorTryCatch = 1;
                            Console.ResetColor();

                            //Console.WriteLine(ex.Message);
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write("Gollurn -- Pick a number precious, not a letter... ");
                            Console.ResetColor();
                        }
                        if ((action < 1 || action > 3) && errorTryCatch == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            errorTryCatch = 1;
                            Console.ResetColor();

                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("Gollurn -- Your potion is empty, just choose something else, precious ! *coughs*");
                            Console.ResetColor();
                        }
                    }
                    while (errorTryCatch == 1);
                    actionPlayer(action, player, potion, dicATKChar);
                }
                else
                {
                    //all effects of the actions according to the character
                    switch (player)
                    {
                        case "Rawrhirrim":
                            switch (action)
                            {
                                case 1:
                                    impactOnHim -= dicATKChar[player];
                                    break;
                            }
                            break;

                        case "Erwan":
                            switch (action)
                            {
                                case 1:
                                    impactOnHim -= dicATKChar[player];
                                    break;
                                case 3:
                                    impactOnMe += 2;
                                    break;
                            }
                            break;

                        case "Gymlit":
                            switch (action)
                            {
                                case 1:
                                    impactOnHim -= dicATKChar[player];
                                    break;

                                case 3:
                                    impactOnMe -= 1;
                                    impactOnHim -= dicATKChar[player] + 1;
                                    break;
                            }
                            break;

                        case "Degolas":
                            switch (action)
                            {
                                case 1:
                                    impactOnHim -= dicATKChar[player];
                                    break;

                                case 3:
                                    impactOnMe += 1;
                                    impactOnHim -= 1;
                                    break;
                            }
                            break;

                        case "Gundalph":
                            switch (action)
                            {
                                case 1:
                                    impactOnHim -= dicATKChar[player];
                                    break;
                            }
                            break;

                        case "Striper":
                            switch (action)
                            {
                                case 1:
                                    impactOnHim -= dicATKChar[player];
                                    break;

                                case 3:
                                    Random critique = new Random();
                                    impactOnHim -= critique.Next(0, 5);
                                    break;
                            }
                            break;
                    }
                }
                return new Tuple<int, int, string>(impactOnMe, impactOnHim, potion);
            }

            //Print the texts for each action
            static Tuple<int, int, int, int> TextActions(int choix, string player, string playerChar, string potionPerso, List<int> allBurnActive)
            {

                int nbAttaque = 0;
                int nbDefense = 0;
                int nbSpecial = 0;
                switch (choix)
                {
                    case 1: //attack
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("{0} : ATTAQUE !\n", player);
                        if (player != "AI")
                            nbAttaque++;
                        Console.ResetColor();
                        break;

                    case 2: //defense
                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine("{0} : DEFENSE\n", player);
                        if (player != "AI")
                            nbDefense++;
                        Console.ResetColor();
                        break;

                    case 3: //special
                        if (playerChar == "Gundalph" && allBurnActive.Count == 3 && player != "AI")
                        {

                            int errorTryCatch = 0;

                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.Write("Gollurn -- IT BURNS AAAH ! Choose something else, precious !");
                            Console.ResetColor();

                            do
                            {
                                try
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    errorTryCatch = 0;
                                    choix = int.Parse(Console.ReadLine());
                                    Console.ResetColor();
                                }
                                catch (Exception ex)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    errorTryCatch = 1;
                                    Console.ResetColor();

                                    //Console.WriteLine(ex.Message);
                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                    Console.Write("Gollurn -- Really, precious ? What is it ?");
                                    Console.ResetColor();
                                }
                                if ((choix < 1 || choix > 2) && errorTryCatch == 0)
                                {
                                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                                    errorTryCatch = 1;
                                    Console.ResetColor();

                                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                                    Console.WriteLine("Gollurn -- Give it to us raw, and wriggling !");
                                    Console.ResetColor();

                                }
                            } while (errorTryCatch == 1);
                            TextActions(choix, player, playerChar, potionPerso, allBurnActive);
                        }
                        else if (playerChar == "Gundalph" && allBurnActive.Count == 3 && player == "AI")
                        {
                            Random randChoixAI = new Random();
                            choix = randChoixAI.Next(1, 3);
                            TextActions(choix, player, playerChar, potionPerso, allBurnActive);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("\n{0} : SPECIAL ATTACK\n", player);
                            if (player != "AI")
                                nbSpecial++;
                            Console.ResetColor();
                        }

                        break;

                    case 4: //potion
                        if (player != "AI" && (potionPerso != "empty" && potionPerso != "null"))
                        {
                            Console.ForegroundColor = ConsoleColor.Magenta;
                            Console.WriteLine("{0} : -- POWER UP POTION --\n", player);

                            //SFX for the power up potion
                            var reader = new NAudio.Wave.Mp3FileReader("Power Up Potions.mp3");
                            var waveOut = new NAudio.Wave.WaveOutEvent();
                            waveOut.Init(reader);
                            waveOut.Play();
                            Console.ResetColor();
                        }
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("Gollurn -- You need to choose a valid number, precious ! \n");
                        Console.ResetColor();
                        TextActions(choix, player, playerChar, potionPerso, allBurnActive);
                        break;
                }
                return new Tuple<int, int, int, int>(nbAttaque, nbDefense, nbSpecial, choix);
            }

            //pause the game for n-seconds (not using Sleep())
            static void Wait(float second)
            {
                Task delay = Task.Delay(TimeSpan.FromSeconds(second));
                delay.Wait();
            }

            //return the character given the choice (int)
            static string ChooseCharacter(int choix)
            {
                string player = "";

                switch (choix)
                {
                    case 1:
                        player = "Rawrhirrim";
                        break;

                    case 2:
                        player = "Erwan";
                        break;

                    case 3:
                        player = "Gymlit";
                        break;

                    case 4:
                        player = "Degolas";
                        break;

                    case 5:
                        player = "Gundalph";
                        break;

                    case 6:
                        player = "Striper";
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("Gollurn -- You need to choose a valid number, precious ! \n");
                        Console.ResetColor();
                        ChooseCharacter(choix);
                        break;
                }

                return player;
            }

            //return the biggest number out of three
            static int BestOutOfThree(int nb1, int nb2, int nb3)
            {
                if (nb1 > nb2)
                {
                    if (nb1 > nb3)
                        return nb1;
                    else
                        return nb3;
                }
                else
                {
                    if (nb2 > nb3)
                        return nb2;
                    else
                        return nb3;
                }
            }

            //return the character of the AI and its prefered action due to its cleverness
            //the first difficulty returns a random choice and no prefered action
            //the second difficulty returns a choice based on the previous actions of the player (and chooses the second best character to counter the player)
            //the third difficulty returns a choice based on the previous actions of the player (and chooses the best character to counter the player)
            static Tuple<string, int> ChoiceCharAI(int difficultyLevelAI, string charPlayer,
            int half, int nbATKPlayerHalf, int nbDefPlayerHalf,
                int nbSpePlayerHalf)
            {
                string charAI = "";
                int preferedActionAI = 0;
                Random randomAI = new Random();

                do
                {
                    int choixAI = randomAI.Next(1, 7);
                    if (difficultyLevelAI == 2 && half != 1)
                    {
                        if (half == 2)
                        {
                            if (BestOutOfThree(nbATKPlayerHalf, nbDefPlayerHalf, nbSpePlayerHalf) == nbDefPlayerHalf && charPlayer != "Gymlit")
                            {
                                charAI = "Gymlit";
                                preferedActionAI = 3;
                            }
                            else if (BestOutOfThree(nbATKPlayerHalf, nbDefPlayerHalf, nbSpePlayerHalf) == nbATKPlayerHalf && charPlayer != "Gundalph")
                            {
                                charAI = "Gundalph";
                                preferedActionAI = 3;
                            }
                            else if (BestOutOfThree(nbATKPlayerHalf, nbDefPlayerHalf, nbSpePlayerHalf) == nbSpePlayerHalf && charPlayer != "Striper")
                            {
                                charAI = "Striper";
                                preferedActionAI = 3;
                            }
                            else if (BestOutOfThree(nbATKPlayerHalf, nbDefPlayerHalf, nbSpePlayerHalf) == nbSpePlayerHalf && charPlayer == "Striper")
                            {
                                charAI = "Gundalph";
                                preferedActionAI = 3;
                            }
                            else
                            {
                                charAI = ChooseCharacter(choixAI);
                                preferedActionAI = 3;
                            }
                        }
                        else if (half == 3)
                        {
                            if (BestOutOfThree(nbATKPlayerHalf, nbDefPlayerHalf, nbSpePlayerHalf) == nbDefPlayerHalf)
                            {
                                preferedActionAI = 3;
                            }
                            else if (BestOutOfThree(nbATKPlayerHalf, nbDefPlayerHalf, nbSpePlayerHalf) == nbATKPlayerHalf)
                            {
                                preferedActionAI = 2;
                            }
                            else if (BestOutOfThree(nbATKPlayerHalf, nbDefPlayerHalf, nbSpePlayerHalf) == nbSpePlayerHalf)
                            {
                                preferedActionAI = 3;
                            }

                            switch (charPlayer)
                            {
                                case "Erwan":
                                    charAI = "Degolas";
                                    break;

                                case "Rawrhirrim":
                                    charAI = "Gymlit";
                                    break;

                                case "Gymlit":
                                    charAI = "Erwan";
                                    break;

                                case "Degolas":
                                    charAI = "Striper";
                                    break;

                                case "Gundalph":
                                    charAI = "Rawrhirrim";
                                    break;

                                case "Striper":
                                    charAI = "Gundalph";
                                    break;
                            }
                        }
                    }
                    else if (difficultyLevelAI == 3 && half != 1)
                    {
                        if (half == 2)
                        {
                            switch (charPlayer)
                            {
                                case "Erwan":
                                    charAI = "Degolas";
                                    break;

                                case "Rawrhirrim":
                                    charAI = "Gymlit";
                                    break;

                                case "Gymlit":
                                    charAI = "Erwan";
                                    break;

                                case "Degolas":
                                    charAI = "Striper";
                                    break;

                                case "Gundalph":
                                    charAI = "Rawrhirrim";
                                    break;

                                case "Striper":
                                    charAI = "Gundalph";
                                    break;
                            }
                        }
                        else if (half == 3)
                        {
                            if (BestOutOfThree(nbATKPlayerHalf, nbDefPlayerHalf, nbSpePlayerHalf) == nbDefPlayerHalf)
                            {
                                preferedActionAI = 3;
                            }
                            else if (BestOutOfThree(nbATKPlayerHalf, nbDefPlayerHalf, nbSpePlayerHalf) == nbATKPlayerHalf)
                            {
                                preferedActionAI = 2;
                            }
                            else if (BestOutOfThree(nbATKPlayerHalf, nbDefPlayerHalf, nbSpePlayerHalf) == nbSpePlayerHalf)
                            {
                                preferedActionAI = 3;
                            }

                            switch (charPlayer)
                            {
                                case "Erwan":
                                    charAI = "Gundalph";
                                    break;

                                case "Rawrhirrim":
                                    charAI = "Gundalph";
                                    break;

                                case "Gymlit":
                                    charAI = "Rawrhirrim";
                                    break;

                                case "Degolas":
                                    charAI = "Striper";
                                    break;

                                case "Gundalph":
                                    charAI = "Rawrhirrim";
                                    break;

                                case "Striper":
                                    charAI = "Rawrhirrim";
                                    break;
                            }
                        }
                        else
                        {
                            charAI = ChooseCharacter(choixAI);
                        }
                    }
                    else
                    {
                        charAI = ChooseCharacter(choixAI);
                        preferedActionAI = 0;
                    }
                }
                while (charAI == charPlayer || (charAI == "Erwan" && charPlayer == "Degolas") || (charPlayer == "Erwan" && charAI == "Degolas"));

                return new Tuple<string, int>(charAI, preferedActionAI);
            }

            //Return the potion chosen by the Player
            static string ChoosePotion(int choix)
            {
                string potion = "";

                switch (choix)
                {
                    case 1:
                        potion = "Second Breakfast";
                        break;

                    case 2:
                        potion = "Shting (the elven short-sword)";
                        break;

                    default:
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.WriteLine("Gollurn -- You need to choose a valid number, precious ! \n");
                        Console.ResetColor();
                        ChoosePotion(choix);
                        break;
                }

                return potion;
            }

            //return a number knowing that one must have biggest probabilities to appear
            static int FakeRandom(int numberToFake)
            {
                int retour = 0;
                Random randomAI = new Random();
                int choix = randomAI.Next(1, 6);

                if (choix == 4 || choix == 5)
                    retour = numberToFake;
                else
                    retour = choix;

                return retour;
            }

            //Print the visual for the halves
            static void PrintHalf(string half)
            {
                Wait(0.5f);
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("         +----------------------+");
                Console.WriteLine("                 {0}", half);
                Console.WriteLine("         +----------------------+\n\n");
                Console.ResetColor();
            }

            //Print the main title
            static void PrintTitle()
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("\n");
                Console.WriteLine("                                       ██████  ███    ██ ███████     ██████  ██ ███    ██  ██████      ████████  ██████      ██   ██ ██ ██      ██          ████████ ██   ██ ███████ ███    ███      █████  ██      ██      ");
                Console.WriteLine("                                      ██    ██ ████   ██ ██          ██   ██ ██ ████   ██ ██              ██    ██    ██     ██  ██  ██ ██      ██             ██    ██   ██ ██      ████  ████     ██   ██ ██      ██      ");
                Console.WriteLine("                                      ██    ██ ██ ██  ██ █████       ██████  ██ ██ ██  ██ ██   ███        ██    ██    ██     █████   ██ ██      ██             ██    ███████ █████   ██ ████ ██     ███████ ██      ██      ");
                Console.WriteLine("                                      ██    ██ ██  ██ ██ ██          ██   ██ ██ ██  ██ ██ ██    ██        ██    ██    ██     ██  ██  ██ ██      ██             ██    ██   ██ ██      ██  ██  ██     ██   ██ ██      ██      ");
                Console.WriteLine("                                       ██████  ██   ████ ███████     ██   ██ ██ ██   ████  ██████         ██     ██████      ██   ██ ██ ███████ ███████        ██    ██   ██ ███████ ██      ██     ██   ██ ███████ ███████ ");
                Console.WriteLine("\n");
                Console.ResetColor();
                Console.Write("\n\n");
            }

            //Print the endgame's texts
            static Tuple<int, bool> EndGame(int pvPlayer, int pvAI, int half, string[] args)
            {

                bool isOkayToProceedNextWave = false;

                if (pvPlayer <= 0 && pvAI > 0)
                {

                    SoundPlayer musicPlayer = new SoundPlayer("Lost Time.wav");
                    musicPlayer.Load();
                    musicPlayer.PlayLooping();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Gollurn -- NooOoo ! You has lost it, precious ! *coughs*\n");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("(Press any key to continue...)");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.WriteLine("");
                    Console.Clear();

                    //CREDITS
                    Credits();
                    Console.ReadKey();
                    Console.Clear();
                    PlayAgain(args);

                }
                else if (pvAI <= 0 && pvPlayer > 0 && half < 3)
                {

                    //SHOWING TEXT
                    if (half == 1) //to the 2nd half
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Now you are out of the Muria. You are tired, but the way still is so long...");
                        Console.WriteLine("You cross the Latlarien forest, where elves help you to rest.");
                        Console.WriteLine("");
                        Wait(0.5f);
                        Console.WriteLine("Before continuing your journey, they offer you a gift:");
                        Console.WriteLine("a potion to help you when you think that all hope is gone.");
                        Console.WriteLine("");
                        Wait(0.5f);
                        Console.ResetColor();
                    }
                    else if (half == 2) //to the last half
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("");
                        Console.WriteLine("Surumen is defeated. He doesn't have any power now.");
                        Console.WriteLine("So you let him behind you and continue your way along the river.");
                        Console.WriteLine("");
                        Wait(0.5f);
                        Console.WriteLine("After a litle forest, you arrive to the foggy mountains.");
                        Console.WriteLine("You follow Gollurn accross this labyrinth and find the dead's swamp.");
                        Console.WriteLine("It's stinking moldy corps and the pressure of the ring makes you feeling like followwing the deads ...");
                        Console.WriteLine("");
                        Wait(0.5f);
                        Console.WriteLine("...but you resist and pass this.");
                        Console.WriteLine("");
                        Wait(0.5f);
                        Console.WriteLine("This is harder and harder to continue because the closer your are to the Meowrdur, the heavier is the ring...");
                        Console.WriteLine("");
                        Wait(0.5f);
                        Console.WriteLine("You are now very close to the Meowrdor so you decide to rest before the last fight.");
                        Console.WriteLine("");
                        Wait(0.5f);
                        Console.ResetColor();
                    }

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("\n(Press any key to continue...)\n");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.WriteLine("");
                    Console.Clear();

                    PrintTitle();

                    half++;
                    isOkayToProceedNextWave = true;
                    Console.ResetColor();

                }
                else if (pvAI <= 0 && pvPlayer <= 0)
                {

                    SoundPlayer musicPlayer = new SoundPlayer("Lost Time.wav");
                    musicPlayer.Load();
                    musicPlayer.PlayLooping();

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Gollurn -- They both died, isn't it, my sweet ? Now what can we do for the precious ?...\n");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("(Press any key to continue...)");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.WriteLine("");
                    Console.Clear();

                    //CREDITS
                    Credits();
                    Console.ReadKey();

                    Console.Clear();
                    PlayAgain(args);
                }
                else if (pvAI <= 0 && pvPlayer > 0 && half == 3)
                {

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("(Press any key to continue...)");
                    Console.ResetColor();

                    Console.ReadKey();
                    Console.WriteLine("");
                    Console.Clear();

                    SoundPlayer musicPlayer = new SoundPlayer("Epic Unease.wav");
                    musicPlayer.Load();
                    musicPlayer.PlayLooping();

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\n");
                    Console.Write("\n");
                    Console.Write("\n");
                    Console.WriteLine("                                                                                  ██    ██ ██  ██████ ████████  ██████  ██████  ██    ██ ");
                    Console.WriteLine("                                                                                  ██    ██ ██ ██         ██    ██    ██ ██   ██  ██  ██  ");
                    Console.WriteLine("                                                                                  ██    ██ ██ ██         ██    ██    ██ ██████    ████   ");
                    Console.WriteLine("                                                                                   ██  ██  ██ ██         ██    ██    ██ ██   ██    ██    ");
                    Console.WriteLine("                                                                                    ████   ██  ██████    ██     ██████  ██   ██    ██    ");
                    Console.ResetColor();

                    //TEXT IF THE PLAYER WINS EVERYTHING

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("");
                    Console.WriteLine("");
                    Wait(1);
                    Console.WriteLine("WHOUUUAAAA you win the fight against the great lord !!! Congratulations !!!");
                    Wait(0.5f);
                    Console.WriteLine("Now you run as fast as possible to the heart of the Mount Doom to destroy the precious !");
                    Wait(0.5f);
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("(Press any key to continue...)");
                    Console.ResetColor();
                    Console.ReadKey();
                    Console.WriteLine("");
                    Console.Clear();

                    //CREDITS
                    Credits();
                    Console.ReadKey();
                    Console.Clear();
                    PlayAgain(args);

                }
                return new Tuple<int, bool>(half, isOkayToProceedNextWave);
            }

            //restart the game if the player wants
            static void PlayAgain(string[] args)
            {
                string playAgain;
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Gollurn -- Would you like to go back to the Shire or leave Puddle Earth ? (yes/no)");
                Console.ResetColor();
                playAgain = Console.ReadLine();
                Console.Write("\n");

                Console.ForegroundColor = ConsoleColor.Cyan;
                if (playAgain == "yes")
                {
                    Console.Clear();
                    Main(args);
                }
                else if (playAgain == "no")
                {
                    Environment.Exit(0);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Gollurn -- No precious ! Not like that ! Please enter again. *coughs*");
                    Console.ResetColor();
                    PlayAgain(args);
                }
            }

            //Print the credits
            static void Credits()
            {

                //CREDITS
                Console.Write("\n");
                Console.Write("\n");
                Console.Write("\n");
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine("                                                                                   ██████ ██████  ███████ ██████  ██ ████████ ███████ ");
                Console.WriteLine("                                                                                  ██      ██   ██ ██      ██   ██ ██    ██    ██      ");
                Console.WriteLine("                                                                                  ██      ██████  █████   ██   ██ ██    ██    ███████ ");
                Console.WriteLine("                                                                                  ██      ██   ██ ██      ██   ██ ██    ██         ██ ");
                Console.WriteLine("                                                                                   ██████ ██   ██ ███████ ██████  ██    ██    ███████ \n\n");
                Console.ResetColor();

                Console.WriteLine("                                                                                  Lead Gameplay Programmer : Enki Bachelier");
                Console.WriteLine("                                                                                  Gameplay Programmers : Thibault Vincent & Diane Aveillan");
                Console.WriteLine("                                                                                  Sound Designer : Diane Aveillan");
                Console.WriteLine("                                                                                  Narrative Designers : Thibault Vincent & Diane Aveillan");

                Console.WriteLine("                                                                                  QA Testers (bug fixes) by :");
                Console.WriteLine("                                                                                  Enki Bachelier | Thibault Vincent | Diane Aveillan");

                Console.WriteLine("                                                                                  SOUND :\n");

                Console.WriteLine("                                                                                  I/ SOUND BACKGROUND");

                Console.WriteLine("                                                                                  \"Adventure Meme\"" + " Kevin MacLeod - incompetech.com");
                Console.WriteLine("                                                                                  \"Noise Attack\" Kevin MacLeod - incompetech.com");
                Console.WriteLine("                                                                                  \"8bit Dungeon Boss\" Kevin MacLeod(incompetech.com");
                Console.WriteLine("                                                                                  \"Video Dungeon Boss\" Kevin MacLeod(incompetech.com");
                Console.WriteLine("                                                                                  Licensed under Creative Commons: By Attribution 4.0 License");
                Console.WriteLine("                                                                                  http://creativecommons.org/licenses/by/4.0/");

                Console.WriteLine("                                                                                  II/ SFX");

                Console.WriteLine("                                                                                  https://freesound.org/people/ethanchase7744/sounds/439538/");
                Console.WriteLine("                                                                                  https://freesound.org/people/Eponn/sounds/547042/");
                Console.WriteLine("                                                                                  https://freesound.org/people/Cloud-10/sounds/647977/");
                
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("                                                                                      (Press any key to continue...)");
                Console.ResetColor();
                Console.Write("\n");
            }

            #endregion

        }
    }
}
