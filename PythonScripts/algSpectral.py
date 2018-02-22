#implement the algorithm described in the Spectral research paper

import sys
import cv2 as cv
import numpy as np
import scipy as sp
import spectral as spc
from scipy.stats import chi2
from sklearn.preprocessing import MinMaxScaler



import timer    #Timer class




t = timer.Timer()


#Save the arguments passed into the python script
args = sys.argv

#Open an image with OpenCV
if len(args) <= 1:
    print("Argument error: No input image")
    exit()




       #WINDOW_NORMAL --> allows the user to adjust the window size, WINDOW_KEEPRATIO --> allows the user to adjust the window size while maintaining the image ratio

raw_img = cv.imread(args[1]) 

#raw_img = cv.GaussianBlur(raw_img, (21,21), 4.5)

scale_value = 0.2

height, width = raw_img.shape[:2]
src_img = cv.resize(raw_img, (int(width*scale_value), int(height*scale_value)), interpolation=cv.INTER_LINEAR)


t.start()

#Calculate the RX scores
#img_stats = spc.calc_stats(src_img)
rx_scores = spc.rx(src_img)

#Apply chi-square percent point function such that only values < 0.1% are kept
rx_chi = chi2.ppf(0.001, rx_scores)

#Normalize values such that they are between the range [0,255] (RGB images are in this range)
scaler = MinMaxScaler(feature_range=(0, 255), copy=False)
scaler.fit_transform(rx_chi)

rx_img = cv.applyColorMap(rx_chi.astype(np.uint8), cv.COLORMAP_JET)

t.end()
print("Elapsed time: {0} ms".format(t.get_time(1000)) )


cv.namedWindow('rx_img', cv.WINDOW_AUTOSIZE)
cv.imshow("rx_img", rx_img)

#cv.imwrite("rx_img_255.jpg", rx_img)

cv.namedWindow("src_img", cv.WINDOW_AUTOSIZE)
cv.imshow("src_img", src_img)



cv.waitKey(0)    #Wait until a key is pressed
cv.destroyAllWindows()

exit()
