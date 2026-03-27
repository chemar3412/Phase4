using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Phase3
{
    public class Calculator
    {
        public static double Add(double a, double b) => a + b;
        public static double Subtract(double a, double b) => a - b;
        public static double Multiply(double a, double b) => a * b;

        public static double Divide(double a, double b)
        {
            if (b == 0)
                throw new DivideByZeroException("Cannot divide by zero.");

            return a / b;
        }
    }

    public class MemoryFunction
    {
        private List<double> memoryValues = new List<double>();
        private const int MaxSize = 10;

        public bool StoreValue(double value)
        {
            if (memoryValues.Count < MaxSize)
            {
                memoryValues.Add(value);
                return true;
            }
            return false;
        }

        public double? RetrieveValue(int index)
        {
            if (index >= 0 && index < memoryValues.Count)
                return memoryValues[index];

            return null;
        }

        public bool ReplaceValue(int index, double newValue)
        {
            if (index >= 0 && index < memoryValues.Count)
            {
                memoryValues[index] = newValue;
                return true;
            }
            return false;
        }

        public bool RemoveValue(int index)
        {
            if (index >= 0 && index < memoryValues.Count)
            {
                memoryValues.RemoveAt(index);
                return true;
            }
            return false;
        }

        public void ClearValues() => memoryValues.Clear();
        public List<double> GetAllValues() => new List<double>(memoryValues);
        public int GetCount() => memoryValues.Count;
        public double GetSum() => memoryValues.Sum();
        public double GetAverage() => memoryValues.Count == 0 ? 0 : memoryValues.Average();

        public double? GetFirstLastDifference()
        {
            if (memoryValues.Count < 2)
                return null;

            return memoryValues.Last() - memoryValues.First();
        }
    }

    public class Menu
    {
        public void Show()
        {
            Console.WriteLine("\nCalculator Menu");
            Console.WriteLine("1. Add");
            Console.WriteLine("2. Subtract");
            Console.WriteLine("3. Multiply");
            Console.WriteLine("4. Divide");
            Console.WriteLine("5. Enter an Equation");
            Console.WriteLine("6. Memory Function");
            Console.WriteLine("7. Exit");
        }
    }

    public class EquationSolver
    {
        public static double Solver(string equation)
        {
            var table = new DataTable();
            var result = table.Compute(equation, "");
            return Convert.ToDouble(result);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Project Week 3, Memory Function, Chelsea Martin");
            Console.WriteLine("Welcome to the Calculator!");

            RunMenuLoop();
        }

        static string GetChoice()
        {
            Console.Write("Select an option: ");
            return Console.ReadLine() ?? "";
        }

        static double GetNumber(string label)
        {
            while (true)
            {
                Console.Write($"Enter {label} number: ");
                string? input = Console.ReadLine();

                if (double.TryParse(input, out double num))
                    return num;

                Console.WriteLine("Invalid input. Please enter a numeric value.\n");
            }
        }

        static void ProcessBasicOperation(string choice)
        {
            switch (choice)
            {
                case "1":
                    {
                        double num1 = GetNumber("first");
                        double num2 = GetNumber("second");
                        double result = Calculator.Add(num1, num2);
                        Console.WriteLine($"{num1} + {num2} = {result}");
                        break;
                    }

                case "2":
                    {
                        double num1 = GetNumber("first");
                        double num2 = GetNumber("second");
                        double result = Calculator.Subtract(num1, num2);
                        Console.WriteLine($"{num1} - {num2} = {result}");
                        break;
                    }

                case "3":
                    {
                        double num1 = GetNumber("first");
                        double num2 = GetNumber("second");
                        double result = Calculator.Multiply(num1, num2);
                        Console.WriteLine($"{num1} * {num2} = {result}");
                        break;
                    }

                case "4":
                    {
                        double a, b;

                        while (true)
                        {
                            Console.Write("Enter the first number: ");
                            if (double.TryParse(Console.ReadLine(), out a))
                                break;

                            Console.WriteLine("Invalid input. Please enter a numeric value.\n");
                        }

                        while (true)
                        {
                            Console.Write("Enter the second number: ");
                            if (!double.TryParse(Console.ReadLine(), out b))
                            {
                                Console.WriteLine("Invalid input. Please enter a numeric value.\n");
                                continue;
                            }

                            try
                            {
                                double divResult = Calculator.Divide(a, b);
                                Console.WriteLine($"{a} / {b} = {divResult}");
                                break;
                            }
                            catch (DivideByZeroException)
                            {
                                Console.WriteLine("Cannot divide by zero. Please enter a number other than zero.\n");
                            }
                        }
                        break;
                    }

                case "5":
                    {
                        Console.Write("Enter an equation: ");
                        string? equation = Console.ReadLine();

                        if (string.IsNullOrWhiteSpace(equation))
                        {
                            Console.WriteLine("Invalid equation.");
                            break;
                        }

                        try
                        {
                            double eqResult = EquationSolver.Solver(equation);
                            Console.WriteLine($"Result: {equation} = {eqResult}");
                        }
                        catch
                        {
                            Console.WriteLine("Invalid equation format.");
                        }
                        break;
                    }
            }
        }

        static void RunMemoryMenu(MemoryFunction memory)
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine("\nMemory Function Menu");
                Console.WriteLine("1. Store a value");
                Console.WriteLine("2. Retrieve a value");
                Console.WriteLine("3. Replace a value");
                Console.WriteLine("4. Remove a value");
                Console.WriteLine("5. Clear all values");
                Console.WriteLine("6. Display all values");
                Console.WriteLine("7. Display count");
                Console.WriteLine("8. Sum of all values");
                Console.WriteLine("9. Average of all values");
                Console.WriteLine("10. Difference (last - first)");
                Console.WriteLine("11. Return to Main Menu");

                string choice = GetChoice();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("Enter values to store. Type 'done' to stop.");

                        while (true)
                        {
                            if (memory.GetCount() >= 10)
                            {
                                Console.WriteLine("Memory is full. Cannot store more values.");
                                break;
                            }

                            Console.Write("Enter a value (or 'done'): ");
                            string? input = Console.ReadLine();

                            if (input == null)
                                continue;

                            if (input.Trim().ToLower() == "done")
                            {
                                Console.WriteLine("Finished storing values.");
                                break;
                            }

                            if (double.TryParse(input, out double val))
                            {
                                memory.StoreValue(val);
                                Console.WriteLine("Value stored.");
                            }
                            else
                            {
                                Console.WriteLine("Invalid number. Try again.");
                            }
                        }
                        break;

                    case "2":
                        Console.Write("Enter index to retrieve: ");
                        if (int.TryParse(Console.ReadLine(), out int idx))
                        {
                            var value = memory.RetrieveValue(idx);
                            Console.WriteLine(value != null ? $"Value: {value}" : "Invalid index.");
                        }
                        break;

                    case "3":
                        Console.Write("Enter index to replace: ");
                        if (int.TryParse(Console.ReadLine(), out int repIdx))
                        {
                            Console.Write("Enter new value: ");
                            if (double.TryParse(Console.ReadLine(), out double newVal))
                            {
                                Console.WriteLine(memory.ReplaceValue(repIdx, newVal)
                                    ? "Value replaced."
                                    : "Invalid index.");
                            }
                        }
                        break;

                    case "4":
                        Console.Write("Enter index: ");
                        if (int.TryParse(Console.ReadLine(), out int remIdx))
                        {
                            Console.WriteLine(memory.RemoveValue(remIdx)
                                ? "Value removed."
                                : "Invalid index.");
                        }
                        break;

                    case "5":
                        memory.ClearValues();
                        Console.WriteLine("All values cleared.");
                        break;

                    case "6":
                        var values = memory.GetAllValues();
                        Console.WriteLine(values.Count == 0
                            ? "No values stored."
                            : "Values: " + string.Join(", ", values));
                        break;

                    case "7":
                        Console.WriteLine($"Count: {memory.GetCount()}");
                        break;

                    case "8":
                        Console.WriteLine($"Sum: {memory.GetSum()}");
                        break;

                    case "9":
                        Console.WriteLine($"Average: {memory.GetAverage()}");
                        break;

                    case "10":
                        var diff = memory.GetFirstLastDifference();
                        Console.WriteLine(diff != null
                            ? $"Difference: {diff}"
                            : "Not enough values.");
                        break;

                    case "11":
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please enter a valid option.");
                        break;
                }
            }
        }

        static void RunMenuLoop()
        {
            Menu menu = new Menu();
            MemoryFunction memory = new MemoryFunction();
            bool running = true;

            while (running)
            {
                menu.Show();
                string choice = GetChoice();

                switch (choice)
                {
                    case "1":
                    case "2":
                    case "3":
                    case "4":
                    case "5":
                        ProcessBasicOperation(choice);
                        break;

                    case "6":
                        RunMemoryMenu(memory);
                        break;

                    case "7":
                        Console.WriteLine("Exiting the calculator. Goodbye!");
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Please select a valid response.");
                        break;
                }
            }
        }
    }
}