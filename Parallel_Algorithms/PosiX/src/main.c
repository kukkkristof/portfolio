#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <pthread.h>

#include "BMPFILE.h"

#define BITMAP_FILE_HEADER_LENGTH 14 // bytes

typedef struct Argument_t
{

    Bmp *Bitmap;
    int startPos;
    int endPos;

} Argument_t;

void *callParallel(void *arg)
{
    Argument_t *argument = arg;
    Bmp *Bitmap = argument->Bitmap;

    scrambleData(Bitmap, argument->startPos, argument->endPos);

}

uint8_t fileHeaderData[BITMAP_FILE_HEADER_LENGTH + 124];

void readFromFile(Bmp *Bitmap, char *path)
{
    FILE *file;
    file = fopen(path, "rb");
    
    fread(fileHeaderData, BITMAP_FILE_HEADER_LENGTH + 124, 1, file);
    fseek(file, 0x0A, SEEK_SET);

    read_bmp(Bitmap, file);

    Bitmap->key = malloc(sizeof(uint8_t) * Bitmap->pixelCount);
    fclose(file);
}

void writeToFile(Bmp *Bitmap, char *path)
{
    FILE *file;

    file = fopen(path, "wb");

    fwrite(fileHeaderData, BITMAP_FILE_HEADER_LENGTH + 124, 1, file);
    fwrite(Bitmap->pixels, Bitmap->dib_header.imageSize, 1, file);
    fclose(file);


    char buff[4] = "out\0";

    char pth[100];
    strcpy(pth, path);

    memcpy(pth + sizeof(char) * strlen(pth)-3, buff, 4);

    file = fopen(pth, "wb");
    fwrite(Bitmap->key, sizeof(uint8_t) * Bitmap->pixelCount, 1, file);
    fclose(file);

}

int main(int argc, char **argv)
{

    srand(time(NULL));

    Bmp Bitmap;

    readFromFile(&Bitmap, "image.bmp");    

    clock_t startTime = clock();
    scrambleData(&Bitmap, 0, Bitmap.pixelCount);
    clock_t endTime = clock();

    writeToFile(&Bitmap, "outputSeq.bmp");    

    printf("Time to scramble sequential: %.3lf seconds\n", (double)(endTime-startTime) / CLOCKS_PER_SEC);

    if(argc != 1)
    {

        readFromFile(&Bitmap, "image.bmp");
        int n = atoi(argv[1]);

        Argument_t arg[n];
        pthread_t threads[n];

        for(int i = 0; i < n; i++)
        {
            arg[i].Bitmap = &Bitmap;
            arg[i].startPos = i * Bitmap.pixelCount/n;
            arg[i].endPos = (i==n-1)?Bitmap.pixelCount:((i+1) * Bitmap.pixelCount/n);
        }

        startTime = clock();
        for(int i = 0; i < n; i++) pthread_create(&threads[i], NULL, callParallel, &arg[i]);
        for(int i = 0; i < n; i++) pthread_join(threads[i], NULL);
        endTime = clock();

        writeToFile(&Bitmap, "outputPar.bmp");
        printf("Time to scramble parallel: %.3lf seconds\n", (double)(endTime-startTime) / CLOCKS_PER_SEC);
        
    }

    return 0;
}

