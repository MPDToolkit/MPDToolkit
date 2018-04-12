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

#Get parameters.ini file
paramFile = open(os.path.join( os.path.dirname(sys.argv[0]), "parameters.ini"), "r")      #current path is "..\bin\analyze.py", therefore need to backup to root folder "bin"

#Default values in dictionaries
RXDParams = {"RxThreshold":90.0, "RxChiThreshold":0.999}
DXDParams = {"LineGaussianIter":0, "LineDilationIter":1, "LineBilatBlurColor":75,"LineBilatBlurSpace":75, "LineCannyEdgeLowerBound":100,"LineCannyEdgeThreshold":0, "CornerGaussianIter":0,"CornerErosionIter":1,"CornerBilateralColor":200,"CornerBilateralSpace":500, "CornerMaxDistance":75, "CornerNumPoints":3}
#Add code to read in the parameters from the file Here to overwrite the defaults
for line in paramFile:
    if line[0] == '#':
        pass
    else:
        #split the line into a list
        splitLine=line.strip().split("=")
        #see if parameter is in our dictionary
        #splitline[1] is 1 for default, 0 for User value
        if splitLine[0] in DXDParams and int(splitLine[1]) is 0:
            DXDParams[splitLine[0]] = float(splitLine[3])
            #print("set parameter: %s", splitLine[0])
        if splitLine[0] in RXDParams and int(splitLine[1]) is 0:
            RXDParams[splitLine[0]] = float(splitLine[3])
            #print("set parameter: %s", splitLine[0])





#--------------------------------------------------------------------------------------------------------
#Outputs backend stsatus to the frontend
def status(flag, in_str=None):

    if flag is None or in_str is None:
        return

    if flag == '-i-':	#Initialization
        print("{0} {1}".format( flag, in_str ))

    if flag == '-d-':	#Detected
        print("{0} {1} {2:.3f}ms {3:.6f}%".format( flag, in_str[0], in_str[1], in_str[2]) )

    if flag == '-o-':	#Other
        print("{0} {1} {2:.3f}ms {3:.6f}%".format( flag, in_str[0], in_str[1], in_str[2]) )


    if flag == '-f-':   #Finished
        print('-f- Finished Image Analysis...\n')
        print("\n{0} image(s) analyzed\n".format( in_str[1] ))
        print("Average elapsed time: {0:.3f} ms".format( (in_str[0] * 1000) / in_str[1] ) )
        print("Total elapsed time: {0:.3f} sec\n".format( in_str[0] ) )

    if flag == '-e-':	#Error
        print('-e- ')
        print("Exception occurred in analyze.py: \n")
        print( in_str + "\n")

    sys.stdout.flush()

#--------------------------------------------------------------------------------------------------------

def openFolder(path):
    #if __name__ == '__main__':
    global job_path
    job_path = path

    #Check for user error
    if os.path.isfile(path):
        openFile(path)
        return
    else:
        global copy_folder
        copy_folder = os.path.join( path, copy_folder)

        #Looks in the copy folder and adds the correct file types to the image list
        #This is a case insensitive version
        if os.path.exists(copy_folder):
            for file in glob.glob(os.path.join(copy_folder, '*')):
                ext = os.path.splitext(file)[-1]
                if ext.lower() in extensions:
                    img_list.append(file)
        else:
            for file in glob.glob(os.path.join(path, '*')):
                ext = os.path.splitext(file)[-1]
                if ext.lower() in extensions:
                    img_list.append(file)

    #Update the global job_path variable
    global detected_folder
    global other_folder

    #Create the output directory if it does not exist
    if not os.path.exists(os.path.join( job_path, detected_folder)):
        os.makedirs(os.path.join( job_path, detected_folder))

    if not os.path.exists(os.path.join( job_path, other_folder)):
        os.makedirs(os.path.join( job_path, other_folder))

    detected_folder = os.path.join( job_path, detected_folder)
    other_folder = os.path.join( job_path, other_folder)


    if __name__ == '__main__':
        #Analyze the images
        args = parser.parse_args()
        total_time.start()
        status('-i-', 'Initialization completed')
        status('-i-', 'Beginning Image Analysis...')

    if __name__ == '__main__':  #In Windows you need to protect the thread creation froms each child thread. If not done, each child thread will create subthreads.
        if int(args.procNum) > 1:
            with Pool(int(args.procNum)) as p:
                p.map(run, img_list)
        else:
            for i in range(0, len(img_list)):
                run(img_list[i])

    if __name__ == '__main__':
        total_time.stop()

    if __name__ == '__main__':    #Replace with writing to log file
        status('-f-', [ total_time.get_time(), len(img_list) ] )

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
        cv.imwrite( rx[0] + ".jpg", rx[1])

    else:
        print("{0} {1} {2:.3f}_ms {3:.6f}_%".format( "-o-", rx[0], rx[2], rx[3]) )
        cv.imwrite( rx[0] + ".jpg", rx[1])

#--------------------------------------------------------------------------------------------------------

def readArgs(args):
    #if __name__ == '__main__':
    parser.add_argument("-F", "--folder", dest="folderPath", help="path to the folder containing images to process.", metavar="folder", default=None)
    parser.add_argument("-f", "--file", dest="filePath", help="path to the specific image to process", metavar="file", default=None)
    parser.add_argument("-t", "--threshold", dest="pixThreshold", help="Color threshold value between 0-1024 for finding anomalies", default=90.0, metavar="threshold")
    parser.add_argument("-p", "--processes", dest="procNum", help="Number of processes to create to process images", default=1, metavar="threads")

    args = parser.parse_args()
    if args.folderPath != None:
        openFolder(args.folderPath)
    elif args.filePath != None:
        openFile(args.filePath)

#--------------------------------------------------------------------------------------------------------

#Analyze the given image (img)
def run(img):

    #Call the algorithms
    rx = RXD(img, RXDParams)
    RxHeatmap = rx[1]
    dx = DebrisDetect(img, DXDParams, RxHeatmap)

    #Print merged results for frontend to read
    run_time = rx[2] + dx[2]
    img_name = rx[0]
    rx_stats = rx[3]
    final_heatmap = dx[1]
    #Append run info to the log file


    #RXD Debug
    if rx[4] == 'D' or dx[3] == 'D':
        #print("{0} {1} {2:.3f}ms {3:.6f}%".format( "-d-", img_name, run_time, rx_stats) )
        #sys.stdout.flush()
        results_str = [ rx[0], rx[2] + dx[2], rx[3] ]
        status('-d-', results_str)

        cv.imwrite(os.path.join( detected_folder, img_name + ".jpg"), final_heatmap)

    else:
        #print("{0} {1} {2:.3f}ms {3:.6f}%".format( "-o-", img_name, run_time, rx_stats) )
        #sys.stdout.flush()
        results_str = [ rx[0], rx[2] + dx[2], rx[3] ]
        status( '-o-', results_str)

        cv.imwrite(os.path.join( other_folder, img_name + ".jpg"), final_heatmap)






#--------------------------------------------------------------------------------------------------------

def main():
    #if __name__ == '__main__':
    try:
        readArgs(sys.argv)
    except Exception as e:
        #print("exception handled in analyze.py: \n")
        #print(str(e) + "\n")
        status('-e-', str(e) )
    return 0


main()
