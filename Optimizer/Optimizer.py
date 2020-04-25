import time
import csv
from datetime import datetime
import os

targetExe = 'D:\\prog\\PasswordRunner\\PasswordRunner\\Runner\\bin\\Release\\netcoreapp3.1\\myRunner.exe'

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
for i in range(1, 100):
    duration = GetExecutionTime(i)
    dnow = datetime.now()
    now = dnow.strftime("%H:%M:%S")
    print(now, ": nbProcessors", i, " duration: ",duration)
    spamwriter.writerow([i, duration])
    csvfile.flush()
csvfile.close()