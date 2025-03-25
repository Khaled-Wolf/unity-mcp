import sys

def greeting(name):
    print(f"Hey {name}! How's it going?")

if __name__ == '__main__':
    greeting(sys.argv[1])
