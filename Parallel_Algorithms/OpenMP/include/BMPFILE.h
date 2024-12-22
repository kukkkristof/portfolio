#ifndef BMPFILE_H
#define BMPFILE_H

#include <stdio.h>
#include <stdint.h>
#include <string.h>

typedef struct RGB24
{   
    uint8_t B;
    uint8_t G;
    uint8_t R;
} RGB24;

typedef struct DIB
{
    uint32_t    bytelength;
    uint32_t    height;
    uint32_t    width;
    uint16_t    colorPlanes;
    uint16_t    bitsPerPixel;
    uint32_t    compressionMethod;
    uint32_t    imageSize;
} DIB;


typedef struct Bmp
{
    DIB         dib_header;
    RGB24      *pixels;
    int         pixelCount;
    uint8_t     *key;
} Bmp;

void read_bmp(Bmp *bmp, FILE *file);
void scrambleData(Bmp *bmp, int startPixel, int endPixel);

#endif /* BMPFILE_H */