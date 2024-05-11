from googletrans import Translator
import os
import sys
def read_string_array(filename):
  data = []
  with open(filename, "r", encoding="utf8") as f:
    for line in f:
      data.append(line.strip())  # Remove trailing newline character
  return data

def tao_file_txt(ten_file, textlines ):
  try:
    with open(ten_file, "w", encoding="utf-8") as f:
        for i in range(len(textlines)):
            f.write(textlines[i].inner_html + ".")
    return True
  except Exception as e:
    print(f"Lỗi khi tạo file: {e}")
    return False

def haha(filepath):
    lines = read_string_array(filepath)
    arr = lines[0].split('.')
    translator = Translator()
    res = []
    with open(filepath + ".eng.txt", "w", encoding="utf-8") as f:
        for line in arr:
            translations = translator.translate(line)
            f.write(translations.text + ".")
            # f.write(line + ".")

if __name__ == "__main__":
    filepath = sys.argv[1]
    haha(filepath)
    