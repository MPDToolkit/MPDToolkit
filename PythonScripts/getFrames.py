import sys
import cv2
import os

args = sys.argv
video_name = args[1]
vidcap = cv2.VideoCapture(video_name)
success,image = vidcap.read()
count = 0
success = True

if not os.path.exists("frames"):
        os.makedirs("frames")

while success:
    cv2.imwrite("frames/frame%d.jpg" % count, image)     # save frame as JPEG file
    success,image = vidcap.read()
    count += 1

    