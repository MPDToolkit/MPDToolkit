#   Missing Person Detection Tool 
#
#
# References:
#    - Spectral Analysis Code: http://www.spectralpython.net/algorithms.html
#         - Code can be found under the section: Target Detectors --> RX Anomaly Detector
#


import sys
import cv2 as cv
import numpy as np
import scipy as sp
import spectral as spc
from scipy.stats import chi2
#import matplotlib.pyplot as plt

import timer    #Custom Timer class

#========================================================================================================
#-------------------------------------Define optimization perameters-------------------------------------
#========================================================================================================

#Scale multiple of the image that should be used. Decrease to improve performance.
scale_value = 1

#Threshold for anomaly detection
threshold_value = 0.999

#Defines the colormap to use
colormap_value = cv.COLORMAP_JET

#Common window properties
window_property = cv.WINDOW_KEEPRATIO

#Initial window size
window_init_width = 750
window_init_height = 500


#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------

#Create a Timer
t = timer.Timer()

#Get the arguments passed into the python script
args = sys.argv

#Check if an image was provided
if len(args) <= 1:
    print("Argument error: No input image")
    exit()

#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------


#Start the timer
t.start()


#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------


#Read the source image
try:
    src_img = cv.imread(args[1]) 

except OSError as e:
    print("OS error: {0}".format(e))
except ValueError as e:
    print("Value error: {0}".format(e))
except TypeError as e:
    print("Type error: {0}".format(e))
except:
    print("Unexpected error:", sys.exc_info()[0])



#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------


#If needed, scale image
if scale_value != 1:
    height, width = src_img.shape[:2]
    src_img = cv.resize( src_img, (int( width * scale_value ), int( height * scale_value)), interpolation= cv.INTER_LINEAR)


#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------


#Calculate the rx scores for the image
rx_scores = spc.rx(src_img)


#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------


#Calculate the reference bands
rx_bands = src_img.shape[-1]


#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------

#Apply a threshold to the rx scores using the chi-square percent point function
rx_chi = chi2.ppf( threshold_value, rx_bands)

#Create a mask with the threshold values
rx_mask = (1 * (rx_scores > rx_chi))

#Apply the mask to the raw rx_scores
rx_mask = rx_mask * rx_scores

#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------


#Apply a colormap
rx_img = cv.applyColorMap( rx_mask.astype(np.uint8), colormap_value )


#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------


#Stop the timer
t.stop()
print("Elapsed time: {0} ms".format(t.get_time(1000)) )


#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------

#Display the anomaly heatmap image
cv.namedWindow('rx_img', window_property)
cv.resizeWindow('rx_img', window_init_width, window_init_height)
cv.imshow("rx_img", rx_img)


#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------

#Display the original image
cv.namedWindow("src_img", window_property)
cv.resizeWindow('src_img', window_init_width, window_init_height)
cv.imshow("src_img", src_img)


#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------

#***    REMOVE IN FINAL RELEASE

#Debug display image
cv.namedWindow("rx_scores", window_property)
cv.resizeWindow('rx_scores', window_init_width, window_init_height)
cv.imshow("rx_scores", cv.applyColorMap( rx_scores.astype(np.uint8), colormap_value ))

#***

#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------


#Wait until a key is pressed
cv.waitKey(0)   


#========================================================================================================
#--------------------------------------------Clean Up & Exit---------------------------------------------
#========================================================================================================

#Clean up all window resources
cv.destroyAllWindows()
exit()



#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------