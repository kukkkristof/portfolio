#include <stdio.h>
#include <unistd.h>
#include <stdlib.h>
#include <omp.h>
#include <time.h>

#include "BMPFILE.h"

int numThreads;

#define BITMAP_FILE_HEADER_LENGTH 14 // bytes

typedef struct Argument_t
{

    Bmp *Bitmap;

    int startPos;
    int endPos;
    

} Argument_t;

void callParallel(void *arg)
{
    Argument_t *argument = arg;
    Bmp *Bitmap = argument->Bitmap;

    unscrambleData(Bitmap, argument->startPos, argument->endPos);
}

uint8_t fileHeaderData[BITMAP_FILE_HEADER_LENGTH + 124]; // Working bitmap headers

void readFromFile(Bmp *Bitmap, char *path)
{
    FILE *file;
    file = fopen(path, "rb");
    
    fread(fileHeaderData, BITMAP_FILE_HEADER_LENGTH + 124, 1, file);

    // Search up the header location to start data read from
    fseek(file, 0x0A, SEEK_SET);


    read_bmp(Bitmap, file);
    fclose(file);
    
    // Get key length
    Bitmap->key = malloc(sizeof(uint8_t) * Bitmap->pixelCount);
    // Change the given image names .bmp extension to .out to read the key
    char buff[4] = "out\0";
    char pth[100];
    strcpy(pth, path);
    memcpy(pth + sizeof(char) * strlen(pth)-3, buff, 4);

    // Read key data from .out file
    file = fopen(pth, "rb");
    fread(Bitmap->key, sizeof(uint8_t) * Bitmap->pixelCount, 1, file);
    fclose(file);
}

void writeToFile(Bmp *Bitmap, char *path)
{
    FILE *file;

    file = fopen(path, "wb");
    fwrite(fileHeaderData, BITMAP_FILE_HEADER_LENGTH + 124, 1, file);
    fwrite(Bitmap->pixels, Bitmap->dib_header.imageSize, 1, file);
    fclose(file);
}

int main(int argc, char **argv)
{
    Bmp Bitmap;
    clock_t startTime;
    clock_t endTime;
    Argument_t *arg;

    readFromFile(&Bitmap, "image.bmp");

    // Time the algorithm on sequential
    if(argc == 1 || strcmp(argv[1], "par") != 0 || strcmp(argv[2], "1") == 0)
    {
        startTime = clock();
        unscrambleData(&Bitmap, 0, Bitmap.pixelCount);
        endTime = clock();
        goto app_end;
    }

    if(argc < 3)
    {

        printf(":: Not enough arguments given!");

        return -1;
    }

    // If the call is not sequential, then call parallel
    int n = atoi(argv[2]);
    arg = malloc(sizeof(Argument_t) * n);
    for(int i = 0; i < n; i++)
    {
        arg[i].Bitmap = &Bitmap;
        arg[i].startPos = i * Bitmap.pixelCount/n;
        arg[i].endPos = (i==n-1)?Bitmap.pixelCount:((i+1) * Bitmap.pixelCount/n);
    }

    startTime = clock();
    #pragma omp parallel
    {
        #pragma omp for
        for(int i = 0; i < n; i++)
        {
            callParallel(&(arg[i]));
        }
    }
    #pragma omp barrier
    endTime = clock();

app_end:

    writeToFile(&Bitmap, "output.bmp");

    printf(":: Time to unscramble: %.0lf ms\n", (double)(endTime-startTime));

    return 0;
}

