using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Лаб_1
{
    enum Direction
    {
        LEFT,
        RIGHT,
        UP,
        DOWN
    }

    class Figure
    {
        protected List<Point> pList;

        public void Draw()
        {
            foreach (Point p in pList)
            {
                p.Draw();
            }
        }
        public bool IsHit(Figure figure)
        {
            foreach (var p in pList)
            {
                if (figure.IsHit(p))
                    return true;
            }
            return false;
        }

        private bool IsHit(Point point)
        {
            foreach (var p in pList)
            {
                if (p.IsHit(point))
                    return true;
            }
            return false;
        }

    }
    class VerticalLine : Figure
    {
        public VerticalLine(int yUp, int yDown, int x, char sym)
        {
            pList = new List<Point>();
            for (int y = yUp; y <= yDown; y++)
            {
                Point p = new Point(x, y, sym);
                pList.Add(p);
            }
        }
    }

    class HorizontalLine : Figure
    {
        public HorizontalLine(int xLeft, int xRight, int y, char sym)
        {
            pList = new List<Point>();
            for (int x = xLeft; x <= xRight; x++)
            {
                Point p = new Point(x, y, sym);
                pList.Add(p);
            }
        }
    }

    class Point
    {
        public int x;
        public int y;
        public char sym;



        public Point(int x, int y, char sym)
        {
            this.x = x;
            this.y = y;
            this.sym = sym;
        }
        public Point(Point p)
        {
            x = p.x;
            y = p.y;
            sym = p.sym;
        }



        public void Draw()
        {
            Console.SetCursorPosition(x, y);
            Console.Write(sym);
        }
        public bool IsHit(Point p)
        {
            return p.x == this.x && p.y == this.y;
        }

        public void Move(int offset, Direction direction)
        {
            if (direction == Direction.RIGHT)
            {
                x = x + offset;
            }
            else if (direction == Direction.LEFT)
            {
                x = x - offset;
            }
            else if (direction == Direction.UP)
            {
                y = y - offset;
            }
            else if (direction == Direction.DOWN)
            {
                y = y + offset;
            }
        }

        public void Clear()
        {
            sym = ' ';
            Draw();
        }



    }

    class Walls
    {
        List<Figure> wallList;

        public Walls(int mapWidth, int mapHeight)
        {
            wallList = new List<Figure>();

            // Отрисовка рамочки
            HorizontalLine upLine = new HorizontalLine(0, mapWidth - 2, 0, '▀');
            HorizontalLine downLine = new HorizontalLine(0, mapWidth - 2, mapHeight - 1, '▄');
            VerticalLine leftLine = new VerticalLine(0, mapHeight - 1, 0, '▐');
            VerticalLine rightLine = new VerticalLine(0, mapHeight - 1, mapWidth - 2, '▌');

            wallList.Add(upLine);
            wallList.Add(downLine);
            wallList.Add(leftLine);
            wallList.Add(rightLine);
        }

        public bool IsHit(Figure figure)
        {
            foreach (var wall in wallList)
            {
                if (wall.IsHit(figure))
                {

                    return true;
                }
            }
            return false;
        }



        public void Draw()
        {
            foreach (var wall in wallList)
            {
                wall.Draw();
            }
        }
    }

    class Snake : Figure
    {
        Direction direction;

        
        public int n;
        public int record;
        public Snake(Point tail, int length, Direction _direction)
        {
            direction = _direction;
            pList = new List<Point>();
            for (int i = 0; i < length; i++)
            {
                Point p = new Point(tail);
                p.Move(i, direction);
                pList.Add(p);
            }
        }

        public bool HitFood(Point point)
        {
            foreach (var item in pList)
            {
                if (item.IsHit(point))
                    return true;

            }
            return false;
        }

        public bool Move()
        {

            Point tail = pList.First();
            pList.Remove(tail);
            Point head = GetNextPoint();
            pList.Add(head);

            tail.Clear();
            head.Draw();

            foreach (var i in pList)
            {
                if (tail.IsHit(i))
                {
                    return false;
                }
            }
            return true;

        }





        private Point GetNextPoint()
        {
            Point head = pList.Last();
            Point nextPoint = new Point(head);
            nextPoint.Move(1, direction);
            return nextPoint;
        }

        public void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.LeftArrow && direction != Direction.RIGHT)
                direction = Direction.LEFT;
            else if (key == ConsoleKey.RightArrow && direction != Direction.LEFT)
                direction = Direction.RIGHT;
            else if (key == ConsoleKey.DownArrow && direction != Direction.UP)
                direction = Direction.DOWN;
            else if (key == ConsoleKey.UpArrow && direction != Direction.DOWN)
                direction = Direction.UP;
        }

        public bool Eat(Point food)
        {
            Point head = GetNextPoint();

            if (head.IsHit(food))
            {
                food.sym = head.sym;
                pList.Add(food);

                return true;
            }
            else
                return false;
        }

        class Program
        {



            static void Main(string[] args)
            {
                bool j = true;

                int save = 0;



                while (j)
                {

                    int speed = 100;
                    Walls walls = new Walls(80, 30);
                    walls.Draw();


                    Console.SetCursorPosition(81, 0);
                    Console.WriteLine("1.Начать игру");
                    Console.SetCursorPosition(81, 1);
                    Console.WriteLine("2.Закрыть программу");
                    Console.SetCursorPosition(81, 5);
                    Console.WriteLine("Рекорд: " + save);



                    switch (Console.ReadLine())
                    {
                        case "1":

                            Console.ForegroundColor = ConsoleColor.Green;
                            Point p = new Point(4, 5, 'O');
                            Snake snake = new Snake(p, 4, Direction.RIGHT);
                            snake.Draw();
                            Point food = new Point(10, 20, '*');
                            food.Draw();


                            while (j)
                            {
                                Console.SetCursorPosition(32, 1);
                                Console.WriteLine("Ваш счет: " + snake.n);
                                Random r1 = new Random();
                                int R1 = r1.Next(2, 78);
                                Random r2 = new Random();
                                int R2 = r2.Next(2, 28);
                                if (snake.n > snake.record)
                                {
                                    snake.record = snake.n;
                                }
                                Console.SetCursorPosition(81, 3);


                                if (snake.Eat(food))
                                {

                                    snake.n++;

                                    speed -= 1;

                                    if (speed <= 50)
                                    {
                                        speed = 50;
                                    }

                                    food = new Point(R1, R2, '*');
                                    while (snake.HitFood(food))
                                    {
                                        R1 = r1.Next(3, 77);
                                        R2 = r2.Next(3, 27);
                                        food = new Point(R1, R2, '*');
                                    }
                                    food.Draw();

                                    

                                }
                                else
                                {
                                    if (snake.Move() == false)
                                    {
                                        Console.ResetColor();
                                        Console.SetCursorPosition(30, 12);
                                        Console.WriteLine("ИГРА ОКОНЧЕНА");
                                        Console.ReadKey();
                                        Console.Clear();

                                        break;
                                    }

                                }
                                Thread.Sleep(speed);
                                if (Console.KeyAvailable)
                                {
                                    ConsoleKeyInfo key = Console.ReadKey();
                                    snake.HandleKey(key.Key);
                                }

                                if (walls.IsHit(snake))
                                {
                                    Console.ResetColor();
                                    Console.SetCursorPosition(32, 12);
                                    Console.WriteLine("ИГРА ОКОНЧЕНА");
                                    Console.ReadKey();
                                    Console.Clear();
                                    break;
                                }
                            }
                            if (save < snake.record)
                            {
                                save = snake.record;
                            }
                            


                            break;
                        case "2":
                            j = false;
                            break;

                    }
                }



            }
        }
    }
}
