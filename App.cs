namespace Calc_app
{
    internal class App
    {
        static void Main()
        {
            MatricesMain();
            Console.ReadLine();
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("===== CALCULATOR =====");
                Console.WriteLine("1. Binary addition");
                Console.WriteLine("2. Matrices");

                Console.WriteLine("0. Exit");
                Console.Write("\nChoose an option: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Binary();
                        break;
                    case "2":
                        MatricesMain();
                        Pause();
                        break;
                    case "0":
                        running = false;
                        break;
                    default:
                        Console.WriteLine("That is not a valid choice.");
                        Pause();
                        break;
                }
            }
        }

        static void Pause()
        {
            Console.WriteLine("\nPress [Enter] to continue");
            Console.ReadLine();
        }

        //===================================================BINARY (Redundant)========================================================
        //=============================================================================================================================

        // Returns true if the text is not empty and only contains 0s and 1s
        static bool IsBinary(string text)
        {
            if (text.Length == 0)
            {
                return false;
            }
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] != '0' && text[i] != '1')
                {
                    return false;
                }
            }
            return true;
        }

        static void Binary()
        {
            Console.Write("Please enter the first binary number to add: ");
            string? first = Console.ReadLine()?.Trim();
            Console.Write("Please enter the second binary number to add: ");
            string? second = Console.ReadLine()?.Trim();

            if (!IsBinary(first) || !IsBinary(second))
            {
                Console.WriteLine("Binary numbers can only contain 0 and 1.");
                Pause();
            }
            else
            {
                // Make both numbers the same length so the digits line up
                int maxLength = Math.Max(first.Length, second.Length);
                first = first.PadLeft(maxLength, '0');
                second = second.PadLeft(maxLength, '0');

                int[] numbers = new int[maxLength];
                int[] numbersToo = new int[maxLength];
                int[] result = new int[maxLength + 1];

                // Turn each character into an integer and store it in an array
                for (int i = 0; i < maxLength; i++) 
                {
                    numbers[i] = Convert.ToInt32(first[i].ToString());
                    numbersToo[i] = Convert.ToInt32(second[i].ToString());
                }

                // Add from the right-hand side, keeping track of the carry
                int carry = 0;
                for (int i = maxLength - 1; i >= 0; i--)
                {
                    int added = numbers[i] + numbersToo[i] + carry;
                    result[i + 1] = added % 2;
                    carry = added / 2;
                }
                result[0] = carry;

                Console.WriteLine($"\n  {first.PadLeft(maxLength + 1)}");
                Console.WriteLine($"+ {second.PadLeft(maxLength + 1)}");
                Console.WriteLine(new string('-', maxLength + 3));
                Console.Write("  ");
                for (int i = 0; i < result.Length; i++)
                {
                    Console.Write(result[i]);
                }
                Console.WriteLine();
                Pause();
            }
        }


        ////===================================================MATRICES========================================================
        ////===================================================================================================================
        public class Matrix
        {
            public double a, b, c, d;
            public Matrix(double a, double b, double c, double d)
            {
                this.a = a;
                this.b = b;
                this.c = c; 
                this.d = d;
            }
        }

        // Matrices are stored by single-letter name e.g. a = index 0, b = index 1
        static Matrix?[] stored = new Matrix?[26];

        static void MatricesMain()
        {
            bool running = true;
            Console.WriteLine("Calculator. Type help for the commands, exit to quit.");

            while (running)
            {
                Console.Write("> ");

                string? input = Console.ReadLine();
                if (input == null)
                {
                    input = "";
                }
                string[] parts = SplitWords(input);


                if (parts.Length > 0)
                {
                    string command = parts[0].ToLower();

                    switch (command)
                    {
                        case "mat":
                            CreateMat(parts);
                            break;

                        case "addmat":
                            if (parts.Length != 3)
                            {
                                Console.WriteLine("Usage: addMat a b");
                            }
                            else
                            {
                                Matrix? first = GetMat(parts[1]);
                                Matrix? second = GetMat(parts[2]);
                                if (first != null && second != null)
                                {
                                    displayMat(addMat(first, second), "Result");
                                }
                            }
                            break;

                        case "dotmat":
                            if (parts.Length != 3)
                            {
                                Console.WriteLine("Usage: dotMat a b");
                            }
                            else
                            {
                                Matrix? first = GetMat(parts[1]);
                                Matrix? second = GetMat(parts[2]);
                                if (first != null && second != null)
                                {
                                    displayMat(dotMat(first, second), "Result");
                                }
                            }
                            break;

                        case "scalmat":
                            if (parts.Length != 3)
                            {
                                Console.WriteLine("Usage: scalMat s a");
                            }
                            else
                            {
                                Matrix? mat = GetMat(parts[2]);
                                if (!double.TryParse(parts[1], out double s))
                                {
                                    Console.WriteLine("The scalar must be a number.");
                                }
                                else if (mat != null)
                                {
                                    displayMat(scalMat(mat, s), "Result");
                                }
                            }
                            break;

                        case "detmat":
                            if (parts.Length != 2)
                            {
                                Console.WriteLine("Usage: detMat a");
                            }
                            else
                            {
                                Matrix? mat = GetMat(parts[1]);
                                if (mat != null)
                                {
                                    detMat(mat);
                                }
                            }
                            break;

                        case "invmat":
                            if (parts.Length != 2)
                            {
                                Console.WriteLine("Usage: invMat a");
                            }
                            else
                            {
                                Matrix? mat = GetMat(parts[1]);
                                if (mat != null)
                                {
                                    displayMat(invMat(mat), "Result");
                                }
                            }
                            break;

                        case "help":
                            ShowHelp();
                            break;

                        case "exit":
                            running = false;
                            break;

                        default:
                            Console.WriteLine("Unknown command. Type help to see the commands.");
                            break;
                    }
                }
            }
        }

        static string[] SplitWords(string line)
        {
            line = line.Replace("(", " ").Replace(")", " ");

            // count the words so the array is the right size
            int count = 0;
            bool inWord = false;
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    inWord = false;
                }
                else
                {
                    if (!inWord)
                    {
                        count++;
                    }
                    inWord = true;
                }
            }

            //build each word one character at a time and store it
            string[] result = new string[count];
            int slot = 0;
            string current = "";
            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    if (current != "")
                    {
                        result[slot] = current;
                        slot++;
                        current = "";
                    }
                }
                else
                {
                    current += line[i];
                }
            }
            if (current != "")
            {
                result[slot] = current;
            }

            return result;
        }

        // Turns a matrix name such as "a" into a position 0-25, or -1 if it isn't a single letter
        static int GetIndex(string name)
        {
            name = name.ToLower();
            if (name.Length != 1 || name[0] < 'a' || name[0] > 'z')
            {
                return -1;
            }
            return name[0] - 'a';
        }

        // Finds a stored matrix by name, printing an error and returning null if it can't
        static Matrix? GetMat(string name)
        {
            int index = GetIndex(name);
            if (index == -1)
            {
                Console.WriteLine($"'{name}' is not a valid name. Use a single letter.");
                return null;
            }
            if (stored[index] == null)
            {
                Console.WriteLine($"Matrix {name} has not been created. Use: mat {name} ( a b c d )");
                return null;
            }
            return stored[index];
        }

        // mat a ( 4 7 2 6 )
        static void CreateMat(string[] parts)
        {
            if (parts.Length != 6)
            {
                Console.WriteLine("Usage: mat a ( 4 7 2 6 )");
                return;
            }

            int index = GetIndex(parts[1]);
            if (index == -1)
            {
                Console.WriteLine($"'{parts[1]}' is not a valid name. Use a single letter.");
                return;
            }

            if (double.TryParse(parts[2], out double a) && double.TryParse(parts[3], out double b) && double.TryParse(parts[4], out double c) && double.TryParse(parts[5], out double d))
            {
                stored[index] = new Matrix(a, b, c, d);
                displayMat(stored[index], $"Matrix {parts[1]}");
            }
            else
            {
                Console.WriteLine("All four values must be numbers.");
            }
        }

        static void ShowHelp()
        {
            Console.WriteLine("Matrix commands (matrices are 2x2, named with one letter):");
            Console.WriteLine("  mat a ( 4 7 2 6 )   create matrix a");
            Console.WriteLine("  addMat a b          add a and b");
            Console.WriteLine("  dotMat a b          dot product of a and b");
            Console.WriteLine("  scalMat s a         multiply a by the number s");
            Console.WriteLine("  detMat a            determinant of a");
            Console.WriteLine("  invMat a            inverse of a");
            Console.WriteLine("  help                show this list");
            Console.WriteLine("  exit                quit");
        }

        public static void displayMat(Matrix? mat, string name)
        {
            if (mat == null)
            {
                // If the matrix is null, do nothing
            }
            else
            {
                Console.WriteLine($"{name}:");
                Console.WriteLine($"{mat.a} {mat.b}");
                Console.WriteLine($"{mat.c} {mat.d}");
            }
        }

        public static Matrix? addMat(Matrix? a, Matrix? b)
        {
            if (a == null || b == null)
            {
                Console.WriteLine("One or both matrices are not defined.");
                return null;
            }
            else
            {
                return new Matrix(a.a + b.a, a.b + b.b, a.c + b.c, a.d + b.d);
            }
        }

        public static Matrix? dotMat(Matrix a, Matrix b)
        {
            if (a == null || b == null)
            {
                Console.WriteLine("One or both matrices are not defined.");
                return null;
            }
            else
            {
                return new Matrix(
                a.a * b.a + a.b * b.c,
                a.a * b.b + a.b * b.d,
                a.c * b.a + a.d * b.c,
                a.c * b.b + a.d * b.d
                );
            }
        }

        public static Matrix scalMat(Matrix a, double s)
        {
            return new Matrix(a.a * s, a.b * s, a.c * s, a.d * s);
        }

        public static void detMat(Matrix a)
        {
            if (a == null)
            {
                Console.WriteLine("Matrix is not defined.");
            }
            else
            {
                double result = (a.a * a.d) - (a.b * a.c);

                Console.WriteLine($"Determinant: {result}");
            }
        }

        public static Matrix? invMat(Matrix a)
        {
            if (a == null)
            {
                Console.WriteLine("Matrix is not defined.");
                return null;
            }
            else
            {
                double det = (a.a * a.d) - (a.b * a.c);

                if (det == 0)
                {
                    Console.WriteLine("Matrix has no inverse.");
                    return null;
                }

                return new Matrix(
                    a.d / det,
                    -a.b / det,
                    -a.c / det,
                    a.a / det
                );
            }
        }
    }
}
 