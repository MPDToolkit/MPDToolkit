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
import skimage as ski
from scipy.stats import chi2
import matplotlib.pyplot as plt


import timer    #Custom Timer class

#========================================================================================================
#-------------------------------------Define optimization perameters-------------------------------------
#========================================================================================================

scale_value = 1

threshold_value = 0.999

colormap_value = cv.COLORMAP_JET

window_property = cv.WINDOW_KEEPRATIO

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
#Algorithm Steps

#Blur img
#Morphological operations
#Edge detection     

#IF positive results --> #Classification decision

#ELSE
#Blur img                                    
#Morphological operations
#Shi-Tomasi corner detection
#Local HSV Filtering
#Shi-Tomasi Outlier removal
#Shape generation
#Classification decision

#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------


#blur_img = cv.bilateralFilter(src_img, 5, 75, 75)

#Algorithm uses Gaussian blur
perprocess_img = cv.GaussianBlur(src_img,(5,5),0)

#cv.imshow('blur_img', blur_img)

perprocess_img = cv.dilate( perprocess_img, (5,5), iterations = 1)

#Use bilateral filtering to preserve edges
preprocess_img = cv.bilateralFilter(src_img, 9, 75, 75)

#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------

#Apply Canny edge detection
#Threshold values are variable in this example
def edgeDetect(arg):
    low_threshold = cv.getTrackbarPos('Low', 'edge_img')
    high_threshold = cv.getTrackbarPos('High', 'edge_img')

    edge_img = cv.Canny(preprocess_img, low_threshold, high_threshold)
    cv.imshow('edge_img', edge_img)


cv.namedWindow('edge_img', window_property)
cv.resizeWindow('edge_img', window_init_width, window_init_height)
cv.createTrackbar('Low', 'edge_img', 100, 1024, edgeDetect)
cv.createTrackbar('High', 'edge_img', 0, 1024, edgeDetect)
edge_img = cv.Canny(preprocess_img, 100, 0)
cv.imshow('edge_img', edge_img)

#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------
    

#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------


#cv.applyColorMap( rx_scores.astype(np.uint8), colormap_value )



#Stop the timer
t.stop()
print("Elapsed time: {0} ms".format(t.get_time(1000)) )


#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------





#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------


cv.namedWindow("src_img", window_property)
cv.resizeWindow('src_img', window_init_width, window_init_height)
cv.imshow("src_img", src_img)


#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------




#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------


#Wait until a key is pressed
cv.waitKey(0)   


#========================================================================================================
#--------------------------------------------Clean Up & Exit---------------------------------------------
#========================================================================================================


cv.destroyAllWindows()
exit()



#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------