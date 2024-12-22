all:
	cls
	gcc -Iinclude/ src/*.c -lmingw32 -lSDL2main -lSDL2 -lSDL2_image -lobj -lopengl32 -lm -o GraphicsAssigment.exe -w
	./GraphicsAssigment.exe
	
debug:
	cls
	gcc -Iinclude/ src/*.c -lmingw32 -lSDL2main -lSDL2 -lSDL2_image -lobj -lopengl32 -lm -o GraphicsAssigment.exe -w -g
	gdb ./GraphicsAssigment.exe