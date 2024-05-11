from DrissionPage import ChromiumPage
from DrissionPage import ChromiumOptions
import os
from  database import *


def read_string_array1(filename):
  data = []
  with open(filename, "r") as f:
    for line in f:
      data.append(line.strip())  # Remove trailing newline character
  return data


def tao_file_txt(ten_file, textlines ):
  try:
    with open(ten_file, "w", encoding="utf-8") as f:
        for i in range(len(textlines)):
            f.write(textlines[i].inner_html + ".")
    print(f"5/5: Tạo file thành công")
    return True
  except Exception as e:
    print(f"Lỗi khi tạo file: {e}")
    return False
  
def cockatoo_convert(filename, title,CookaAccountId,index):
    folder = os.path.join(os.getcwd())
    print(f"Tiến hành tạo file sub....")
    try:
        file_path = os.path.join(folder,"file", filename)
        co = ChromiumOptions().auto_port() 
        co.set_user(user="Profile " + str(index)) 
        #co.set_argument('--guest')
        co.headless()
        #co.incognito()
        p = ChromiumPage(co)        
        p.set.load_strategy.eager()
        #login
        print('2/5: Đăng nhập cockatoo ..')
        p.get('https://www.cockatoo.com/login')

        cockatoo = getCookaAccountById(CookaAccountId)
        p.ele('@id=email').input(cockatoo[0])
        p.ele('@id=password').input(cockatoo[1])
        p.ele('.cursor-pointer text-md font-light flex w-full justify-center rounded-sm bg-cyan-500 px-2 py-4 text-white shadow-sm animate ease-in-out duration-75 hover:bg-cyan-600 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-blue-600').click()
        
        p.wait(1)
        #get 
        print('3/5: Upload file ..')
        p.ele('.uppy-DragDrop-input').input(file_path)
        p.wait.ele_displayed('.p-2 bg-green-50 text-right rounded-md text-sm font-medium text-green-800 hover:bg-green-100 focus:outline-none',240)
        p.wait(1)
        p.ele('.p-2 bg-green-50 text-right rounded-md text-sm font-medium text-green-800 hover:bg-green-100 focus:outline-none').click()
        p.wait(2)
     
        p.wait.ele_deleted('.rounded-md bg-purple-50 shadow-sm p-4',240)
        #p.wait.title_change(title,120)
        p.ele('.w-full').click()
        p.wait.ele_displayed('.inline py-1 m-0',240)
        #timelines = p.eles('.flex mt-1 text-sm font-normal text-left justify-left items-left content-left text-gray-500 hover:text-gray-600 rounded-sm')
        textlines = p.eles('.inline py-1 m-0')
        transcript = ""
        for i in range(len(textlines)):
            transcript =  transcript + textlines[i].inner_html + "."
        return transcript
        # print('4/5: Tạo file txt ..')

        # ten_file = os.path.join(os.getcwd(), "ketqua",filename + ".txt" )
        # tao_file_txt(ten_file, textlines)   
        # p.quit()    
        # return textlines
    except Exception as e:
        print(f"Error: {e}")
        return "False"
