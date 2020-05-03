import time
import csv
from datetime import datetime
import os

targetExe = '..\\bin\\myRunner.exe'

def GetExecutionTime(nbProcessors):
    start = time.time()
    cmd = targetExe + ' ' + str(nbProcessors)
    os.system(cmd)
    end = time.time()
    duration = round(end - start)
    return duration

csvfile = open('report.csv', 'w', newline='') 
spamwriter = csv.writer(csvfile, delimiter=',', quotechar=',', quoting=csv.QUOTE_MINIMAL)
spamwriter.writerow(['nbProcessors', 'ExecutionTimeSec'])
for i in range(1, 10):
    duration = GetExecutionTime(i)
    dnow = datetime.now()
    now = dnow.strftime("%H:%M:%S")
    print(now, ": nbProcessors", i, " duration: ",duration)
    spamwriter.writerow([i, duration])
    csvfile.flush()
csvfile.close()

#result of simulation:
#nbProcessors,ExecutionTimeSec
#1,30
#2,21
#3,21  <<<<<<<<<<<<<<<<<
#4,23
#5,25
#6,27
#7,28
#8,29
#9,29
#10,29
