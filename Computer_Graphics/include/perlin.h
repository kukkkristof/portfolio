#ifndef PERLIN_H
#define PERLIN_H

/**
 * Slight modification for random seed generation
 * Source : https://gist.github.com/nowl/828013
*/

extern double Perlin_Get2d(double x, double y, double freq, int depth, int _seed);

#endif  // PERLIN_H 