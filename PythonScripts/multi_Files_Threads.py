#   Missing Person Detection Tool 
#
#
# References:
#    - Spectral Analysis Code: http://www.spectralpython.net/algorithms.html
#         - Code can be found under the section: Target Detectors --> RX Anomaly Detector
#


import sys
import glob
import os
import cv2 as cv
import numpy as np
import scipy as sp
import spectral as spc
from scipy.stats import chi2
from multiprocessing import Pool

import timer            #Custom Timer class
import RXDetector       #Spectral Algorithm





#========================================================================================================
#-------------------------------------Define optimization perameters-------------------------------------
#========================================================================================================

multithread = True
num_threads = 6


#Common window properties
window_property = cv.WINDOW_KEEPRATIO

#Initial window size
window_init_width = 750
window_init_height = 500

anomaly_folder = "result"
not_anomaly_folder = "not_result"


#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------

#Create a Timer
t = timer.Timer()
rxd = RXDetector.RXD()

#Get the arguments passed into the python script
args = sys.argv

#Check if an image was provided
if len(args) <= 1:
    print("Argument error: No input image")
    exit()

#--------------------------------------------------------------------------------------------------------

#Create the output directory if it does not exist
if not os.path.exists(anomaly_folder):
    os.makedirs(anomaly_folder)

if not os.path.exists(not_anomaly_folder):
    os.makedirs(not_anomaly_folder)


#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------

argv = args[1] 

extensions = ("*.JPG","*.jpg","*.jpeg","*.JPEG")
img_list = []

 #Check if argument is a file or a directory
if os.path.isfile(argv):    #If: argv is a file
    img_list.append(argv)
else:                       #Else: argv is a directory
    for ext in extensions:
        img_list.extend(glob.glob( argv + "/" + ext ))


t.start()

if multithread:
    with Pool(num_threads) as p:
        p.map(rxd.analyze, img_list)

else:
    for i in range(0, len(img_list)):
        rx_img = rxd.analyze(img_list[i])

t.stop()




#========================================================================================================
#--------------------------------------------Clean Up & Exit---------------------------------------------
#========================================================================================================

#Clean up all window resources
#cv.destroyAllWindows()

print("\n{0} image(s) analyzed\n".format(len(img_list)))
print("Average elapsed time: {0} ms".format( t.get_time(1000) / len(img_list) ) )
print("Total elapsed time: {0} sec\n".format( t.get_time() ) )

exit()



#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------