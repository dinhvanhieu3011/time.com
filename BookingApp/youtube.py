#importing our used libraries

from bs4 import BeautifulSoup
from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.chrome.options import Options
from webdriver_manager.chrome import ChromeDriverManager

"""---------------------------------------------------------------"""
#intiating the driver "Beta version"


def getting_source(url):
    #initiating the Chrome driver
    options=Options()
    options.add_argument("--headless")
    options.add_experimental_option("detach",True)
    driver=webdriver.Chrome(service=Service(ChromeDriverManager().install()),options=options)
    driver.get(url)
    page_source=driver.page_source
    driver.quit()
    return page_source

def getting_url_titles(page_source):
    #getting the urls and titles from the page we got the source of in the first function
    soup=BeautifulSoup(page_source,"html.parser")
    titles=soup.findAll(id="video-title-link")
    urls=[]
    t=[]
    for i in titles:
        url=i.get('href')
        url='https://www.youtube.com'+str(url)
        title=i.get('title')
        urls.append(url)
        t.append(title)
    return urls,t

def get_last_video(channel_url):
    urls,titles=getting_url_titles(page_source=getting_source(channel_url))
    return urls,titles










