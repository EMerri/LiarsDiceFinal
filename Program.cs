
namespace LiarsDice
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Liar's Dice!");

            bool repeat = true;
            while (repeat)
            {
                Console.WriteLine("Would you either like to: \n 1. Play a new game \n 2. Read the instructions \n 3. Exit \n Please enter your choice (1-3)");
                int Selection = Convert.ToInt32(Console.ReadLine());
                if (Selection == 1)
                {
                    Console.WriteLine("Please enter your name:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Welcome " + name);
                    Player player = new Player(name);
                    Player computer = new Player("Computer");
                    Player currentPlayer = null;
                    Console.WriteLine("How many rounds would you like to play?");
                    int Rounds = Convert.ToInt32(Console.ReadLine());
                    int ComputersPoints = 0;
                    int PlayersPoints = 0;


                    for (int i = 0; i < Rounds; i++)
                    {
                        Console.WriteLine("To begin the game, roll one dice to see who is going to go first: ");
                        Dice PlayersDice = new Dice(6);
                        PlayersDice.Roll();
                        Console.WriteLine(name + " rolls a " + PlayersDice.GetValue());
                        Dice ComputersDice = new Dice(6);
                        ComputersDice.Roll();
                        Console.WriteLine("the computer rolls a " + ComputersDice.GetValue());

                        bool equal = true;

                        while (equal)
                        {
                            if (PlayersDice.GetValue() > ComputersDice.GetValue())
                            {
                                currentPlayer = player;
                                Console.WriteLine(name + " is starting the game");
                                equal = false;
                            }
                            else if (PlayersDice.GetValue() < ComputersDice.GetValue())
                            {
                                currentPlayer = computer;
                                Console.WriteLine("The computer is starting the game");
                                equal = false;
                            }
                            else if (PlayersDice.GetValue() == ComputersDice.GetValue())
                            {
                                Console.WriteLine("It looks liek you rolled the same number! \n please roll again:");
                                PlayersDice.Roll();
                                Console.WriteLine(name + " rolls a " + PlayersDice.GetValue());
                                ComputersDice.Roll();
                                Console.WriteLine("the computer rolls a " + ComputersDice.GetValue());

                            }



                        }



                        Cup playersCup = new Cup(6);
                        Cup computersCup = new Cup(6);


                        playersCup.RollDice();
                        computersCup.RollDice();



                        Console.WriteLine("The dice you have rolled are:");

                        foreach (var die in playersCup.GetDiceValues())
                        {
                            Console.WriteLine(die);
                        }



                        //Player currentPlayer = player;
                        bool challenge = false;
                        int lastBidValue = 0;
                        int lastBidQuantity = 0;

                        while (true)
                        {
                            Console.WriteLine(currentPlayer.Name + "'s turn");



                            int choice;
                            if (currentPlayer == player)
                            {
                                Console.WriteLine("Current bid: Quantity  = " + lastBidQuantity + " Value = " + lastBidValue);
                                Console.WriteLine("1. Make a bid");
                                Console.WriteLine("2. Challenge");
                                choice = GetInput("Enter your choice:");
                            }
                            else
                            {
                                double Probability = CalculateProbabilityOfLiar(computersCup, lastBidValue, lastBidQuantity);

                                if (Probability > 0.5)
                                {
                                    challenge = true;
                                    choice = 2;
                                }
                                else
                                {
                                    Console.WriteLine(Probability + "\n");

                                    (int value, int quantity) = CalculateComputerBid(computersCup, lastBidValue, lastBidQuantity);
                                    Console.WriteLine($"Computer bids: {quantity} {value}");
                                    choice = 1;
                                    lastBidValue = value;
                                    lastBidQuantity = quantity;
                                }

                           
                            }

                            if (choice == 1)
                            {
                                if (currentPlayer == player)
                                {
                                    int bidValue = GetInput("Enter the face value for your bid:");
                                    int bidQuantity = GetInput("Enter the quantity for your bid:");

                                    if (bidValue > lastBidValue || (bidValue == lastBidValue && bidQuantity > lastBidQuantity))
                                    {
                                        lastBidValue = bidValue;
                                        lastBidQuantity = bidQuantity;
                                        challenge = false;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid bid. Try again.");
                                        continue;
                                    }
                                }
                            }
                            else if (choice == 2)
                            {
                                challenge = true;
                            }
                            else
                            {
                                Console.WriteLine("Invalid choice. Try again.");
                                continue;
                            }

                            // Switch player
                            if (currentPlayer == player)
                                currentPlayer = computer;
                            else
                                currentPlayer = player;

                            if (challenge)
                            {
                                if (currentPlayer == player)
                                    currentPlayer = computer;
                                else
                                    currentPlayer = player;
                                Console.WriteLine($"{currentPlayer.Name} challenges!");

                                if (computersCup.CountDiceWithValue(lastBidValue) + playersCup.CountDiceWithValue(lastBidValue) >= lastBidQuantity)
                                {
                                    if (currentPlayer == player)
                                        Console.WriteLine($"{currentPlayer.Name} loses the challenge!");
                                    else
                                        Console.WriteLine($"{currentPlayer.Name} loses the challenge!");
                                    break;
                                }
                                else
                                {
                                    if (currentPlayer == player)
                                        Console.WriteLine($"{currentPlayer.Name} wins the challenge!");
                                    else
                                        Console.WriteLine($"{currentPlayer.Name} wins the challenge!");
                                    break;
                                    PlayersPoints++;
                                }
                            }
                        }

                        Console.WriteLine("Game Over.");
                    }




                }
                else if (Selection == 2)
                {
                    Console.WriteLine("Each round, every player rolls a cup/hand with 5 dice, keeping this hidden from the other players. \r\n\r\nThe first player begins by bidding a value and the minimum amount they believe it to be \r\n\r\nTurns rotate among the players in order.  \r\n\r\nEach player has two choices during their turn: to make a higher bid or challenge the previous bid-typically with a call of \"liar\". \r\n\r\nIf they choose to bid then: the player may bid a higher quantity of any particular face, or the same quantity of a higher face. \r\n\r\nIf the current player challenges the previous bid, all dice are revealed. If the bid is valid (at least as many of the face values are showing as were bid), the bidder wins. Otherwise, the challenger wins. ");

                }
                else if (Selection == 3)
                {
                    Console.WriteLine("Now Exiting, Thank you for playing!");
                    repeat = false;
                }
                else
                {
                    Console.WriteLine("Invalid selection please try again:");

                }
            }
        }





        static int GetInput(string message)
        {
            Console.WriteLine(message);
            int input;
            while (!int.TryParse(Console.ReadLine(), out input))
            {
                Console.WriteLine("Invalid input. Please enter an integer value:");
            }
            return input;
        }

        static (int, int) CalculateComputerBid(Cup cup, int lastBidValue, int lastBidQuantity)
        {
            if (lastBidQuantity < 6)
            {
                lastBidQuantity++;
                lastBidValue = lastBidValue;
            }
            else if (lastBidQuantity > 6)
            {
                lastBidValue++;
                lastBidQuantity = 1;
            }
            return (lastBidValue, lastBidQuantity);
        }
        static double CalculateProbabilityOfLiar(Cup cup, int lastBidValue, int lastBidQuantity)
        {
            int OneCount = 0;
            int TwoCount = 0;
            int ThreeCount = 0;
            int FourCount = 0;
            int FiveCount = 0;
            int SixCount = 0;

            foreach (var die in cup.GetDiceValues())
            {
                Console.WriteLine(die);
                if (die == 1)
                {
                    OneCount++;
                    
                }
                if (die == 2)
                {
                    TwoCount++;
                    

                }
                if (die == 3)
                {
                    ThreeCount++;
                    

                }
                if (die == 4)
                {
                    FourCount++;
                    

                }
                if (die == 5)
                {
                    FiveCount++;
                }
                if (die == 6)
                {
                    SixCount++;
                }
            }
            




        

            double probabilityOfLiar = 0;
            if (lastBidValue == 1)
            {
                probabilityOfLiar = (lastBidQuantity + OneCount) / 10;
                

            }
            if (lastBidValue == 2)
            {
                probabilityOfLiar = (double)(lastBidQuantity + TwoCount) / 10;
                

            }
            if (lastBidValue == 3)
            {
                probabilityOfLiar = (double)(lastBidQuantity + ThreeCount) / 10;
                

            }
            if (lastBidValue == 4)
            {
                probabilityOfLiar = (double)(lastBidQuantity + FourCount) / 10;
               

            }
            if (lastBidValue == 5)
            {
                probabilityOfLiar = (double)(lastBidQuantity + FiveCount) / 10;
                

            }
            if (lastBidValue == 6)
            {
                probabilityOfLiar = (double)(lastBidQuantity + SixCount) / 10;
                
            }
            return probabilityOfLiar;

        }




    }

    public class Player
    {
        public string Name; 

        public Player(string name)
        {
            Name = name;
        }
    }

    public class Dice
    {
        private int value;
        private int sides;
        private Random rnd = new Random();

        public Dice(int numSides)
        {
            sides = numSides;
        }

        public int GetValue()
        {
            return value;
        }

        public void Roll()
        {
            value = rnd.Next(sides) + 1;
        }
    }

    public class Cup
    {
        private List<Dice> diceList;

        public int DiceCount; 

        public Cup(int numSides)
        {
            diceList = new List<Dice>();
            for (int i = 0; i < 5; i++)
            {
                diceList.Add(new Dice(numSides));
            }
        }

        public void RollDice()
        {
            foreach (var die in diceList)
            {
                die.Roll();
            }
        }

        public List<int> GetDiceValues()
        {
            List<int> diceValues = new List<int>();
            foreach (var die in diceList)
            {
                diceValues.Add(die.GetValue());
            }
            return diceValues;
        }

        public int CountDiceWithValue(int value)
        {
            int count = 0;
            foreach (var die in diceList)
            {
                if (die.GetValue() == value)
                    count++;
            }
            return count;
        }
    }
}





