using System;
using System.Collections.Generic;

class Program
{
    // Register file
    struct RegisterFile
    {
        public int[] reg; // 7 general-purpose registers

        public RegisterFile()
        {
            reg = new int[8];
            for (int i = 0; i < reg.Length; i++)
            {
                reg[i] = 0;
            }
        }
    }

    // Memory
    struct Memory
    {
        public List<int> data; // Data memory

        public Memory()
        {
            data = new List<int>();
        }

        public void Store(int address, int value)
        {
            if (address >= 0 && address < data.Count)
            {
                data[address] = value;
            }
            else
            {
                Console.WriteLine("Error: Invalid memory address!");
            }
        }

        public int Load(int address)
        {
            if (address >= 0 && address < data.Count)
            {
                return data[address];
            }
            else
            {
                Console.WriteLine("Error: Invalid memory address!");
                return 0;
            }
        }
    }

    // CPU
    class CPU
    {
        private RegisterFile regFile;
        private Memory memory;
        private int programCounter;

        public CPU()
        {
            programCounter = 0;
            regFile = new RegisterFile();
            memory = new Memory();
        }

        public void ExecuteCommand(string command)
        {
            string[] parts = command.Split(' ');

            if (parts.Length > 0)
            {
                string opcode = parts[0].ToLower();

                switch (opcode)
                {
                    case "add":
                        if (parts.Length == 3)
                        {
                            int register = ParseRegister(parts[1]);
                            int value = ParseValue(parts[2]);
                            regFile.reg[register] += value;
                        }
                        else
                        {
                            Console.WriteLine("Error: Invalid add command format!");
                        }
                        break;

                    case "sub":
                        if (parts.Length == 3)
                        {
                            int register = ParseRegister(parts[1]);
                            int value = ParseValue(parts[2]);
                            regFile.reg[register] -= value;
                        }
                        else
                        {
                            Console.WriteLine("Error: Invalid sub command format!");
                        }
                        break;

                    case "put":
                        if (parts.Length == 3)
                        {
                            int register = ParseRegister(parts[1]);
                            int value = ParseValue(parts[2]);
                            regFile.reg[register] = value;
                        }
                        else
                        {
                            Console.WriteLine("Error: Invalid put command format!");
                        }
                        break;

                    case "view":
                        View();
                        break;

                    case "save":
                        Save();
                        break;

                    case "load":
                        Load();
                        break;

                    case "run":
                        Run();
                        break;

                    default:
                        Console.WriteLine("Error: Invalid command!");
                        break;
                }
            }
        }

        private int ParseRegister(string registerStr)
        {
            if (int.TryParse(registerStr, out int register) && register >= 1 && register <= 7)
            {
                return register;
            }
            else
            {
                Console.WriteLine("Error: Invalid register number!");
                return -1;
            }
        }

        private int ParseValue(string valueStr)
        {
            if (int.TryParse(valueStr, out int value))
            {
                return value;
            }
            else
            {
                Console.WriteLine("Error: Invalid value!");
                return 0;
            }
        }

        private void View()
        {
            Console.WriteLine("Registers:");
            for (int i = 1; i <= 7; i++)
            {
                Console.WriteLine($"R{i}: {regFile.reg[i]}");
            }

            Console.WriteLine("Memory:");
            for (int i = 0; i < memory.data.Count; i++)
            {
                Console.WriteLine($"M[{i}]: {memory.data[i]}");
            }
        }

        private void Save()
        {
            // Save the contents of memory and registers to a file
            Console.WriteLine("Save command executed.");
        }

        private void Load()
        {
            // Load the contents of memory and registers from a file
            Console.WriteLine("Load command executed.");
        }

        private void Run()
        {
            // Execute the instructions in memory
            while (programCounter < memory.data.Count)
            {
                int instruction = memory.Load(programCounter);
                ExecuteInstruction(instruction);
            }

            Console.WriteLine("Program execution completed.");
        }

        private void ExecuteInstruction(int instruction)
        {
            int opcode = (instruction >> 24) & 0xFF; // Extract the opcode from the instruction

            // Decode the opcode and perform the corresponding operation
            switch (opcode)
            {
                case 0x01: // Add
                    {
                        int rd = (instruction >> 16) & 0xFF; // Extract the destination register
                        int rs1 = (instruction >> 8) & 0xFF; // Extract the first source register
                        int rs2 = instruction & 0xFF;        // Extract the second source register

                        regFile.reg[rd] = regFile.reg[rs1] + regFile.reg[rs2]; // Perform addition

                        break;
                    }
                case 0x02: // Store
                    {
                        int rs1 = (instruction >> 16) & 0xFF; // Extract the source register
                        int immediate = instruction & 0xFFFF; // Extract the immediate value

                        int address = regFile.reg[rs1] + immediate; // Calculate the memory address
                        int data = regFile.reg[(instruction >> 8) & 0xFF]; // Extract the data to be stored

                        memory.Store(address, data); // Store the data in memory

                        break;
                    }
                case 0x03: // Load
                    {
                        int rd = (instruction >> 16) & 0xFF; // Extract the destination register
                        int rs1 = (instruction >> 8) & 0xFF; // Extract the source register
                        int immediate = instruction & 0xFFFF; // Extract the immediate value

                        int address = regFile.reg[rs1] + immediate; // Calculate the memory address
                        int data = memory.Load(address); // Load the data from memory

                        regFile.reg[rd] = data; // Store the data in the destination register

                        break;
                    }
                // ... other opcodes and corresponding operations
            }

            programCounter++; // Move to the next instruction
        }
    }

    static void Main(string[] args)
    {
        CPU cpu = new CPU();

        Console.WriteLine("ChatScript Programming Language");

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();

            if (input.ToLower() == "quit")
            {
                break;
            }

            cpu.ExecuteCommand(input);
        }
    }
}
