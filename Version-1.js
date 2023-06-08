// Register file
class RegisterFile {
  constructor() {
    this.reg = new Array(8).fill(0);
  }
}

// ALU (Arithmetic Logic Unit)
class ALU {
  add(a, b) {
    return a + b;
  }

  subtract(a, b) {
    return a - b;
  }

  // ... other ALU operations
}

// Memory
class Memory {
  constructor() {
    this.data = [];
  }

  store(address, value) {
    if (address >= 0 && address < this.data.length) {
      this.data[address] = value;
    } else {
      console.log("Error: Invalid memory address!");
    }
  }

  load(address) {
    if (address >= 0 && address < this.data.length) {
      return this.data[address];
    } else {
      console.log("Error: Invalid memory address!");
      return 0;
    }
  }
}

// CPU
class CPU {
  constructor() {
    this.programCounter = 0;
    this.regFile = new RegisterFile();
    this.alu = new ALU();
    this.memory = new Memory();
  }

  executeInstruction(instruction) {
    const opcode = (instruction >> 24) & 0xFF; // Extract the opcode from the instruction

    // Decode the opcode and perform the corresponding operation
    switch (opcode) {
      case 0x01: // Add
        {
          const rd = (instruction >> 16) & 0xFF; // Extract the destination register
          const rs1 = (instruction >> 8) & 0xFF; // Extract the first source register
          const rs2 = instruction & 0xFF; // Extract the second source register

          const result = this.alu.add(
            this.regFile.reg[rs1],
            this.regFile.reg[rs2]
          ); // Perform addition using ALU
          this.regFile.reg[rd] = result; // Store the result in the destination register

          break;
        }
      case 0x02: // Store
        {
          const rs1 = (instruction >> 16) & 0xFF; // Extract the source register
          const immediate = instruction & 0xFFFF; // Extract the immediate value

          const address =
            this.regFile.reg[rs1] + immediate; // Calculate the memory address
          const data =
            this.regFile.reg[(instruction >> 8) & 0xFF]; // Extract the data to be stored

          this.memory.store(address, data); // Store the data in memory

          break;
        }
      case 0x03: // Load
        {
          const rd = (instruction >> 16) & 0xFF; // Extract the destination register
          const rs1 = (instruction >> 8) & 0xFF; // Extract the source register
          const immediate = instruction & 0xFFFF; // Extract the immediate value

          const address =
            this.regFile.reg[rs1] + immediate; // Calculate the memory address
          const data = this.memory.load(address); // Load the data from memory

          this.regFile.reg[rd] = data; // Store the data in the destination register

          break;
        }
      // ... other opcodes and corresponding operations
    }

    this.programCounter++; // Move to the next instruction
  }
}

// Example program
const instructions = [
  0x01020304, // Add R3 = R1 + R2
  0x02100456, // Store R4 at [R1 + 0x0456]
  0x03180234, // Load [R2 + 0x0234] into R3
  // ... more instructions
];

const cpu = new CPU();

// Load the instructions into memory
for (let i = 0; i < instructions.length; i++) {
  cpu.memory.store(i, instructions[i]);
}

while (cpu.programCounter < instructions.length) {
  const instruction = cpu.memory.load(cpu.programCounter);
  cpu.executeInstruction(instruction);
}

console.log("Program execution completed.");
