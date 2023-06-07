import os

# SwiftCode Terminal

# Global variable to store the program code
program_code = ""

# Function to execute SwiftCode commands
def execute_command(command):
    global program_code
    
    if command == "help":
        print("Welcome to the SwiftCode terminal!")
        print("Available commands:")
        print("help - Show this help message")
        print("advanced help - Show advanced help information")
        print("add <num1> <num2> - Add two numbers")
        print("subtract <num1> <num2> - Subtract num2 from num1")
        print("run <filename> - Execute the SwiftCode program from a file")
        print("memory - View the current memory state")
        print("save <filename> - Save the current program to a file")
        print("load <filename> - Load and edit a program from a file")
        print("clear - Clear the terminal screen")
    elif command == "advanced help":
        print("Advanced help information:")
        print("Functions:")
        print("1. add(num1, num2): Adds two numbers and returns the sum.")
        print("2. subtract(num1, num2): Subtracts num2 from num1 and returns the difference.")
    elif command.startswith("add"):
        numbers = command.split(" ")[1:]
        if len(numbers) == 2:
            result = add(float(numbers[0]), float(numbers[1]))
            print(f"The sum is: {result}")
        else:
            print("Invalid number of arguments for 'add' command.")
    elif command.startswith("subtract"):
        numbers = command.split(" ")[1:]
        if len(numbers) == 2:
            result = subtract(float(numbers[0]), float(numbers[1]))
            print(f"The difference is: {result}")
        else:
            print("Invalid number of arguments for 'subtract' command.")
    elif command.startswith("run"):
        filename = command.split(" ")[1]
        run_program(filename)
    elif command == "memory":
        view_memory()
    elif command.startswith("save"):
        _, filename, *path_parts = command.split(" ")[1:]
        path = os.path.join(*path_parts)
        save_program(filename, path)
    elif command.startswith("load"):
        _, filename, *path_parts = command.split(" ")[1:]
        path = os.path.join(*path_parts)
        load_program(filename, path)
    elif command == "clear":
        clear_terminal()
    elif command.isdigit():
        line_number = int(command)
        program_code += f"{line_number}: {command}\n"
    else:
        print("Unknown command. Type 'help' to see available commands.")

# Function to add two numbers
def add(num1, num2):
    return num1 + num2

# Function to subtract one number from another
def subtract(num1, num2):
    return num1 - num2
# Function to load and edit a program from a file
def load_program(filename, path=""):
    global program_code
    
    if path:
        filename = os.path.join(path, filename)
    
    filename_with_extension = f"{filename}.swc"  # Custom file extension
    try:
        with open(filename_with_extension, "r") as file:
            program_code = file.read()
            print(f"Loading program from file: {filename_with_extension}")
    except FileNotFoundError:
        print(f"File '{filename_with_extension}' not found.")

# Function to clear the terminal screen
def clear_terminal():
    os.system('cls' if os.name == 'nt' else 'clear')

# Main terminal loop
while True:
    user_input = input("SwiftCode> ")  # Prompt for user input
    execute_command(user_input)
