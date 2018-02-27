import sys
import cv2
import numpy as np
import scipy as sp
import spectral as spc
import math

args = sys.argv
img = cv2.imread(args[1])

# resize if necessary
#proc_img = cv2.resize(img, (800,400))

#kernel = np.ones((5,5), np.uint8)

# Gaussian Blur
proc_img = cv2.GaussianBlur(img, (5,5), 0)

# Kernel Dilation
proc_img = cv2.dilate(proc_img, (5,5), iterations=1)

# Bilateral Blur
proc_img = cv2.bilateralFilter(proc_img, 9, 75, 75)

# Low sensitivity Hough transform
dst = cv2.Canny(proc_img, 50, 200, None, 3)
cdst = cv2.cvtColor(dst, cv2.COLOR_GRAY2BGR)
lines = cv2.HoughLines(dst, 1, np.pi / 180, 150, None, 0, 0)
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
		cv2.line(cdst, pt1, pt2, (0,0,255), 3, cv2.LINE_AA)

cv2.imshow('proc_img.jpg',cdst)

cv2.waitKey(0)
cv2.destroyAllWindows()
exit()

# TODO: Check hough transform for edges, if none proceed to below code section
# if no edges detected

# Gaussian Blur
proc2_img = cv2.GaussianBlur(img, (5,5), 0)

# Erosion
proc2_img = cv2.erode(proc2_img, (5,5), iterations=1)

# Stronger Bilateral Blur
proc_img = cv2.bilateralFilter(proc_img, 9, 100, 100)

# Shi-Tomasi


# Filter based on HSV values of local pixels


# Filter out outliers


# Generate line segments (shape generation)


# Classify