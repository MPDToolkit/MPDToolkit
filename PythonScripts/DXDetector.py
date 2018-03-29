import sys
import cv2
import numpy as np
import scipy as sp
import spectral as spc
from scipy.spatial import distance
import math

import copy


import timer	#Custom Timer class

#========================================================================================================
#-------------------------------------Define optimization parameters-------------------------------------
#========================================================================================================

scale_value = 1

threshold_value = 0.999

colormap_value = cv2.COLORMAP_JET

window_property = cv2.WINDOW_KEEPRATIO

window_init_width = 1600
window_init_height = 900

#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------

#Create a Timer
#t = timer.Timer()

#Get the arguments passed into the python script
def main():
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
	DebrisDetect(img)
#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------
def DebrisDetect(img_path, heatmap = None):
	img = cv2.imread(img_path)
	#If needed, scale image
	if scale_value != 1:
		height, width = img.shape[:2]
		img = cv2.resize( img, (int( width * scale_value ), int( height * scale_value)), interpolation= cv2.INTER_LINEAR)
    #--------------------------------------------------------------------------------------------------------
    #Line Detector
    #--------------------------------------------------------------------------------------------------------
	height, width = img.shape[:2]
	avg_dim = (((width+height)//2)//3)*2

	# create copy of original img
	line_check = copy.deepcopy(img)
	# Gaussian Blur
	proc_img = cv2.GaussianBlur(line_check, (5,5), 0)

    # Kernel Dilation
	proc_img = cv2.dilate(proc_img, (5,5), iterations=1)

    # Bilateral Blur
	proc_img = cv2.bilateralFilter(proc_img, 9, 75, 75)

    # Low sensitivity Hough transform
	dst = cv2.Canny(proc_img, 100, 0)
	cdst = cv2.cvtColor(dst, cv2.COLOR_GRAY2BGR)

	lines = cv2.HoughLines(dst, 0.7, float(np.pi / 180.0), int(avg_dim/2))
    #print(lines)
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
		# more copies of the original img
		src = copy.deepcopy(img)
		corner_check = copy.deepcopy(img)
		img_copy = copy.deepcopy(img)
		# if no edges detected
		# Gaussian Blur
		proc2_img = cv2.GaussianBlur(corner_check, (5,5), 0)

		# Erosion
		proc2_img = cv2.erode(proc2_img, (5,5), iterations=1)

		# Stronger Bilateral Blur
		proc2_img = cv2.bilateralFilter(proc2_img, 16, 200, 500)
		# Shi-Tomasi
		# Green and brown HSV values
		lower_green = np.array([33, 87, 78])
		upper_green = np.array([53, 107, 158])
		lower_brown = np.array([167, 13, 186])
		upper_brown = np.array([179, 33, 255])

		gray = cv2.cvtColor(proc2_img, cv2.COLOR_BGR2GRAY)

		corners = cv2.goodFeaturesToTrack(gray, 50, 0.02, 10)
		corners = np.int0(corners)
		hsv_img = cv2.cvtColor(img_copy, cv2.COLOR_BGR2HSV)

		kept_corners = []

		for i in corners:
			x, y = i.ravel()
			# Indexing into an opencv image is height, width
			pixel1 = hsv_img[y-1, x-1]
			pixel2 = hsv_img[y-1, x]
			pixel3 = hsv_img[y-1, x+1]
			pixel4 = hsv_img[y, x-1]
			pixel5 = hsv_img[y, x+1]
			pixel6 = hsv_img[y+1, x-1]
			pixel7 = hsv_img[y+1, x]
			pixel8 = hsv_img[y+1, x+1]

			hue, sat, value = hsv_img[y,x]
			# Filter based on HSV values of local pixels
			if (((pixel1 >= lower_green).all() and (pixel1 <= upper_green).all()) or ((pixel1 >= lower_brown).all() and (pixel1 <= upper_brown).all())) and \
				(((pixel2 >= lower_green).all() and (pixel2 <= upper_green).all()) or ((pixel2 >= lower_brown).all() and (pixel2 <= upper_brown).all())) and \
				(((pixel3 >= lower_green).all() and (pixel3 <= upper_green).all()) or ((pixel3 >= lower_brown).all() and (pixel3 <= upper_brown).all())) and \
				(((pixel4 >= lower_green).all() and (pixel4 <= upper_green).all()) or ((pixel4 >= lower_brown).all() and (pixel4 <= upper_brown).all())) and \
				(((pixel5 >= lower_green).all() and (pixel5 <= upper_green).all()) or ((pixel5 >= lower_brown).all() and (pixel5 <= upper_brown).all())) and \
				(((pixel6 >= lower_green).all() and (pixel6 <= upper_green).all()) or ((pixel6 >= lower_brown).all() and (pixel6 <= upper_brown).all())) and \
				(((pixel7 >= lower_green).all() and (pixel7 <= upper_green).all()) or ((pixel7 >= lower_brown).all() and (pixel7 <= upper_brown).all())) and \
				(((pixel8 >= lower_green).all() and (pixel8 <= upper_green).all()) or ((pixel8 >= lower_brown).all() and (pixel8 <= upper_brown).all())):
				continue
			# Filter if all pixels have low saturation and high value
			if (((pixel1[1] <= 20) and (pixel1[2]>=220)) and \
				((pixel2[1] <= 20) and (pixel2[2]>=220)) and \
				((pixel3[1] <= 20) and (pixel3[2]>=220)) and \
				((pixel4[1] <= 20) and (pixel4[2]>=220)) and \
				((pixel5[1] <= 20) and (pixel5[2]>=220)) and \
				((pixel6[1] <= 20) and (pixel6[2]>=220)) and \
				((pixel7[1] <= 20) and (pixel7[2]>=220)) and \
				((pixel8[1] <= 20) and (pixel8[2]>=220))):
				continue

			close = False
			for j in corners:
				x2, y2 = j.ravel()
				pix1 = (x , y, 0)
				pix2 = (x2, y2, 0)
				dist = distance.euclidean(pix1, pix2)
				if dist < 75 and dist != 0:
					close = True
					break
			if close:
				cv2.circle(src, (x,y), 3, 255, 10)
				kept_corners.append(i)
		connected_pairs = []
		for i in kept_corners:
			x, y = i.ravel()
			connected_pairs.append([(x,y)])

		for i in kept_corners:
			x, y = i.ravel()
			for j in kept_corners:
				x2, y2 = j.ravel()
				pix1 = (x , y, 0)
				pix2 = (x2, y2, 0)
				dist = distance.euclidean(pix1, pix2)
				if dist < 75 and dist != 0:
					cv2.line(src, (x,y), (x2,y2), (0,0,255), 1, cv2.LINE_AA)
					for k in connected_pairs:
						if (x,y) in k and (x2,y2) in k:
							break;
						elif (x,y) in k:
							k.append((x2,y2))
							break
						elif (x2,y2) in k:
							k.append((x,y))
							break
		if any(len(t) > 3 for t in connected_pairs):
			# TODO something here if we find debris
		cv2.namedWindow('shi', window_property)
		cv2.resizeWindow('shi', window_init_width, window_init_height)
		cv2.imshow('shi',src)
		cv2.waitKey(0)
	#Stop the timer
	#t.stop()
	#print("Elapsed time: {0} ms".format(t.get_time(1000)) )
	cv2.destroyAllWindows()
	return heatmap
	exit()
