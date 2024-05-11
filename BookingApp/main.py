import os
import datetime
import requests
import subprocess
import time
import youtube_dl 
from database import *
from youtube import *
from cooca import *
from sumary import *
import random
import threading
prev_video_url = ''
prev_title = ''
# Function to read API key from a text file


def send_message(message, UserId):
    user = getUser(UserId)
    print(user)
    url = f"https://api.telegram.org/bot{user[7]}/sendMessage?chat_id={user[8]}&text={message}"
    requests.get(url)  

# Function to download the video using yt-dlp
def download_video(video_url, title, channel_url, channel_name, UserId, CookaAccountId,index  ):
    try:
        # Run yt-dlp.exe
        with youtube_dl.YoutubeDL() as ydl:
            
            # Lấy thông tin về video
            duration = subprocess.getoutput(f"yt-dlp --print duration {video_url}")
            trimmed_text = duration.splitlines()[1].strip()
            if int(trimmed_text) > 1200:
                print("Video có thời lượng lớn hơn 20 phút, không thể tải về.")
                current_dateTime = datetime.datetime.now()
                create(str(video_url),title,title+ ".mp4",title+ ".mp4.txt","Duration > 20 mins" ,channel_name,current_dateTime,current_dateTime)
            else:
                num = str(random.randint(1,9999999))
                filename = subprocess.getoutput(f"yt-dlp  --write-description  --write-info-json --write-thumbnail -o .\\file\\{num}.%(ext)s {video_url}")
                print('filename',filename)
                print("1/5 :Download video thành công.")
                transcript =  cockatoo_convert(num+ ".mp4",title,CookaAccountId ,index)
                if(transcript!="") :
                    #send_message(f'Download video: {title} từ {channel_url}', UserId)
                    current_dateTime = datetime.datetime.now()
                    sumary = summarize(transcript)
                    create(str(video_url),title,num,sumary,"OK" ,channel_name,current_dateTime,current_dateTime)
        # UrlVideo, Title, VideoPath,FilePath,Status, ChannelYoutubeName,GetedDate,CreatedDate
    except Exception as e:
        print("Download video thất bại:", e)

def read_string_array(filename):
  data = []
  with open(filename, "r") as f:
    for line in f:
      data.append(line.strip())  # Remove trailing newline character
  return data

def crawlData(channel,index):
    channel_url = channel[2]
    print('Đang quét video từ channel :'+ channel_url)
    urls,titles = get_last_video(channel_url)
    if urls.__len__() > 0 and checkExist(urls[0]):
        print(f"Tìm thấy video mới: {titles[0]}")
        download_video(urls[0],titles[0] , channel_url, channel[2], channel[6],channel[5],index)
    else :
        print(f"Chưa có video mới từ channel: " + channel_url)

def work(cookaAccount,index) :
        print(cookaAccount)
        channels = getChannelYoutube(cookaAccount[0])      
        for channel in channels :  
            print(channel)
            # Continuous loop to check for new videos
            crawlData(channel,index) 
        
# Main function
def main():
    print('running........')
    while True:
        cookaAccounts = getCookaAccount()
        for i in range(len(cookaAccounts)):
            work(cookaAccounts[i],i)
        time.sleep(1200)

if __name__ == "__main__":
     _ = main()
