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

#Algorithm Libraries
from RXDetector import RXD
from DXDetector import DebrisDetect


#Global Variables
img_list = []
job_path = str()        #<-- Don't remove
parser = ArgumentParser()
extensions = (".jpg", ".jpeg", ".png")


detected_folder = "Detected"
other_folder = "Other"
copy_folder = "Copy"

#Analysis Parameters
total_time = timer.Timer()


#--------------------------------------------------------------------------------------------------------

def openFolder(path):
    if __name__ == '__main__':
        #Check for user error
        if os.path.isfile(path):
            openFile(path)
            return
        else:
            global copy_folder
            copy_folder = os.path.join( path, copy_folder)

            #Looks in the copy folder and adds the correct file types to the image list
            #This is a case insensitive version
            for file in glob.glob(os.path.join(copy_folder, '*')):
                ext = os.path.splitext(file)[-1]
                if ext.lower() in extensions:
                    img_list.append(file)

        #Update the global job_path variable
        global job_path
        job_path = path

        global detected_folder
        global other_folder

        #Create the output directory if it does not exist
        if not os.path.exists(os.path.join( job_path, detected_folder)):
            os.makedirs(os.path.join( job_path,detected_folder))

        if not os.path.exists(os.path.join( job_path, other_folder)):
            os.makedirs(os.path.join( job_path, other_folder))

        detected_folder = os.path.join( job_path,detected_folder)
        other_folder = os.path.join( job_path, other_folder)

        #Analyze the images
        args = parser.parse_args()

    if __name__ == '__main__':
        total_time.start()

    if int(args.procNum) > 1:
        if __name__ == '__main__':  #In Windows you need to protect the thread creation froms each child thread. If not done, each child thread will create subthreads.
            with Pool(int(args.procNum)) as p:
                p.map(run, img_list)
    else:
        for i in range(0, len(img_list)):
            run(img_list[i])

    if __name__ == '__main__':
        total_time.stop()

    #if __name__ == '__main__':    #Replace with writing to log file
        #Display time
        #print("\n{0} image(s) analyzed\n".format(len(img_list)))
        #print("Average elapsed time: {0:.3f} ms".format( total_time.get_time(1000) / len(img_list) ) )
        #print("Total elapsed time: {0:.3f} sec\n".format( total_time.get_time() ) )

#--------------------------------------------------------------------------------------------------------

#This is not supported in the full release
def openFile(path):
    #Check for user error
    if os.path.isdir(path):
        openFolder(path)
        return

    rx = RXD(path)

    if rx[4] == 'D':
        print("{0} {1} {2:.3f}_ms {3:.6f}_%".format( "-d-", rx[0], rx[2], rx[3]) )
        cv.imwrite( rx[0] + "_detected.jpg", rx[1])

    else:
        print("{0} {1} {2:.3f}_ms {3:.6f}_%".format( "-o-", rx[0], rx[2], rx[3]) )
        cv.imwrite( rx[0] + "_other.jpg", rx[1])

#--------------------------------------------------------------------------------------------------------

def readArgs(args):
    if __name__ == '__main__':
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

    #Call the algorithms
    rx = RXD(img)
    dx = DebrisDetect(img, rx[1])



    #Merge Results



    #Print merged results for frontend to read


    #Append run info to the log file


    #RXD Debug
    if rx[4] == 'D':
        print("{0} {1} {2:.3f}_ms {3:.6f}_%".format( "-d-", rx[0], rx[2], rx[3]) )
        cv.imwrite(os.path.join( detected_folder, rx[0] + "_detected.jpg"), rx[1])

    else:
        print("{0} {1} {2:.3f}_ms {3:.6f}_%".format( "-o-", rx[0], rx[2], rx[3]) )
        cv.imwrite(os.path.join( other_folder, rx[0] + "_other.jpg"), rx[1])






#--------------------------------------------------------------------------------------------------------

def main():
    if __name__ == '__main__':
        try:
            readArgs(sys.argv)
        except Exception as e:
            print("exception handled in analyze.py: \n")
            print(str(e) + "\n")

    return 0


main()
