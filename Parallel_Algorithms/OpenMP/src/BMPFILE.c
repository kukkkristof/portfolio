#include "BMPFILE.h"
#include <stdlib.h>

void read_bmp(Bmp *bmp, FILE *file)
{
    uint32_t FirstPixelPosition;
    fread(&FirstPixelPosition, 4, 1, file);
    fread(&(bmp->dib_header), 24, 1, file);

    fseek(file, FirstPixelPosition, SEEK_SET);
    bmp->pixels = malloc(bmp->dib_header.imageSize); 
    bmp->pixelCount = bmp->dib_header.width * bmp->dib_header.height;

    fread(bmp->pixels, bmp->dib_header.imageSize, 1, file);
}

void unscrambleData(Bmp *bmp, int startPixel, int endPixel)
{

    for(int i = 0; i < 10; i++) rand();

    for(int i = startPixel; i < endPixel; i++)
    {

        uint8_t pushDirection = !(bmp->key[i]>>7&0x01);
        uint8_t pushValue;

        uint32_t result;

        pushValue = bmp->key[i]&0x7F;

        uint32_t colorValue;

        memcpy(&colorValue, &(bmp->pixels[i]), 3);

        uint32_t cutbits = colorValue;

        if(pushDirection)
        {
            cutbits>>=24-pushValue;
            colorValue<<=pushValue;
        }
        else
        {
            cutbits<<=24-pushValue;
            colorValue>>=pushValue;
        }

        colorValue = (colorValue|cutbits)&0x00FFFFFF;
        pushValue |= pushDirection<<7;

        memcpy(&(bmp->pixels[i]), &colorValue, 3);

        bmp->key[i] = pushValue;

    }
}
