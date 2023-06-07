#include <iostream>
#include <vector>

// Register file
struct RegisterFile {
    int reg[8] = {0}; // 8 general-purpose registers
};

// ALU (Arithmetic Logic Unit)
struct ALU {
    int add(int a, int b) { return a + b; }
    int subtract(int a, int b) { return a - b; }
    // ... other ALU operations
};

// Memory
struct Memory {
    std::vector<int> data; // Data memory

    void store(int address, int value) {
        if (address >= 0 && address < data.size()) {
            data[address] = value;
        } else {
            std::cerr << "Error: Invalid memory address!" << std::endl;
        }
    }

    int load(int address) {
        if (address >= 0 && address < data.size()) {
            return data[address];
        } else {
            std::cerr << "Error: Invalid memory address!" << std::endl;
            return 0;
        }
    }
};

// CPU
class CPU {
private:
    RegisterFile regFile;
    ALU alu;
    Memory memory;
    int programCounter;

public:
    CPU() : programCounter(0) {}

    void executeInstruction(int instruction) {
        int opcode = (instruction >> 24) & 0xFF; // Extract the opcode from the instruction

        // Decode the opcode and perform the corresponding operation
        switch (opcode) {
            case 0x01: // Add
                {
                    int rd = (instruction >> 16) & 0xFF; // Extract the destination register
                    int rs1 = (instruction >> 8) & 0xFF; // Extract the first source register
                    int rs2 = instruction & 0xFF;        // Extract the second source register

                    int result = alu.add(regFile.reg[rs1], regFile.reg[rs2]); // Perform addition using ALU
                    regFile.reg[rd] = result; // Store the result in the destination register

                    break;
                }
            case 0x02: // Store
                {
                    int rs1 = (instruction >> 16) & 0xFF; // Extract the source register
                    int immediate = instruction & 0xFFFF; // Extract the immediate value

                    int address = regFile.reg[rs1] + immediate; // Calculate the memory address
                    int data = regFile.reg[(instruction >> 8) & 0xFF]; // Extract the data to be stored

                    memory.store(address, data); // Store the data in memory

                    break;
                }
            case 0x03: // Load
                {
                    int rd = (instruction >> 16) & 0xFF; // Extract the destination register
                    int rs1 = (instruction >> 8) & 0xFF; // Extract the source register
                    int immediate = instruction & 0xFFFF; // Extract the immediate value

                    int address = regFile.reg[rs1] + immediate; // Calculate the memory address
                    int data = memory.load(address); // Load the data from memory

                    regFile.reg[rd] = data; // Store the data in the destination register

                    break;
                }
            // ... other opcodes and corresponding operations
        }

        programCounter++; // Move to the next instruction
    }
};

int main() {
    CPU cpu;

    // Example program
    std::vector<int> instructions = {
        0x01020304, // Add R3 = R1 + R2
        0x02100456, // Store R4 at [R1 + 0x0456]
        0x03180234  // Load [R2 + 0x0234] into R3
        // ... more instructions
    };

    // Load the instructions into memory
    for (int i = 0; i < instructions.size(); i++) {
        cpu.memory.store(i, instructions[i]);
    }

    while (cpu.programCounter < instructions.size()) {
        int instruction = cpu.memory.load(cpu.programCounter);
        cpu.executeInstruction(instruction);
    }

    std::cout << "Program execution completed." << std::endl;
    return 0;
}
