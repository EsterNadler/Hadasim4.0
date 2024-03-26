using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tar2_Hadasim4._0
{
    enum Options { Rectangle = 1, Triangle, Exit }

    internal class Program
    {////////enummmmmmmmmmmmm!!!!
        //תקינות לגובה של מגדל?(דרישה בשאלה?
        ///לעשות קאצ למקומות שקלט לא חוקי? איפה שפארס לא הצליח להמיר??
        static void Main(string[] args)
        {
            int choice;
            Console.WriteLine("If you want to select a rectangle tower, press 1\r\nIf in a triangle tower, press 2\r\nTo exit, press 3");
            choice = int.Parse(Console.ReadLine());
            while (choice != 3)
            {
                switch (choice)
                {
                    case 1:
                        Rectangle();
                        break;
                    case 2:
                        Triangle();
                        break;
                    case 3:
                        break;
                    default:
                        Console.WriteLine("ERROR! try to choose another option!");
                        break;
                }
                Console.WriteLine("If you want to select a rectangle tower, press 1,\r\nIf in a triple tower, press 2\r\nTo exit, press 3");
                choice = int.Parse(Console.ReadLine());
            }
        }

        static void Rectangle()//why static???? no operate by object..
        {
            int height, width;
            Console.Write("insert height: ");
            height = int.Parse(Console.ReadLine());//>2!!!!
            Console.Write("insert width: ");
            width = int.Parse(Console.ReadLine());
            if (height == width || Math.Abs(height - width) > 5)
                Console.WriteLine($"The rectangle area is:{height * width}");
            else
                Console.WriteLine($"The rectangle perimeter is:{2 * (height + width)}");
        }

        static void Triangle()
        {
            int height, width, choice;
            Console.Write("insert height: ");
            height = int.Parse(Console.ReadLine());
            Console.Write("insert width: ");
            width = int.Parse(Console.ReadLine());
            Console.Write("press 1 to perimeter, 2 to area: ");
            choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    double yeter = Math.Sqrt(Math.Pow(width / 2.0, 2) + Math.Pow(height, 2));
                    Console.WriteLine($"perimeter is: {2 * yeter + width}");
                    break;
                case 2:
                    PrintTriangle(height,width);
                    break;
                default:
                    Console.WriteLine("ERROR! try to choose another option!");
                    break;
            }
        }

        static void PrintTriangle(int height, int width)
        {
            int h;
            if (width % 2 == 0 || height > 2 * width)
                Console.WriteLine("ERROR!!!");
            else
            {
                h = (height-2) / (width / 2 - 1);
                Console.WriteLine(new string(' ', width/2)+"*");
                for (int i = 1; i <= (height - 2) % (width / 2 - 1); i++)
                {
                    Console.WriteLine(new string(' ', (width / 2)-1)+new string('*', 3));
                }

                for (int i = 3;i<width;i+=2)
                {
                    for (int j = 1;j<=h;j++) {
                        Console.WriteLine(new string(' ', (width / 2) - i/2)+new string('*', i));
                    }
                }
                Console.WriteLine(new string('*',width));
            }

        }
    }
}
