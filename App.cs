namespace Calc_app
{
    internal class App
    {
        static void Main()
        {
            bool running = true;
            do
            {
                Console.Clear();
                Console.WriteLine("Press 1");
                int mainMenuChoice = Convert.ToInt32(Console.ReadLine());
                switch (mainMenuChoice)
                {
                    case 1:
                        Binary();
                        break;
                    case 2:
                        break;
                    default:
                        Console.WriteLine("That is not a valid Choice");
                        break;
                }
            } while (running);



        }

        static void Binary()
        {
            Console.WriteLine("Please enter the first binary number to add: ");
            string ONEst_Binary = Console.ReadLine();
            Console.WriteLine("Please enter the second binary number to add: ");
            string TWOnd_Binary = Console.ReadLine();

            int maxLength = Math.Max(ONEst_Binary.Length, TWOnd_Binary.Length);
            ONEst_Binary = ONEst_Binary.PadLeft(maxLength, '0');
            TWOnd_Binary = TWOnd_Binary.PadLeft(maxLength, '0');

            int[] Numbers = new int[maxLength];
            int[] NumbersToo = new int[maxLength];
            int[] Result = new int[maxLength + 1];

            for (int i = 0; i < maxLength; i++)
            {
                Numbers[i] = ONEst_Binary[i];
                NumbersToo[i] = TWOnd_Binary[i];
            }

            int overflow = 0;
            int carry = 0;
            for (int i = maxLength - 1; i > 0; i--)
            {
                int Added = Numbers[i] + NumbersToo[i] + carry;
                Result[i + 1] += Added % 2;
                carry = Added / 2;
            }
        }

    }
}
