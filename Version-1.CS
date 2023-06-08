using System;
using System.Collections.Generic;

// Register file
struct RegisterFile
{
    public int[] reg; // 8 general-purpose registers

    public RegisterFile()
    {
        reg = new int[8];
        for (int i = 0; i < reg.Length; i++)
        {
            reg[i] = 0;
        }
    }
}

// ALU (Arithmetic Logic Unit)
struct ALU
{
    public int Add(int a, int b) { return a + b; }
    public int Subtract(int a, int b) { return a - b; }
    // ... other ALU operations
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
    private ALU alu;
    private Memory memory;
    private int programCounter;

    public CPU()
    {
        programCounter = 0;
        regFile = new RegisterFile();
        alu = new ALU();
        memory = new Memory();
    }

    public void ExecuteInstruction(int instruction)
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

                    int result = alu.Add(regFile.reg[rs1], regFile.reg[rs2]); // Perform addition using ALU
                    regFile.reg[rd] = result; // Store the result in the destination register

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

class Program
{
    static void Main(string[] args)
    {
        CPU cpu = new CPU();

        // Example program
        List<int> instructions = new List<int>
        {
            0x01020304, // Add R3 = R1 + R2
            0x02100456, // Store R4 at [R1 + 0x0456]
            0x03180234  // Load [R2 + 0x0234] into R3
            // ... more instructions
        };

        // Load the instructions into memory
        for (int i = 0; i < instructions.Count; i++)
        {
            cpu.memory.Store(i, instructions[i]);
        }

        while (cpu.programCounter < instructions.Count)
        {
            int instruction = cpu.memory.Load(cpu.programCounter);
            cpu.ExecuteInstruction(instruction);
        }

        Console.WriteLine("Program execution completed.");
    }
}
