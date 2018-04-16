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
#import matplotlib.pyplot as plt

import timer

#----------------------------------------------------Analyze Image----------------------------------------------------

def RXD( image_file, Params):

    #Analysis Variables
    scale_value = 1.0
    chi_threshold = Params["RxChiThreshold"]
    anomaly_threshold = Params["RxThreshold"]

    #Output Variables
    colormap_value = cv.COLORMAP_JET

    #Statistics Variables
    t = timer.Timer()


    #Attempt to Analyze
    t.start()

    #Read the input image
    src_img = cv.imread( image_file )

    #Extract the name of the original image from its path
    result_name = os.path.split(image_file)[1].split(".")[0]

    #If needed, scale image
    if scale_value != 1.0:
        height, width = src_img.shape[:2]
        src_img = cv.resize( src_img, (int( width * scale_value ), int( height * scale_value)), interpolation= cv.INTER_LINEAR)


    #Calculate the rx scores for the image
    rx_scores = spc.rx(src_img)

    #Calculate the reference bands
    #rx_bands = src_img.shape[-1]       #Removed and integrated into chi function in order to save of memory usage

    #Apply a threshold to the rx scores using the chi-square percent point function
    rx_chi = chi2.ppf( chi_threshold, src_img.shape[-1])

    #Create a mask with the threshold values
    rx_mask = (1 * (rx_scores > rx_chi))

    #Apply the mask to the raw rx_scores
    rx_mask = rx_mask * rx_scores

    #Percentage of anomalies above the annomaly_threshold
    stats = ((rx_mask >= anomaly_threshold).sum() / rx_mask.size ) * 100.0

    #Flag the image as a Result (R) or Other (O)
    if np.max(rx_mask) >= anomaly_threshold:
        flag = 'D'
    else:
        flag = 'O'

    #Apply a colormap
    #heatmap = cv.applyColorMap( rx_mask.astype(np.uint8), colormap_value )
    heatmap = rx_mask

    t.stop()

    return result_name, heatmap, t.get_time(1000), stats, flag
