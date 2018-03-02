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



class RXD:

    def __init__(self, scale=1, chi=0.999, threshold=90.0, color=cv.COLORMAP_JET):
        self.heatmap = None
        self.rx_mask = None
        self.rx_scores = None
        self.scale_value = scale
        self.chi_threshold = chi
        self.anomaly_threshold = threshold
        self.stats = 0.0
        self.colormap_value = color
        self.result_name = ""
        self.t = timer.Timer()
        
        

    #--------------------------------------------------------------------------------------------------------
    def analyze(self, image_file):

        #Read the source image
        try:
            self.t.start()

            src_img = cv.imread( image_file ) 

            self.result_name = image_file.split("/")[-1].split(".")[0]

            #If needed, scale image
            if self.scale_value != 1:
                height, width = src_img.shape[:2]
                src_img = cv.resize( src_img, (int( width * scale_value ), int( height * scale_value)), interpolation= cv.INTER_LINEAR)


            #Calculate the rx scores for the image
            self.rx_scores = spc.rx(src_img)

            #Calculate the reference bands
            #rx_bands = src_img.shape[-1]

            #Apply a threshold to the rx scores using the chi-square percent point function
            rx_chi = chi2.ppf( self.chi_threshold, src_img.shape[-1])

            #Create a mask with the threshold values
            self.rx_mask = (1 * (self.rx_scores > rx_chi))

            #Apply the mask to the raw rx_scores
            self.rx_mask = self.rx_mask * self.rx_scores

            self.stats = ((self.rx_mask >= self.anomaly_threshold).sum() / self.rx_mask.size ) * 100.0

            #Apply a colormap
            self.heatmap = cv.applyColorMap( self.rx_mask.astype(np.uint8), self.colormap_value )

            #return self.heatmap

            self.t.stop()

            if np.max(self.rx_mask) >= self.anomaly_threshold : 
                print("{0} {1} {2}_ms {3}% --> {4}".format( '--', self.result_name, self.t.get_time(1000), self.stats, "Result") )
                cv.imwrite(os.path.join( "result", self.result_name + "_result.jpg"), self.heatmap)
                

            else:
                print("{0} {1} {2}_ms {3}% --> {4}".format( '--', self.result_name, self.t.get_time(1000), self.stats, "Other") )
                cv.imwrite(os.path.join( "not_result", self.result_name + "_other.jpg"), self.heatmap)


        except OSError as e:
            print("OS error: {0}".format(e))
        except ValueError as e:
            print("Value error: {0}".format(e))
        except TypeError as e:
            print("Type error: {0}".format(e))
        #except:
        #    print("Unexpected error:", sys.exc_info()[0])

    
    

