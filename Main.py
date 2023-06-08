import os

memory = [0] * 1024
registers = [0] * 8
file_extension = "SC"

def help():
    print("Welcome to the CyberStream X1 Terminal!")
    print("Available commands:")
    print("help - Show basic help information")
    print("advanced help - Show detailed help for all commands")
    print("clear - Clear the terminal screen")
    print("view - View the contents of memory and registers")
    print("save - Save the contents of memory and registers to a file")
    print("load - Load the contents of memory and registers from a file")
    # Add more commands here

def advanced_help():
    print("Detailed help for all commands:")
    print("help - This command shows basic help information")
    print("advanced help - This command shows detailed help for all commands")
    print("clear - This command clears the terminal screen")
    print("view - This command displays the contents of memory and registers")
    print("save - This command saves the contents of memory and registers to a file")
    print("load - This command loads the contents of memory and registers from a file")
    # Add more commands here

def clear():
    os.system('cls')  # Use 'clear' instead if you're on Unix-like systems

def view():
    print("Memory Contents:")
    for i, value in enumerate(memory):
        print(f"Address {i}: {value}")
    print("\nRegister Contents:")
    for i, value in enumerate(registers):
        print(f"Register {i}: {value}")

def save():
    filename = input("Enter the filename to save to: ")
    filename += f".{file_extension}"
    with open(filename, 'w') as file:
        file.write("Memory\n")
        for value in memory:
            file.write(str(value) + "\n")
        file.write("Registers\n")
        for value in registers:
            file.write(str(value) + "\n")
    print("Save complete.")

def load():
    filename = input("Enter the filename to load from: ")
    if not filename.endswith(f".{file_extension}"):
        print(f"Invalid file extension. File must have '.{file_extension}' extension.")
        return
    if not os.path.isfile(filename):
        print(f"File '{filename}' does not exist.")
        return
    with open(filename, 'r') as file:
        section = None
        for line in file:
            line = line.strip()
            if line == "Memory":
                section = "memory"
            elif line == "Registers":
                section = "registers"
            elif section == "memory":
                memory.append(int(line))
            elif section == "registers":
                registers.append(int(line))
    print("Load complete.")

# Main program loop
while True:
    command = input("Enter a command: ").lower()

    if command == "help":
        help()
    elif command == "advanced help":
        advanced_help()
    elif command == "clear":
        clear()
    elif command == "view":
        view()
    elif command == "save":
        save()
    elif command == "load":
        load()
    else:
        print("Invalid command. Type 'help' for a list of available commands.")
