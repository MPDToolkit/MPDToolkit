import sys
import os
import cv2 as cv
import numpy as np
import scipy as sp
import spectral as spc
from scipy.stats import chi2
from multiprocessing import Pool
from argparse import ArgumentParser
import glob

import timer    #Timer class
import error    #ExceptionHandler

#Algorithm Libraries
from RXDetector import RXD

e = error.ExceptionHandler()

#Global Variables
img_list = []
job_path = str()        #<-- Don't remove
parser = ArgumentParser()
extensions = ("*.JPG","*.jpg","*.jpeg","*.JPEG", "*.png")

copy_folder = "Copy"    

#Analysis Parameters



#--------------------------------------------------------------------------------------------------------

def openFolder(path):
    #Check if argument is a file or a directory
    if os.path.isfile(path):    
        #img_list.append(path)
        openFile(path)
        return
    else:                      
        #Looks in the copy folder
        for ext in extensions:
            img_list.extend(glob.glob( os.path.join( path, copy_folder) + "/" + ext ))

    #Update the global job_path variable
    global job_path
    job_path = path

    #Analyze the images
    args = parser.parse_args()

    if int(args.procNum) > 1:
        with Pool(int(args.procNum)) as p:
            p.map(run, img_list)
    else:
        for i in range(0, len(img_list)):
            run(img_list[i])

#--------------------------------------------------------------------------------------------------------

#This is not supported in the full release
def openFile(path):

    rx = RXD(path)

    if rx[4] == 'D': 
        print("{0} {1} {2:.3f}_ms {3:.6f}_%".format( "Detected", rx[0], rx[2], rx[3]) )
        cv.imwrite( rx[0] + "_detected.jpg", rx[1])  

    else:
        print("{0} {1} {2:.3f}_ms {3:.6f}_%".format( "Other", rx[0], rx[2], rx[3]) )
        cv.imwrite( rx[0] + "_other.jpg", rx[1])

#--------------------------------------------------------------------------------------------------------    

def readArgs(args):
    
    parser.add_argument("-F", "--folder", dest="folderPath", help="path to the folder containing images to process.", metavar="folder", default=None)
    parser.add_argument("-f", "--file", dest="filePath", help="path to the specific image to process", metavar="file", default=None)
    parser.add_argument("-t", "--threshold", dest="pixThreshold", help="Color threshold value between 0-1024 for finding anomalies", default=90.0, metavar="threshold")
    parser.add_argument("-p", "--processes", dest="procNum", help="Number of processes to create to process images (NOT IMPLEMENTED)", default=1, metavar="threads")

    args = parser.parse_args()
    if args.folderPath != None:
        openFolder(args.folderPath)
    elif args.filePath != None:
        openFile(args.filePath)
        
#--------------------------------------------------------------------------------------------------------

#Analyze the given image (img)
def run(img):
    
    #Get the full paths for each results folder
    detected_folder = os.path.join(job_path, "Detected")
    other_folder = os.path.join(job_path, "Other")
    

    #Call the algorithms
    rx = RXD(img)



    #Merge Results



    #Print merged results for frontend to read


    #Append run info to the log file


    #RXD Debug
    if rx[4] == 'R': 
        print("{0} {1} {2:.3f}_ms {3:.6f}_%".format( '-R', rx[0], rx[2], rx[3]) )
        #print(os.path.join( detected_folder, rx[0] + "_detected.jpeg"))
        cv.imwrite(os.path.join( detected_folder, rx[0] + "_detected.jpg"), rx[1])  

    else:
        print("{0} {1} {2:.3f}_ms {3:.6f}_%".format( '-O', rx[0], rx[2], rx[3]) )
        cv.imwrite(os.path.join( other_folder, rx[0] + "_other.jpg"), rx[1])






#--------------------------------------------------------------------------------------------------------

def main():

    try:
        readArgs(sys.argv)
    except Exception as e:
        print("exception handled in analyze.py: \n")
        print(e + "\n")
    #Save the arguments passed into the python script


    #print("\nElapsed time: %f ms" % (t.get_time() * 1000))

    #print( e.retry(t.get_time, None, 5) )
    #print("Returned value: {0} \n".format(e.watch(t.get_time, '1000')) )

    #Open an image with OpenCV
    return 0


main()
