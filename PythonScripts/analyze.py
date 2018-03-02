import sys
import cv2 as cv
import numpy as np
import scipy as sp
import spectral as spc

import timer    #Timer class
import error    #EceptionHandler

e = error.ExceptionHandler()

t = timer.Timer()
t.start()

#Save the arguments passed into the python script
args = sys.argv


t.stop()

print("\nElapsed time: %f ms" % (t.get_time() * 1000))
print("Elapsed time: {0} ms\n".format(t.get_time(1000)) )

#print( e.retry(t.get_time, None, 5) )
print("Returned value: {0} \n".format(e.watch(t.get_time, '1000')) )

print("Imported Arguments:")
idx = 0
for a in args:
    print("%i\t" % idx + a)



#Open an image with OpenCV
if len(args) > 1:
    cv.namedWindow('Image', cv.WINDOW_KEEPRATIO)       #WINDOW_NORMAL --> allows the user to adjust the window size, WINDOW_KEEPRATIO --> allows the user to adjust the window size while maintaining the image ratio
    img = cv.imread(args[1])
    cv.imshow('Image', img)
    cv.waitKey()    #Wait until a key is pressed
    cv.destroyWindow('Image')

print("\n")

