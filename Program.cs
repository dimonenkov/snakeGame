using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

namespace snakeGame
{
    struct Position
    {
        public int row;
        public int col;
        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
           byte right = 0;
           byte left = 1;
           byte down = 2;
           byte up = 3;

            int lastFoodTime = 0;
            int foodDissapearTime = 9000;
            int negativePoint = 0;

           
           

            Position[] directions = new Position[]
            {
                new Position(0, 1), //right
                new Position(0, -1), //left
                new Position(1, 0), //down
                new Position(-1, 0), //top
                
                // >
                // <
                // ^
                // v



            };

            double sleepTime = 100;
            int direction = right;
            Random randomNumbersGenerator = new Random();
            Console.BufferHeight = Console.WindowHeight;
           // Console.BufferWidth = Console.WindowWidth;
            lastFoodTime = Environment.TickCount;
            Console.ForegroundColor = ConsoleColor.White;
           
           
            Console.WriteLine("                 Ready? Go!");




            List<Position> obstacles = new List<Position>();
            {
                new Position(12, 12);
                new Position(14, 20);
                new Position(7, 7);

            };
            foreach (Position obstacle in obstacles)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.SetCursorPosition(obstacle.col, obstacle.row);
                Console.Write("+");
            };
            Queue<Position> snakeElements = new Queue<Position>();
            for (int i = 0; i < 9; i++)
            {
                snakeElements.Enqueue(new Position(0, i));
            }

            Position food;
            do
            {


                food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                   randomNumbersGenerator.Next(0, Console.WindowWidth));

            }

            while (snakeElements.Contains(food) || obstacles.Contains(food));
            Console.SetCursorPosition(food.col, food.row);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("@");


            foreach (Position position in snakeElements)
            {
                Console.SetCursorPosition(position.col, position.row);
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("@");
            }
           
            while (true)
            {
                //negativePoint++;
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo userInput = Console.ReadKey();
                    if (userInput.Key == ConsoleKey.LeftArrow)
                    {
                        if (direction != right) direction = left;
                    }
                    if (userInput.Key == ConsoleKey.RightArrow)
                    {
                        if (direction != left) direction = right;
                    }
                    if (userInput.Key == ConsoleKey.UpArrow)
                    {
                        if (direction != down) direction = up;
                    }
                    if (userInput.Key == ConsoleKey.DownArrow)
                    {
                        if (direction != up) direction = down;
                    }
                }
               
                Position snakeHead = snakeElements.Last();
                Position nextDirection = directions[direction];
                
                Position snakeNewHead = new Position(snakeHead.row + nextDirection.row,
                snakeHead.col + nextDirection.col);
                if (snakeNewHead.col < 0) snakeNewHead.col = Console.WindowWidth - 1;// right
                if (snakeNewHead.row < 0) snakeNewHead.row = Console.WindowHeight - 1;// up
                if (snakeNewHead.row >= Console.WindowHeight) snakeNewHead.row = 0; //down
                if (snakeNewHead.col >= Console.WindowWidth) snakeNewHead.col = 0;




                //if (snakeNewHead.row < 0 ||
                //    snakeNewHead.col < 0 ||
                //    snakeNewHead.row >= Console.WindowHeight ||
                //    snakeNewHead.col >= Console.WindowWidth)

                    if (snakeElements.Contains(snakeNewHead) || obstacles.Contains(snakeNewHead))
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Game over!");
                    int userPoints = (snakeElements.Count - 6) * 173;
                    if (userPoints < 0) userPoints = 0;
                    Console.WriteLine("Your point are: {0}", userPoints);

                    Console.Read();
                    return;
                }
                Console.SetCursorPosition(snakeHead.col, snakeHead.row);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("@");



                snakeElements.Enqueue(snakeNewHead);
                Console.SetCursorPosition(snakeNewHead.col, snakeNewHead.row);
                Console.ForegroundColor = ConsoleColor.Green;
                if (direction == right) Console.Write("<");
                if (direction == left) Console.Write(">");
                if (direction == down) Console.Write("^");
                if (direction == up) Console.Write("V");


                if (snakeNewHead.col == food.col && snakeNewHead.row == food.row)
                {
                    //feeding the snake
                    do
                    {
                        food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(food) && obstacles.Contains(food));
                    lastFoodTime = Environment.TickCount;
                    Console.SetCursorPosition(food.col, food.row);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("@");

                    sleepTime--;

                    Position obstacle = new Position();
                    do
                    {
                        obstacle = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                randomNumbersGenerator.Next(0, Console.WindowWidth));

                    }
                    while (snakeElements.Contains(obstacle) ||
                    obstacles.Contains(obstacle) || (food.row != obstacle.row && food.col != obstacle.col)
                  );
                    obstacles.Add(obstacle);
                    Console.SetCursorPosition(obstacle.col, obstacle.row);
                   
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("+");
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.Write("+");


                }
                else
                {
                    // moving...
                    Position last = snakeElements.Dequeue();
                    Console.SetCursorPosition(last.col, last.row);
                    Console.Write(" ");
                }
                if(Environment.TickCount - lastFoodTime >= foodDissapearTime)
                {
                    negativePoint += 1;
                    Console.SetCursorPosition(food.col, food.row);
                    Console.Write(" ");

                    do
                    {
                        food = new Position(randomNumbersGenerator.Next(0, Console.WindowHeight),
                randomNumbersGenerator.Next(0, Console.WindowWidth));
                    }
                    while (snakeElements.Contains(food) || obstacles.Contains(snakeNewHead));
                    lastFoodTime = Environment.TickCount;
                }


                Console.SetCursorPosition(food.col, food.row);
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.Write("@");
                

                sleepTime -= 0.01;
                
                Thread.Sleep((int)sleepTime);
                
                
            }
            
        }
    } 
}
