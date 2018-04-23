import cv2
import numpy as np
import sys
import os



def frames_to_video(inputpath,outputpath,fps):
    image_array = []
    files = [f for f in os.listdir(inputpath) if os.path.isfile(os.path.join(inputpath, f))]
    #print(files)
    files.sort(key = lambda x: int(x[5:-4]))
    for i in range(len(files)):
        img = cv2.imread(os.path.join(inputpath, files[i]))
        size = (img.shape[1],img.shape[0])
        img = cv2.resize(img,size)
        image_array.append(img)
    fourcc = cv2.VideoWriter_fourcc('D', 'I', 'V', 'X')
    out = cv2.VideoWriter(outputpath,fourcc, int(fps), size)
    for i in range(len(image_array)):
        out.write(image_array[i])
    out.release()



inputpath = sys.argv[1]
outpath = sys.argv[2]
fps = sys.argv[3]

frames_to_video(inputpath,outpath,fps)