# -*- coding: utf-8 -*-
"""
This is a short script to randomly generate words based on the transition
probabilities of a real language, which are calculated from a list of words.

Input :
* A text file containing a list of words from a "real" language

Output
* A text file containing new words generated from that language
* An image file showing transition probabilities between letters

2015-10-27. David Louapre. sciencetonnante@gmail.com
http://sciencetonnante.wordpress.com
"""

import codecs
import re
import matplotlib.pyplot as plt
import numpy as np
from numpy.random import choice, seed

seed(1)

###############################################################################

# DEFINE INPUT AND OUTPUT
# =======================

# Input file containing one word per line, and its encoding
# Assumes one word per line but if the the words are followed by 
# a space, a tab, a slash, a comma, etc....the end of the line will be trimmed
dic_file = r"data/FR.txt"
encoding = "ISO-8859-1"

# Name of the output binary matrix, matrix image file and output txt file
count_file = r"count_FR.bin"
proba_matrix = r"matrix_FR.png"
outfile = r"output_FR.txt"

# For the random generator : what is the minimum and maximum number of letters
# in the words that we want to generate, and how many words for each length
smin, smax = 4, 12
K = 100

###############################################################################

# Read the dictionary and compute the occurrence of each trigram
# ==============================================================


dico = []  # to store the words of the dictionary

count = np.zeros((256, 256, 256), dtype='int32')
with codecs.open(dic_file, "r", encoding) as lines:
    for l in lines:
        # Trimming of the line :
        # Split on white space, tab, slash backslah or open parenthesis
        # and keep the first string, add EOL character
        l2 = re.split("[ /\t,(]", l)[0] + "\n"
        dico.append(l2[:-1])
        i, j = 0, 0
        for k in [ord(c) for c in list(l2)]:
            count[i, j, k] += 1
            i = j
            j = k
# Save the results for later use
count.tofile(count_file)

###############################################################################

# 2D plot
# =======

# This is an optional 2D plot showing bigram probabilities
# We have to do a partial sum on the 3D matrix to go fro trigram to bigram

count2D = count.sum(axis=0)
p2D = count2D.astype('float') / np.tile(sum(count2D.T), (256, 1)).T
p2D[np.isnan(p2D)] = 0

# For better contrast, we plot p^alpha instead of p
alpha = 0.33
p2Da = p2D ** alpha

# We display only letters a to z, ie ASCII from 97 to 123.
plt.figure(figsize=(8, 8))
plt.imshow(p2Da[97:123, 97:123], interpolation='nearest')
plt.axis('off')

for i in range(97, 123):
    plt.text(-1, i - 97, chr(i), horizontalalignment='center',
             verticalalignment='center')
    plt.text(i - 97, -1, chr(i), horizontalalignment='center',
             verticalalignment='center')
plt.savefig(proba_matrix)

###############################################################################

# GENERATE WORDS
# ==============

# Compute the probabilities by normalizing the counts
s = count.sum(axis=2)
st = np.tile(s.T, (256, 1, 1)).T
p = count.astype('float') / st
p[np.isnan(p)] = 0

# Generate words
f = codecs.open(outfile, "w", encoding)

for size in range(smin, smax + 1):
    total = 0

    while total < K:
        i, j = 0, 0
        res = u''
        while not j == 10:
            k = choice(range(256), 1, p=p[i, j, :])[0]
            res = res + chr(k)
            i, j = j, k
        if len(res) == 1 + size:
            x = res[:-1]
            if res[:-1] in dico:
                x = res[:-1] + "*"
            total += 1
            f.write(x + "\n")
f.close()
