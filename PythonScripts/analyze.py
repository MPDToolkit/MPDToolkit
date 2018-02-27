import sys
import cv2 as cv
import numpy as np
import scipy as sp
import spectral as spc
from argparse import ArgumentParser
import glob

import timer    #Timer class
import error    #ExceptionHandler

e = error.ExceptionHandler()
t = timer.Timer()

def openFolder(folderPath, threshold):
    for filename in glob.glob('*.png'):
        openFile(filename, threshold)
    pass

def openFile(filePath, threshold):
    t.start()
    cv.namedWindow('Image', cv.WINDOW_KEEPRATIO)       #WINDOW_NORMAL --> allows the user to adjust the window size, WINDOW_KEEPRATIO --> allows the user to adjust the window size while maintaining the image ratio
    img = cv.imread(filePath)
    cv.imshow('Image', img)
    t.stop()
    print("Elapsed time: {0} ms\n".format(t.get_time(1000)) )
    cv.waitKey()    #Wait until a key is pressed
    cv.destroyWindow('Image')
    print("\n")

def readArgs(args):

    parser = ArgumentParser()

    parser.add_argument("-F", "--folder", dest="folderPath", help="path to the folder containing images to process.", metavar="PATH", default=None)
    parser.add_argument("-f", "--file", dest="filePath", help="path to the specific image to process", metavar="PATH", default=None)
    parser.add_argument("-t", "--threshold", dest="pixThreshold", help="Color threshold value between 0-1 for finding anomalies", default=0.2)
    args = parser.parse_args()
    if args.folderPath != None:
        openFolder(args.folderPath, args.pixThreshold)
    elif args.filePath != None:
        openFile(args.filePath, args.pixThreshold)




def main():


    #Save the arguments passed into the python script
    args = sys.argv
    readArgs(args)

    #print("\nElapsed time: %f ms" % (t.get_time() * 1000))

    #print( e.retry(t.get_time, None, 5) )
    #print("Returned value: {0} \n".format(e.watch(t.get_time, '1000')) )

    #Open an image with OpenCV
    return 0


main()
