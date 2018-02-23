#implement the algorithm described in the Spectral research paper

import sys
import cv2 as cv
import numpy as np
import scipy as sp
import spectral as spc
from scipy.stats import chi2

import timer    #Timer class



t = timer.Timer()

#Save the arguments passed into the python script
args = sys.argv

#Open an image with OpenCV
if len(args) <= 1:
    print("Argument error: No input image")
    exit()





raw_img = cv.imread(args[1]) 


scale_value = 1#0.2

height, width = raw_img.shape[:2]
#src_img = cv.resize(raw_img, (int(width*scale_value), int(height*scale_value)), interpolation=cv.INTER_LINEAR)

src_img = raw_img

#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------

t.start()


rx_scores = spc.rx(src_img)
rx_bands = src_img.shape[-1]

#--------------------------------------------------------------------------------------------------------

#Apply chi-square percent point function such that only values < 0.1% are kept
rx_chi = chi2.ppf(0.999, rx_bands)

rx_img = cv.applyColorMap(rx_scores.astype(np.uint8), cv.COLORMAP_JET)


t.end()
print("Elapsed time: {0} ms".format(t.get_time(1000)) )


cv.namedWindow('rx_img', cv.WINDOW_KEEPRATIO)
cv.resizeWindow('rx_img', 750, 500)
cv.imshow("rx_img", rx_img)


cv.namedWindow("src_img", cv.WINDOW_KEEPRATIO)
cv.resizeWindow('src_img', 750, 500)
cv.imshow("src_img", src_img)


cv.waitKey(0)    #Wait until a key is pressed
cv.destroyAllWindows()

exit()











































