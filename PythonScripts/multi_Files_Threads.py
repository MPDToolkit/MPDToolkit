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
import RXDetector as rxd      #Spectral Algorithm





#========================================================================================================
#-------------------------------------Define optimization perameters-------------------------------------
#========================================================================================================

multithread = True
num_threads = 8


#Common window properties
window_property = cv.WINDOW_KEEPRATIO

#Initial window size
window_init_width = 750
window_init_height = 500

result_folder = "Result"
other_folder = "Other"


#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------

#Create a Timer
total = timer.Timer()
#rxd = RXDetector.RXD()

#Get the arguments passed into the python script
args = sys.argv

#Check if an image was provided
if len(args) <= 1:
    print("Argument error: No input image")
    exit()

#--------------------------------------------------------------------------------------------------------

#Create the output directory if it does not exist
if not os.path.exists(result_folder):
    os.makedirs(result_folder)

if not os.path.exists(other_folder):
    os.makedirs(other_folder)


#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------

argv = args[1] 

extensions = ("*.JPG","*.jpg","*.jpeg","*.JPEG")
result_list = [{}]
img_list = []

 #Check if argument is a file or a directory
if os.path.isfile(argv):    #If: argv is a file
    img_list.append(argv)
else:                       #Else: argv is a directory
    for ext in extensions:
        img_list.extend(glob.glob( argv + "/" + ext ))


total.start()

if multithread:
    with Pool(num_threads) as p:
        #result_list.append( p.map(rxd.analyze, img_list) )
        p.map(rxd.analyze, img_list)

else:
    for i in range(0, len(img_list)):
        rx_img = rxd.analyze(img_list[i])
        #result_list.append( rxd.analyze(img_list[i]) )
total.stop()


#for r in result_list:
#    if r.get('flag') == 'R':
#        print("{0} {1} {2:.3f}_ms {3:.6f}_%".format( '-' + r.get('flag'), r.get('name'), r.get('time'), r.get('stats') ) )
#        cv.imwrite(os.path.join( result_folder, r.get('name') + "_result.jpeg"), r.get('heatmap'))
#    else:
#        print("{0} {1} {2:.3f}_ms {3:.6f}_%".format( '-' + r.get('flag'), r.get('name'), r.get('time'), r.get('stats') ) )
#        cv.imwrite(os.path.join( other_folder, r.get('name') + "_other.jpeg"), r.get('heatmap'))


#========================================================================================================
#--------------------------------------------Clean Up & Exit---------------------------------------------
#========================================================================================================

#Clean up all window resources
#cv.destroyAllWindows()

print("\n{0} image(s) analyzed\n".format(len(img_list)))
print("Average elapsed time: {0:.3f} ms".format( total.get_time(1000) / len(img_list) ) )
print("Total elapsed time: {0:.3f} sec\n".format( total.get_time() ) )

exit()



#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------
#--------------------------------------------------------------------------------------------------------