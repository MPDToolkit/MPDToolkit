#   Missing Person Detection Tool 
#
#
# References:
#    - Spectral Analysis Code: http://www.spectralpython.net/algorithms.html
#         - Code can be found under the section: Target Detectors --> RX Anomaly Detector
#


import sys
import math
import cv2 as cv
import numpy as np
import scipy as sp
#import skimage as ski
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
perprocess_img = cv.GaussianBlur(src_img, (5,5), 0)

#cv.imshow('blur_img', blur_img)
#perprocess_img = cv.erode( perprocess_img, (51,51), iterations = 1 )
perprocess_img = cv.dilate( perprocess_img, (5,5), iterations = 1)

#Use bilateral filtering to preserve edges
preprocess_img = cv.bilateralFilter(src_img, 5, 20, 1000)

#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------

#Apply Canny edge detection
#Threshold values are variable in this example
def edgeDetect(arg):
	low_threshold = cv.getTrackbarPos('Low', 'edge_img')
	high_threshold = low_threshold * 2.0 #(cv.getTrackbarPos('Low', 'edge_img') / 10.0)*2.0
	rho = cv.getTrackbarPos('rho', 'edge_img') / 10.0

	mll = cv.getTrackbarPos('MinLineLength', 'edge_img')
	mld = cv.getTrackbarPos('MaxLineDistance', 'edge_img')


	edge_img = cv.Canny(preprocess_img, low_threshold, high_threshold)
	cdst = cv.cvtColor(edge_img, cv.COLOR_GRAY2BGR)

	if rho == 0:
		rho = 0.01

	#lines = cv.HoughLines(edge_img, rho, np.pi / 180, 150)
	plines = cv.HoughLinesP(edge_img, rho, np.pi / 180, 50, None, mll, mld)

	# Draw the lines
	if plines is not None:
		for i in range(0, len(plines)):
			l = plines[i][0]
			cv.line(cdst, (l[0], l[1]), (l[2], l[3]), (0,0,255), 3, cv.LINE_AA)
	
	cv.imshow('edge_img', cdst)
	

cv.namedWindow('edge_img', window_property)
cv.resizeWindow('edge_img', window_init_width, window_init_height)
cv.createTrackbar('Low', 'edge_img', 200, 1000, edgeDetect)
cv.createTrackbar('rho', 'edge_img', 10, 100, edgeDetect)

cv.createTrackbar('MinLineLength', 'edge_img', 10, 1000, edgeDetect)
cv.createTrackbar('MaxLineDistance', 'edge_img', 5, 1000, edgeDetect)

edge_img = cv.Canny(preprocess_img, 200, 400)
cdst = cv.cvtColor(edge_img, cv.COLOR_GRAY2BGR)

plines = cv.HoughLinesP(edge_img, 1.0, np.pi / 180, 50, None, 10, 5)

# Draw the lines
if plines is not None:
	for i in range(0, len(plines)):
		l = plines[i][0]
		cv.line(cdst, (l[0], l[1]), (l[2], l[3]), (0,0,255), 3, cv.LINE_AA)

cv.imshow('edge_img', cdst)


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
#cv.imshow("src_img", src_img)
cv.imshow("src_img", preprocess_img)


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