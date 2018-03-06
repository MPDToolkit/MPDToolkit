import sys
import cv2
import numpy as np
import scipy as sp
import spectral as spc
import math

import timer    #Custom Timer class

#========================================================================================================
#-------------------------------------Define optimization perameters-------------------------------------
#========================================================================================================

scale_value = 1

threshold_value = 0.999

colormap_value = cv2.COLORMAP_JET

window_property = cv2.WINDOW_KEEPRATIO

window_init_width = 1920
window_init_height = 1080

#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------

#Create a Timer
#t = timer.Timer()

#Get the arguments passed into the python script
args = sys.argv

#Check if an image was provided
if len(args) <= 1:
    print("Argument error: No input image")
    exit()

#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------


#Start the timer
#t.start()

#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------

#Read the source image
try:
    img = cv2.imread(args[1]) 
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
    src_img = cv2.resize( src_img, (int( width * scale_value ), int( height * scale_value)), interpolation= cv2.INTER_LINEAR)

#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------
width, height = img.shape[:2]
avg_dim = (((width+height)//2)//3)*2
print(avg_dim)
# Gaussian Blur
proc_img = cv2.GaussianBlur(img, (5,5), 0)

# Kernel Dilation
proc_img = cv2.dilate(proc_img, (5,5), iterations=1)

# Bilateral Blur
proc_img = cv2.bilateralFilter(proc_img, 9, 75, 75)

# Low sensitivity Hough transform
dst = cv2.Canny(proc_img, 100, 0)
cdst = cv2.cvtColor(dst, cv2.COLOR_GRAY2BGR)
lines = cv2.HoughLines(dst, 0.7, float(np.pi / 180.0), avg_dim)

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
		cv2.line(proc_img, pt1, pt2, (0,0,255), 3, cv2.LINE_AA)
	# canny
	cv2.namedWindow("s", window_property)
	cv2.resizeWindow('s', window_init_width, window_init_height)
	cv2.imshow('s',dst)
	# hough
	cv2.namedWindow("dst", window_property)
	cv2.resizeWindow('dst', window_init_width, window_init_height)
	cv2.imshow('dst',proc_img)
	cv2.waitKey(0)

else:
	# if no edges detected
	# Gaussian Blur
	proc2_img = cv2.GaussianBlur(img, (5,5), 0)

	# Erosion
	proc2_img = cv2.erode(proc2_img, (5,5), iterations=1)

	# Stronger Bilateral Blur
	proc2_img = cv2.bilateralFilter(proc2_img, 9, 100, 100)

	# Shi-Tomasi
	# Green pixels
	lower_green = np.array([31, 48, 91])
	upper_green = np.array([51, 68, 171])
	
	gray = cv2.cvtColor(proc2_img, cv2.COLOR_BGR2GRAY)
	corners = cv2.goodFeaturesToTrack(gray, 100, 0.02, 10)
	corners = np.int0(corners)
	for i in corners:
		x, y = i.ravel()
		# Indexing into an opencv image is backwards
		pixel1 = img[y-1, x-1]
		pixel2 = img[y-1, x]
		pixel3 = img[y-1, x+1]
		pixel4 = img[y, x-1]
		pixel5 = img[y, x+1]
		pixel6 = img[y+1, x-1]
		pixel7 = img[y+1, x]
		pixel8 = img[y+1, x+1]
		# check for local
		if ((pixel1 >= lower_green).all() and (pixel1 <= upper_green).all()) and \
			((pixel2 >= lower_green).all() and (pixel2 <= upper_green).all()) and \
			((pixel3 >= lower_green).all() and (pixel3 <= upper_green).all()) and \
			((pixel4 >= lower_green).all() and (pixel4 <= upper_green).all()) and \
			((pixel5 >= lower_green).all() and (pixel5 <= upper_green).all()) and \
			((pixel6 >= lower_green).all() and (pixel6 <= upper_green).all()) and \
			((pixel7 >= lower_green).all() and (pixel7 <= upper_green).all()) and \
			((pixel8 >= lower_green).all() and (pixel8 <= upper_green).all()):
			continue
		else:
			cv2.circle(proc2_img, (x,y), 3, 255, -1)
	cv2.namedWindow('shi', window_property)
	#cv2.resizeWindow('shi', window_init_width, window_init_height)
	cv2.imshow('shi',proc2_img)
	cv2.waitKey(0)

	# Filter based on HSV values of local pixels
	
	
	# Filter out outliers


	# Generate line segments (shape generation)


	# Classify

#Stop the timer
#t.stop()
#print("Elapsed time: {0} ms".format(t.get_time(1000)) )

cv2.destroyAllWindows()
exit()

