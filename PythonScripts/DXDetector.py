import os
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

window_property = cv2.WINDOW_KEEPRATIO

window_init_width = 1600
window_init_height = 900

#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------
"""
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
	DebrisDetect(args[1])
"""
#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------
def DebrisDetect(img_path, Params, heatmap = None):
	# Create a Timer
	t = timer.Timer()
	t.start()

	img = cv2.imread(img_path)
	# Name of the original file
	#result_name = img_path.split("/")[-1].split(".")[0]
	result_name = os.path.split(img_path)[1].split(".")[0]

	#If needed, scale image
	if scale_value != 1:
		height, width = img.shape[:2]
		img = cv2.resize( img, (int( width * scale_value ), int( height * scale_value)), interpolation= cv2.INTER_LINEAR)
    #--------------------------------------------------------------------------------------------------------
    # Line Detector
    #--------------------------------------------------------------------------------------------------------

	# Attain dimensions of the image
	height, width = img.shape[:2]
	avg_dim = (((width+height)//2)//3)*2

	# Create copy of original img
	line_check = copy.deepcopy(img)

	# Gaussian Blur
	proc_img = cv2.GaussianBlur(line_check, (5,5), Params["LineGaussianIter"])

    # Kernel Dilation
	proc_img = cv2.dilate(proc_img, (5,5), iterations=Params["LineDilationIter"])

    # Bilateral Blur
	proc_img = cv2.bilateralFilter(proc_img, 9, Params["LineBilatBlur"], Params["LineBilatBlur"])

    # Low sensitivity Hough transform
	dst = cv2.Canny(proc_img, 100, 0)
	cdst = cv2.cvtColor(dst, cv2.COLOR_GRAY2BGR)
	lines = cv2.HoughLines(dst, 0.7, float(np.pi / 180.0), int(avg_dim/2))

	# If lines are found
	if lines is not None:
		if heatmap is not None:
			for i in range(0, len(lines)):
				rho = lines[i][0][0]
				theta = lines[i][0][1]
				a = math.cos(theta)
				b = math.sin(theta)
				x0 = a * rho
				y0 = b * rho
				pt1 = (int(x0 + 1000*(-b)), int(y0 + 1000*(a)))
				pt2 = (int(x0 - 1000*(-b)), int(y0 - 1000*(a)))
				cv2.line(heatmap, pt1, pt2, (0,0,255), 3, cv2.LINE_AA)
			#Stop the timer
			t.stop()
			return result_name, heatmap, t.get_time(1000), 'D'
		else:
			for i in range(0, len(lines)):
				rho = lines[i][0][0]
				theta = lines[i][0][1]
				a = math.cos(theta)
				b = math.sin(theta)
				x0 = a * rho
				y0 = b * rho
				pt1 = (int(x0 + 1000*(-b)), int(y0 + 1000*(a)))
				pt2 = (int(x0 - 1000*(-b)), int(y0 - 1000*(a)))
				cv2.line(line_check, pt1, pt2, (0,0,255), 3, cv2.LINE_AA)
			#Stop the timer
			t.stop()
			return result_name, line_check, t.get_time(1000), 'D'

	# Otherwise, corner detection
	else:
		# more copies of the original img and heatmap is there is one
		src = copy.deepcopy(img)
		corner_check = copy.deepcopy(img)
		img_copy = copy.deepcopy(img)
		if heatmap is not None:
			heatmap_copy = copy.deepcopy(heatmap)

		# Gaussian Blur
		proc2_img = cv2.GaussianBlur(corner_check, (5,5), Params["CornerGaussianIter"])

		# Erosion
		proc2_img = cv2.erode(proc2_img, (5,5), iterations=Params["CornerErosionIter"])

		# Stronger Bilateral Blur
		proc2_img = cv2.bilateralFilter(proc2_img, 16, Params["CornerBilateralColor"], Params["CornerBilateralSpace"])

		# Shi-Tomasi
		gray = cv2.cvtColor(proc2_img, cv2.COLOR_BGR2GRAY)
		corners = cv2.goodFeaturesToTrack(gray, 50, 0.02, 10)
		corners = np.int0(corners)
		hsv_img = cv2.cvtColor(img_copy, cv2.COLOR_BGR2HSV)

		# Green and brown HSV values
		lower_green = np.array([33, 87, 78])
		upper_green = np.array([53, 107, 158])
		lower_brown = np.array([167, 13, 186])
		upper_brown = np.array([179, 33, 255])

		kept_corners = []

		for i in corners:
			x, y = i.ravel()
			pixels = []
			if y == 0 and x == 0:
				pixels.append(hsv_img[y, x+1])
				pixels.append(hsv_img[y+1, x])
				pixels.append(hsv_img[y+1, x+1])
			elif y == 0 and x == (height - 1):
				pixels.append(hsv_img[y, x-1])
				pixels.append(hsv_img[y+1, x])
				pixels.append(hsv_img[y+1, x-1])
			elif x == 0 and y == (width - 1):
				pixels.append(hsv_img[y, x+1])
				pixels.append(hsv_img[y-1, x])
				pixels.append(hsv_img[y-1, x+1])
			elif x == (height - 1) and y == (width - 1):
				pixels.append(hsv_img[y, x-1])
				pixels.append(hsv_img[y-1, x])
				pixels.append(hsv_img[y-1, x-1])
			elif y == 0:
				pixels.append(hsv_img[y, x-1])
				pixels.append(hsv_img[y, x+1])
				pixels.append(hsv_img[y+1, x-1])
				pixels.append(hsv_img[y+1, x])
				pixels.append(hsv_img[y+1, x+1])
			elif x == 0:
				pixels.append(hsv_img[y-1, x])
				pixels.append(hsv_img[y-1, x+1])
				pixels.append(hsv_img[y, x+1])
				pixels.append(hsv_img[y+1, x])
				pixels.append(hsv_img[y+1, x+1])
			elif y == (width - 1):
				pixels.append(hsv_img[y-1, x-1])
				pixels.append(hsv_img[y-1, x])
				pixels.append(hsv_img[y-1, x])
				pixels.append(hsv_img[y, x-1])
				pixels.append(hsv_img[y, x+1])
			elif x == (height - 1):
				pixels.append(hsv_img[y-1, x-1])
				pixels.append(hsv_img[y-1, x])
				pixels.append(hsv_img[y, x-1])
				pixels.append(hsv_img[y+1, x-1])
				pixels.append(hsv_img[y+1, x])
			else:
				pixels.append(hsv_img[y-1, x-1])
				pixels.append(hsv_img[y-1, x])
				pixels.append(hsv_img[y-1, x+1])
				pixels.append(hsv_img[y, x-1])
				pixels.append(hsv_img[y, x+1])
				pixels.append(hsv_img[y+1, x-1])
				pixels.append(hsv_img[y+1, x])
				pixels.append(hsv_img[y+1, x+1])

			# Obtain HSV values for the image
			hue, sat, value = hsv_img[y,x]

			# Filter based on HSV values of local pixels vs the green and brown HSV values
			if all(((pixel >= lower_green).all() and (pixel <= upper_green).all()) or ((pixel >= lower_brown).all() and (pixel <= upper_brown).all()) for pixel in pixels):
				continue
			# Filter if all pixels have low saturation and high value
			if all((pixel[1] <= 20 and pixel[2] >= 220) for pixel in pixels):
				continue

			# Look for corners that are near other corners - finding outliers
			close = False
			for j in corners:
				x2, y2 = j.ravel()
				pix1 = (x , y, 0)
				pix2 = (x2, y2, 0)
				dist = distance.euclidean(pix1, pix2)
				if dist < Params["CornerMaxDistance"] and dist != 0:
					close = True
					break
			if close:
				if heatmap is not None:
					cv2.circle(heatmap_copy, (x,y), 3, 255, 10)
				else:
					cv2.circle(src, (x,y), 3, 255, 10)
				kept_corners.append(i)

		# This list is to count the number of circles in a polygon when connected
		connected_pairs = []
		for i in kept_corners:
			x, y = i.ravel()
			connected_pairs.append([(x,y)])

		# Connect the circles close to eachother to determine polygons
		for i in kept_corners:
			x, y = i.ravel()
			for j in kept_corners:
				x2, y2 = j.ravel()
				pix1 = (x , y, 0)
				pix2 = (x2, y2, 0)
				dist = distance.euclidean(pix1, pix2)
				if dist < Params["CornerMaxDistance"] and dist != 0:
					if heatmap is not None:
						cv2.line(heatmap_copy, (x,y), (x2,y2), (0,0,255), 1, cv2.LINE_AA)
					else:
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

		# If any of the polygons have more than 3 connected circles, then we have a hit
		if any(len(t) > Params["CornerNumPoints"] for t in connected_pairs):
			#Stop the timer
			t.stop()
			if heatmap is not None:
				return result_name, heatmap_copy, t.get_time(1000), 'D'
			else:
				return result_name, src, t.get_time(1000), 'D'

		else:
			t.stop()
			return result_name, heatmap, t.get_time(1000), 'O'
	exit()
