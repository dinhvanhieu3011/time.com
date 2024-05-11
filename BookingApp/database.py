import sqlite3
import os.path

def connect_db():
    """Creates a connection to the database."""
    dir_path = os.getcwd().replace('yt-dlp','')
    db_path = os.path.join(dir_path, "booking.db")
    return sqlite3.connect(db_path)


def checkExist(url):
    con = connect_db()
    cur = con.cursor()
    cur.execute("SELECT * FROM Files WHERE UrlVideo=?", (url,))
    config = cur.fetchall()
    con.close()
    if(config.__len__() == 0 ) :
        return True
    else :
        return False


def create(UrlVideo, Title, VideoPath,FilePath,Status, ChannelYoutubeName,GetedDate,CreatedDate ):
    con = connect_db()
    cur = con.cursor()
    cur.execute("INSERT INTO Files (UrlVideo, Title, VideoPath,FilePath,Status, ChannelYoutubeName,GetedDate,CreatedDate) VALUES (?, ?, ?, ?,?,  ?,?, ?)",
                (UrlVideo, Title, VideoPath,FilePath,Status, ChannelYoutubeName,GetedDate,CreatedDate))
    print('Tạo dữ liệu thành công')
    con.commit()
    con.close()
    return True
def createNews(UrlVideo, Title, VideoPath,FilePath,Status, ChannelYoutubeName,GetedDate,CreatedDate ):
    con = connect_db()
    cur = con.cursor()
    cur.execute("INSERT INTO News (UrlVideo, Title, VideoPath,FilePath,Status, ChannelYoutubeName,GetedDate,CreatedDate) VALUES (?, ?, ?, ?,?,  ?,?, ?)",
                (UrlVideo, Title, VideoPath,FilePath,Status, ChannelYoutubeName,GetedDate,CreatedDate))
    print('Tạo dữ liệu thành công')
    con.commit()
    con.close()
    return True

def getCookaAccount():
    con = connect_db()
    cur = con.cursor()
    cur.execute("SELECT * FROM CookaAccounts")
    data = cur.fetchall()
    con.close()
    return data

def getCookaAccountById(Id):
    con = connect_db()
    cur = con.cursor()
    cur.execute("SELECT Username,Password FROM CookaAccounts where Id = ? ",(Id,))
    data = cur.fetchone()
    con.close()
    return data

def getCookaAccountStatus(Id):
    con = connect_db()
    cur = con.cursor()
    cur.execute("SELECT IsStop FROM CookaAccounts WHERE Id=?",(Id,))
    data = cur.fetchone()
    con.close()
    return data



def getUser(UserId):
    con = connect_db()
    cur = con.cursor()
    cur.execute("SELECT * FROM Users WHERE UserId=?", (UserId,))
    data = cur.fetchone()
    con.close()
    return data

def getChannelYoutube(CookaAccountId):
    con = connect_db()
    cur = con.cursor()
    cur.execute("SELECT * FROM ChannelYoutubes WHERE CookaAccountId=?", (CookaAccountId,))
    data = cur.fetchall()
    con.close()
    return data