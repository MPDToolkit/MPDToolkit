import sys
import cv2 as cv
import numpy as np
import scipy as sp
import spectral as spc

import time
import timer



t = timer.Timer()
t.start()

args = sys.argv

print("...Python code begin...")
print("Argc:\tArgv:")
idx = 0
for a in args:
    print("%i\t" % idx + a)
    idx += 1

t.end()
print("Elapsed time: %f ms" % (t.get_time() * 1000))
print("...Python code end...\n")