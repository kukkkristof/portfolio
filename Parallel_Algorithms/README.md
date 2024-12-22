<h1>PROJECT SPECIFICATION</h1>

>### DESCRIPTION:
>>A .BMP image de-detailing software, generating a key to an image, scrambling the data with **pthread**, and later unscrambling with **OpenMP**.

>## POSIX
>Steps:
><ol>
>   <li>Read the 14 byte data chunk to get information about the image</li>
>   <li>Go to the first pixel <b>pointer</b> defined in the file</li>
>   <li>Read all bytes containing the image</li>
>   <li>For each pixel generate a push direction and a push value</li>
>   <li>Bit shift the pixel by <u>value</u> to the <u>direction</u></li>
>   <li>Save this value to a key array</li>
>   <li>Rewrite the binary data corresponding to the image array in the file to the scrambled byte sequence</li>
>   <li>Create a file for the key, to be able to decipher the original image</li>
></ol>

>## OPENMP
>Steps:
><ol>
>   <li>Read the 14 byte data chunk to get information about the image</li>
>   <li>Go to the first pixel <b>pointer</b> defined in the file</li>
>   <li>Read all bytes containing the image</li>
>   <li>Read the key bytes from the key file</li>
>   <li>Bit shift the pixel by <u>value</u> in the keys corresponding segment to opposite of the <u>direction</u> in the key.</li>
>   <li>Rewrite the binary data corresponding to the image array in the file to the scrambled byte sequence</li>
></ol>

>## C#
>### DESCRIPTION:
>>Count the primes until a number given as a parameter on a thread count also given as a parameter.
>
>Steps:
><ol>
>   <li>Read the data from the arguments</li>
>   <li>Slice the interval into <u>thread count</u> pieces</li>
>   <li>For each thread, count the primes in the current slice</li>
>   <li>After all threads are finished working, SUM up the slice values</li>
></ol>

>## SOURCES
>> BMP format breakdown: https://en.wikipedia.org/wiki/BMP_file_format

<sub><sub>
Kristóf Kukk | <b>P2MZHY</b> @ University of Miskolc, 2024
</sub></sub>