using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tar2_Hadasim4._0
{
    enum Options { Rectangle = 1, Triangle, Exit }

    internal class Program
    {
        //A program for handling the "Twitter Towers"
        static void Main(string[] args)
        {
            //Choice of rectangle, triangle or exit
            int choice;
            Console.WriteLine("If you want to select a rectangle tower, press 1\r\nIf in a triangle tower, press 2\r\nTo exit, press 3");
            choice = int.Parse(Console.ReadLine());
            while (choice != 3)
            {
                switch (choice)
                {
                    case (int)Options.Rectangle:
                        Rectangle();
                        break;
                    case (int)Options.Triangle:
                        Triangle();
                        break;
                    case (int)Options.Exit:
                        break;
                    default:
                        Console.WriteLine("ERROR! try to choose another option!");
                        break;
                }
                Console.WriteLine("If you want to select a rectangle tower, press 1,\r\nIf in a triple tower, press 2\r\nTo exit, press 3");
                choice = int.Parse(Console.ReadLine());
            }
        }

        //A function for handling a rectangle
        static void Rectangle()
        {
            int height, width;
            Console.Write("insert height: ");
            height = int.Parse(Console.ReadLine());//valid input promised...
            Console.Write("insert width: ");
            width = int.Parse(Console.ReadLine());
            //Is it a square or a rectangle with the difference between the lengths of the sides His is greater than 5
            if (height == width || Math.Abs(height - width) > 5)
                Console.WriteLine($"The rectangle area is:{height * width}");
            else
                Console.WriteLine($"The rectangle perimeter is:{2 * (height + width)}");
        }

        //A function for handling a triangle
        static void Triangle()
        {
            int height, width, choice;
            Console.Write("insert height: ");
            height = int.Parse(Console.ReadLine());
            Console.Write("insert width: ");
            width = int.Parse(Console.ReadLine());
            Console.Write("press 1 to perimeter, 2 to print the triangle: ");
            choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    //Calculation of 2 sides by Pythagoras
                    double yeter = Math.Sqrt(Math.Pow(width / 2.0, 2) + Math.Pow(height, 2));
                    Console.WriteLine($"perimeter is: {2 * yeter + width}");
                    break;
                case 2:
                    PrintTriangle(height, width);
                    break;
                default:
                    Console.WriteLine("ERROR! try to choose another option!");
                    break;
            }
        }

        //A function for handling a triangle printing
        static void PrintTriangle(int height, int width)
        {
            if (width % 2 == 0 || width > 2 * height)
                Console.WriteLine("ERROR! The triangle cannot be printed!");
            else
            {
                int h;
                //Calculation of how many rows will be at each value of width
                h = (height - 2) / (width / 2 - 1);
               
                Console.WriteLine(new string(' ', width / 2) + "*");//print the first line of just 1 '*'
               
                //Printing rows from the top group according to the remainder of the division
                for (int i = 1; i <= (height - 2) % (width / 2 - 1); i++)
                {
                    //Printing spaces and asterisks accordingly
                    Console.WriteLine(new string(' ', (width / 2) - 1) + new string('*', 3));
                }

                //Printing all internal groups when each group has the same number of lines
                for (int i = 3; i < width; i += 2)//For each odd-width in the range: 1 < odd-width < triangle-width
                {
                    for (int j = 1; j <= h; j++)//Print h rows for the same width
                    {
                        //Printing spaces and asterisks accordingly
                        Console.WriteLine(new string(' ', (width / 2) - i / 2) + new string('*', i));
                    }
                }
                //Printing asterisks row as wide as the triangle width
                Console.WriteLine(new string('*', width));
            }

        }
    }
}
