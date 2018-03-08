#   Missing Person Detection Tool 
#
#
# References:
#    - Spectral Analysis Code: http://www.spectralpython.net/algorithms.html
#         - Code can be found under the section: Target Detectors --> RX Anomaly Detector
#


import sys
import math
import cv2 as cv, cv2
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

height, width = src_img.shape[:2]
avg_dim = ((width+height)/2)
avg_dim = int((avg_dim/3))
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



#Algorithm uses Gaussian blur
pre_img = cv.GaussianBlur(src_img, (5,5), 0)

#cv.imshow('blur_img', blur_img)
#perprocess_img = cv.erode( perprocess_img, (51,51), iterations = 1 )
pre_img = cv.dilate( pre_img, (5,5), iterations = 1)

pre_img = cv.bilateralFilter(pre_img, 9, 2, 2)

#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------

#Apply Canny edge detection
#Threshold values are variable in this example
def edgeDetect(arg):

	low_threshold = cv.getTrackbarPos('th1', 'edge_img')
	high_threshold = cv.getTrackbarPos('th2', 'edge_img')

	clr = cv.getTrackbarPos('bilat Color', 'edge_img')
	dist = cv.getTrackbarPos('bilat Distance', 'edge_img')
	d = cv.getTrackbarPos('Kernel', 'edge_img')
	
	
	pre_img = cv.GaussianBlur(src_img, (5,5), 0)
	pre_img = cv.dilate( pre_img, (5,5), iterations = 1)
	pre_img = cv.bilateralFilter(pre_img, 9, clr, dist)

	edge_img = cv.Canny(pre_img, low_threshold, high_threshold)
	mask_img = cv.bitwise_or(pre_img, pre_img, mask=edge_img)

	lines = cv2.HoughLines(edge_img, 1, float(np.pi / 180.0), avg_dim)    #avg_dim
	#plines = cv2.HoughLinesP(edge_img, 1.0, float(np.pi / 180.0), 50, None, 30, 2)

	#
	if lines is not None:
		for i in range(0, len(lines)):
			rho = lines[i][0][0]
			theta = lines[i][0][1]
			a = math.cos(theta)
			b = math.sin(theta)
			x0 = a * rho
			y0 = b * rho
			pt1 = (int(x0 + 1000*(-b)), int(y0 + 1000*(a)))
			pt2 = (int(x0 - 1000*(-b)), int(y0 - 1000*(a)))
			cv2.line(pre_img, pt1, pt2, (0,0,255), 3, cv2.LINE_AA)

	#if plines is not None:
	#    for i in range(0, len(plines)):
	#        l = plines[i][0]
	#        #cv2.line(cdst, (l[0], l[1]), (l[2], l[3]), (0,0,255), 3, cv2.LINE_AA)
	#        cv2.line(pre_img, (l[0], l[1]), (l[2], l[3]), (0,0,255), 3, cv2.LINE_AA)
#

	
	cv.imshow('edge_img', edge_img)
	cv.imshow('Blur', pre_img)
	cv.imshow('Mask', mask_img)

cv.namedWindow('edge_img', window_property)
cv.resizeWindow('edge_img', window_init_width, window_init_height)

cv.namedWindow("Blur", window_property)
cv.resizeWindow('Blur', window_init_width, window_init_height)

cv.namedWindow("Mask", window_property)
cv.resizeWindow('Mask', window_init_width, window_init_height)


cv.createTrackbar('th1', 'edge_img', 50, 1000, edgeDetect)
cv.createTrackbar('th2', 'edge_img', 100, 1000, edgeDetect)

cv.createTrackbar('bilat Color', 'edge_img', 2, 1000, edgeDetect)
cv.createTrackbar('bilat Distance', 'edge_img', 2, 10, edgeDetect)
cv.createTrackbar('Kernel', 'edge_img', 0, 100, edgeDetect)

edge_img = cv.Canny(pre_img, 50, 100)
mask_img = cv.bitwise_or(pre_img, pre_img, mask=edge_img)

lines = cv2.HoughLines(edge_img, 1, float(np.pi / 180.0), avg_dim)    #avg_dim
#plines = cv2.HoughLinesP(edge_img, 1.0, float(np.pi / 180.0), 50, None, 30, 2)

#
if lines is not None:
	for i in range(0, len(lines)):
		rho = lines[i][0][0]
		theta = lines[i][0][1]
		a = math.cos(theta)
		b = math.sin(theta)
		x0 = a * rho
		y0 = b * rho
		pt1 = (int(x0 + 1000*(-b)), int(y0 + 1000*(a)))
		pt2 = (int(x0 - 1000*(-b)), int(y0 - 1000*(a)))
		cv2.line(pre_img, pt1, pt2, (0,0,255), 3, cv2.LINE_AA)

#if plines is not None:
#	for i in range(0, len(plines)):
#		l = plines[i][0]
#		#cv2.line(cdst, (l[0], l[1]), (l[2], l[3]), (0,0,255), 3, cv2.LINE_AA)
#		cv2.line(pre_img, (l[0], l[1]), (l[2], l[3]), (0,0,255), 3, cv2.LINE_AA)
#


cv.imshow('edge_img', edge_img)
cv.imshow('Blur', pre_img)
cv.imshow('Mask', mask_img)


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


#cv.namedWindow("src_img", window_property)
#cv.resizeWindow('src_img', window_init_width, window_init_height)
#cv.imshow("src_img", src_img)
#cv.imshow("src_img", pre_img)


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